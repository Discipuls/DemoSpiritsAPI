using DemoSpiritsAPI.AutoMappers;
using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Servicies;
using DemoSpiritsAPI.Servicies.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;


builder.Services.AddDbContext<MySQLContext>();
builder.Services.AddTransient<IHabitatService, HabitatService>();
builder.Services.AddTransient<ISpiritService, SpiritService>();
builder.Services.AddTransient<IGoogleAuthService, GoogleAuthService>();
//builder.Services.AddAutoMapper(typeof(SpiritAutoMapper));
//builder.Services.AddAutoMapper(typeof(HabitatAutoMapper));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
Environment.SetEnvironmentVariable("DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP3SUPPORT","false");
var url = $"http://0.0.0.0:{port}";

app.Run(url);



/*app.Run();*/  