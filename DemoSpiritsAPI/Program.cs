using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Servicies;
using DemoSpiritsAPI.Servicies.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;


builder.Services.AddDbContext<SQLServerContext>();
builder.Services.AddTransient<IHabitatService, HabitatService>();
builder.Services.AddTransient<ISpiritService, SpiritService>();
builder.Services.AddTransient<IGoogleAuthService, GoogleAuthService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

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