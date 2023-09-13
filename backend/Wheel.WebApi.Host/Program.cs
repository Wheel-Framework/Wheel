using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Wheel;
using Wheel.Const;
using Wheel.Core.Exceptions;
using Wheel.Core.Dto;
using Wheel.Domain.Identity;
using Wheel.EntityFrameworkCore;
using Wheel.Localization;
using Wheel.AutoMapper;
using static System.Net.Mime.MediaTypeNames;
using Wheel.Hubs;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Wheel.EventBus;
using Role = Wheel.Domain.Identity.Role;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wheel.EntityFrameworkCore.SoftDelete;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<WheelAutofacModule>();
});

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");

// Add services to the container.
builder.Services.AddLocalEventBus();
builder.Services.AddDistributedEventBus(builder.Configuration);
builder.Services.AddAutoMapper();
builder.Services.AddIdGen(0);

builder.Services.AddDbContext<WheelDbContext>(options =>
options.UseSqlite(connectionString)
    .AddInterceptors(new SoftDeleteInterceptor())
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


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSingleton<IStringLocalizerFactory, EFStringLocalizerFactory>();

builder.Services.AddMemoryCache();
var redis = ConnectionMultiplexer.Connect(builder.Configuration["Cache:Redis"]);
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

builder.Services.AddControllers()
    .AddControllersAsServices();

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

var app = builder.Build();

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

app.UseRequestLocalization(new RequestLocalizationOptions
{
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
app.MapHub<NotificationHub>("/hubs/notification");
app.MapIdentityApi<User>();

app.Run();
