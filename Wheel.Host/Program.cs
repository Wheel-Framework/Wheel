using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wheel.DependencyInjection;
using Wheel.Domain.Identity;
using Wheel.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WheelHostContextConnection") ?? throw new InvalidOperationException("Connection string 'WheelHostContextConnection' not found.");

builder.Services.InitWheelDependency();

builder.Services.AddDbContext<WheelDbContext>(options => 
options.UseSqlite(connectionString)
    .UseLazyLoadingProxies()
);

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<WheelDbContext>()
    .AddDefaultTokenProviders()
    ;

builder.Services.AddIdGen(0);

// Add services to the container.

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
