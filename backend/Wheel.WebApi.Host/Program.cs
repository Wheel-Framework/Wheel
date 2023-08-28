using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wheel.DependencyInjection;
using Wheel.Domain.Identity;
using Wheel.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");

// Add services to the container.

builder.Services.InitWheelDependency();
builder.Services.AddIdGen(0);

builder.Services.AddDbContext<WheelDbContext>(options =>
options.UseSqlite(connectionString)
    .UseLazyLoadingProxies()
);

builder.Services.AddAuthentication(IdentityConstants.BearerScheme)
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<WheelDbContext>()
                .AddApiEndpoints();

builder.Services.AddSignalR();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization(); 

var webSocketOptions = new WebSocketOptions
{
};
app.UseWebSockets(webSocketOptions);

app.MapIdentityApi<User>();
app.MapControllers();

app.Run();
