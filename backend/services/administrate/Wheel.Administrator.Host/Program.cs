using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using Wheel.Administrator.EntityFrameworkCore;
using Wheel.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Wheel.Administrator;
using Microsoft.AspNetCore.Http.Features;
using Wheel;
using IdGen.DependencyInjection;
using Wheel.AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using static System.Net.Mime.MediaTypeNames;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;
using Wheel.Administrator.Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Wheel.Administrator.Settings;
using Wheel.DataSeeders;
using System.Text.Json.Serialization;
using Wheel.Json;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Wheel.Administrator.Identity;
using Wheel.Authorization.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

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
    builder.RegisterModule<AdministratorAutofacModule>();
});

builder.Services.Configure<FormOptions>(options =>
{
    // Set the limit to 256 MB
    options.MultipartBodyLengthLimit = 1024 * 1024 * 256;
});

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");
builder.Services.AddDbContext<AdministratorDbContext>(options =>
    options.UseSqlite(connectionString)
        .AddInterceptors(new WheelEFCoreInterceptor())
        .UseLazyLoadingProxies()
);
builder.Services.AddIdentityCore<BackendUser>(o =>
                {
                    o.Stores.MaxLengthForKeys = 128;
                    o.Stores.ProtectPersonalData = false;

                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                })
                .AddUserManager<BackendUserManager>()
                .AddSignInManager<SignInManager<BackendUser>>()
                .AddRoles<BackendRole>()
                .AddUserStore<UserStore<BackendUser, BackendRole, AdministratorDbContext, string, BackendUserClaim, BackendUserRole, BackendUserLogin, BackendUserToken, BackendRoleClaim>>()
                .AddRoleStore<RoleStore<BackendRole, AdministratorDbContext, string, BackendUserRole, BackendRoleClaim>>()
                .AddEntityFrameworkStores<AdministratorDbContext>()
                .AddDefaultTokenProviders()
                ;

builder.Services.AddChannelLoacalEventBus();
builder.Services.AddCapDistributedEventBus(x =>
{
    x.UseEntityFramework<AdministratorDbContext>();

    x.UseSqlite(builder.Configuration.GetConnectionString("Default"));

    x.UseRabbitMQ(o => o.ConnectionFactoryOptions = (factory) => factory.Uri = new Uri(builder.Configuration["ConnectionStrings:RabbitMq"]));
    //x.UseRedis(builder.Configuration["ConnectionStrings:Redis"]);
});

builder.Services.AddAutoMapper();
builder.Services.AddIdGen(0);

builder.Services.AddSetting(typeof(AdministratorSettingSotre));


builder.Services.AddStackExchangeRedisCache(a => a.Configuration = builder.Configuration["ConnectionStrings:Redis"]);

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:Redis"]));
builder.Services.AddDataProtection()
    .SetApplicationName("Wheel-Administrator")
    .PersistKeysToStackExchangeRedis(() => ConnectionMultiplexer.Connect(builder.Configuration["ConnectionStrings:Redis"]).GetDatabase(), "Administrator-DataProtection-Keys");

// Localizer
builder.Services.AddLinguaNexLocalization(options =>
{
    options.LinguaNexApiUrl = builder.Configuration["LinguaNex:ApiUrl"];
    options.Project = builder.Configuration["LinguaNex:Project"];
    options.UseWebSocket = true;
});
builder.Services.AddLocalization();

#region Authentication
JwtSettingOptions jwtSettingOptions = new JwtSettingOptions();
builder.Configuration.GetSection("JwtSetting").Bind(jwtSettingOptions);
builder.Services.Configure<JwtSettingOptions>(builder.Configuration.GetSection("JwtSetting"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddCookie(IdentityConstants.ApplicationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettingOptions.Issuer, // 设置发行者
        ValidAudience = jwtSettingOptions.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettingOptions.SecurityKey)) // 设置密钥
    };
})
;

builder.Services.AddAuthorizationBuilder();
# endregion Authentication


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new Int32Converter());
    options.SerializerOptions.Converters.Add(new LongJsonConverter());
});

builder.Services.AddHealthChecks();
builder.Services.AddControllers()
    .AddControllersAsServices()
    .AddJsonOptions(configure =>
    {
        configure.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        configure.JsonSerializerOptions.Converters.Add(new Int32Converter());
        configure.JsonSerializerOptions.Converters.Add(new LongJsonConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//初始化种子信息
await app.SeedData();

app.UseRequestLocalization();

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
            string message = string.Empty;
            if (businessException.Message.IsNullOrWhiteSpace())
                message = businessException.Code;
            if (businessException.MessageData != null)
                await context.Response.WriteAsJsonAsync(new R { Code = businessException.Code, Message = L[message, businessException.MessageData] });
            else
                await context.Response.WriteAsJsonAsync(new R { Code = businessException.Code, Message = L[message] });
        }
        else
        {
            await context.Response.WriteAsJsonAsync(new R { Code = ErrorCode.InternalError, Message = exceptionHandlerPathFeature?.Error.Message });
        }
    });
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("Health");

app.Run();
