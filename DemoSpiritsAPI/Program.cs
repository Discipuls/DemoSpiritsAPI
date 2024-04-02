using DemoSpiritsAPI.AutoMappers;
using DemoSpiritsAPI.EntiryFramework.Contexts;
using DemoSpiritsAPI.Servicies;
using DemoSpiritsAPI.Servicies.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MySQLContext>();
builder.Services.AddTransient<IHabitatService, HabitatService>();
builder.Services.AddTransient<ISpiritService, SpiritService>();
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

/*var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var url = $"http://0.0.0.0:{port}";*/

app.Run(/*url*/);
