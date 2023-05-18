using _06_Inventory.Api;
using _06_Inventory.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container. FORMA 1
IConfiguration configuration = builder.Configuration;
//forma de leer direcatamente el valor de la propiedad de appsettings.
var connection = configuration.GetValue<string>("ConnectionString");

// forma de leer las propiedades del appsettings con clase
builder.Services.Configure<SettingsValue>(configuration);
var serviceProvider = builder.Services.BuildServiceProvider();
var settings = serviceProvider.GetService<IOptions<SettingsValue>>();

builder.Services.AddDbContext<NAFContext>(options =>
{
    options.UseOracle(settings.Value.ConnectionString, options => options.UseOracleSQLCompatibility("11"));
});



//// Add services to the container. FORMA 2
//var serviceProvider = builder.Services.BuildServiceProvider(); //para la segunda forma de conexion
//var settings = serviceProvider.GetService<IOptions<SettingsValue>>();

//builder.Services.AddDbContext<NAFContext>(options =>
//{
//    options.UseOracle(settings.Value.ConnectionString, options => options.UseOracleSQLCompatibility("11"));
//});


//// primera forma para leer solo el string de conexión (FORMA 3)
//IConfiguration configuration = builder.Configuration;
//var connection = configuration.GetValue<string>("ConnectionString");

//builder.Services.AddDbContext<NAFContext>(options =>
//{
//    options.UseOracle(connection, options => options.UseOracleSQLCompatibility("11"));
//});



//// Add services to the container. (FORNA 4)
//var cadenaConexion = builder.Configuration.GetConnectionString("ConnectionString");

//builder.Services.AddDbContext<NAFContext>(x =>
//    x.UseOracle(
//        cadenaConexion,
//        options => options.UseOracleSQLCompatibility("11")
//)
//);

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

app.Run();
