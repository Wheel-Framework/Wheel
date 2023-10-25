using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
using Wheel;
using Wheel.AutoMapper;
using Wheel.Const;
using Wheel.Controllers;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;
using Wheel.DataSeeders;
using Wheel.Domain.Identity;
using Wheel.Email;
using Wheel.EntityFrameworkCore;
using Wheel.EventBus;
using Wheel.Graphql;
using Wheel.Hubs;
using Wheel.Json;
using Wheel.Localization;
using Wheel.Settings;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;
using Role = Wheel.Domain.Identity.Role;

var builder = WebApplication.CreateBuilder(args);
// Kestrel
builder.WebHost.ConfigureKestrel(options => 
{
    // Handle requests up to 50 MB
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 50;
});
// logging
Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .WriteTo.Async(c => c.Console())
    .WriteTo.Async(c => c.File("Logs/log.txt", rollingInterval: RollingInterval.Day))
    .CreateLogger();

builder.Host.UseSerilog();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<WheelAutofacModule>();
});

builder.Services.Configure<FormOptions>(options =>
{
    // Set the limit to 256 MB
    options.MultipartBodyLengthLimit = 1024 * 1024 * 256;
});

// Add services to the container.
builder.Services.AddMailKit(builder.Configuration);
builder.Services.AddChannelRLoacalEventBus();
//builder.Services.AddMediatRLocalEventBus();
builder.Services.AddCapDistributedEventBus(x =>
{
    x.UseEntityFramework<WheelDbContext>();

    x.UseSqlite(builder.Configuration.GetConnectionString("Default"));

    //x.UseRabbitMQ(configuration["RabbitMQ:ConnectionString"]);
    x.UseRedis(builder.Configuration["Cache:Redis"]);
});
builder.Services.AddAutoMapper();
builder.Services.AddIdGen(0);

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");


builder.Services.AddDbContext<WheelDbContext>(options =>
    options.UseSqlite(connectionString)
        .AddInterceptors(new WheelEFCoreInterceptor())
        .UseLazyLoadingProxies()
);

builder.Services.AddAuthentication(IdentityConstants.BearerScheme)
    .AddBearerToken(IdentityConstants.BearerScheme, options =>
    {
        options.Events = new BearerTokenEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<WheelDbContext>()
                .AddApiEndpoints();

builder.Services.AddWheelGraphQL();

builder.Services.AddSetting(typeof(WheelSettingSotre));

// Localizer
builder.Services.AddEFStringLocalizer<EFStringLocalizerStore>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMemoryCache();
var redis = await ConnectionMultiplexer.ConnectAsync(builder.Configuration["Cache:Redis"]);
builder.Services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(_ => redis);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConnectionMultiplexerFactory = async () => await Task.FromResult(redis);
});

builder.Services.AddDataProtection()
    .SetApplicationName("Wheel")
    .PersistKeysToStackExchangeRedis(redis);

builder.Services.AddSignalR()
    .AddJsonProtocol()
    .AddMessagePackProtocol()
    .AddStackExchangeRedis(builder.Configuration["Cache:Redis"]);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new Int32Converter());
    options.SerializerOptions.Converters.Add(new LongJsonConverter());
});

builder.Services.AddControllers()
    .AddApplicationPart(typeof(WheelControllerBase).Assembly)
    .AddControllersAsServices()
    .AddJsonOptions(configure =>
    {
        configure.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        configure.JsonSerializerOptions.Converters.Add(new Int32Converter());
        configure.JsonSerializerOptions.Converters.Add(new LongJsonConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //遍历所有xml并加载
    var binXmlFiles =
        new DirectoryInfo(string.IsNullOrWhiteSpace(AppDomain.CurrentDomain.BaseDirectory)
            ? Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
            : AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.xml", SearchOption.TopDirectoryOnly);
    foreach (var filePath in binXmlFiles.Select(item => item.FullName))
    {
        options.IncludeXmlComments(filePath, true);
    }
    string GetCustomerSchemaId(Type type, bool first = true)
    {
        var name = "";
        if (first)
            name = type.FullName;
        else
            name = type.Name;
        if (type.IsGenericType)
        {
            name = type.Name.Substring(0, type.Name.IndexOf("`"));
            name += "<";
            for (int i = 0; i < type.GenericTypeArguments.Length; i++)
            {
                var arg = type.GenericTypeArguments[i];
                name += GetCustomerSchemaId(arg, false);
                if (i < type.GenericTypeArguments.Length - 1)
                    name += ",";
            }
            name += ">";
        }
        return name;
    }

    options.CustomSchemaIds(type => GetCustomerSchemaId(type));
});

builder.Services.AddHealthChecks();

var app = builder.Build();

//初始化种子信息
await app.SeedData();

var forwardOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardOptions.KnownNetworks.Clear();
forwardOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardOptions);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseReDoc(options => options.RoutePrefix = "doc");

app.UseRequestLocalization(new RequestLocalizationOptions
{
    ApplyCurrentCultureToResponseHeaders = true,
    DefaultRequestCulture = new RequestCulture("zh-CN"),
    SupportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("zh-CN"),
            },
    SupportedUICultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("zh-CN"),
            }
});

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Application.Json;
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is BusinessException businessException)
        {
            var L = context.RequestServices.GetRequiredService<IStringLocalizerFactory>().Create(null);
            if (businessException.MessageData != null)
                await context.Response.WriteAsJsonAsync(new R { Code = businessException.Code, Message = L[businessException.Message, businessException.MessageData] });
            else
                await context.Response.WriteAsJsonAsync(new R { Code = businessException.Code, Message = L[businessException.Message] });
        }
        else
        {
            await context.Response.WriteAsJsonAsync(new R { Code = ErrorCode.InternalError, Message = exceptionHandlerPathFeature?.Error.Message });
        }
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

var webSocketOptions = new WebSocketOptions
{
};
app.UseWebSockets(webSocketOptions);
app.MapControllers();
app.MapGraphQL();
app.MapHub<NotificationHub>("/hubs/notification");
app.MapGroup("api/identity")
   .WithTags("Identity")
   .MapIdentityApi<User>();
app.MapHealthChecks("Health");
app.Run();
