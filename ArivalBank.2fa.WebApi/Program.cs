using ArivalBank._2fa.Application.Authorization;
using ArivalBank._2fa.Application.Interfaces;
using ArivalBank._2fa.Domain.Configuration;
using ArivalBank._2fa.Infrastructure.Data;
using ArivalBank._2fa.Infrastructure.Messaging;
using ArivalBank._2fa.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("AppConfiguration"));
var appConfiguration = builder.Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();


// Database Context
builder.Services
    .AddDbContext<AuthorizationDbContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("AuthCodesConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});


// Application
builder.Services.AddScoped(typeof(IRepository<>), typeof(AuthorizationRepository<>));
builder.Services.AddTransient<IAuthorizationCodeService, AuthorizationCodeService>();
builder.Services.AddSingleton<ISmsGatewayMessaging, TwilioGatewayMessaging>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
