using _06_Inventory.Api;
using _06_Inventory.Api.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Configuration;
using _06_Inventory.Api.Mapper;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. (FORMA 1)
//{
//    IConfiguration configuration = builder.Configuration;

//    // forma de leer las propiedades del appsettings con clase
//    builder.Services.Configure<SettingsValue>(configuration);
//    var serviceProvider = builder.Services.BuildServiceProvider();
//    var settings = serviceProvider.GetService<IOptions<SettingsValue>>();

//    builder.Services.AddDbContext<NAFContext>(options => 
//    { 
//        options.UseOracle(settings.Value.ConnectionString, options => options.UseOracleSQLCompatibility("11")); 
//    });
//}


//Add services to the container. (FORNA 2)
{
    var cadenaConexion = builder.Configuration.GetConnectionString("ConnectionString");

    builder.Services.AddDbContext<NAFContext>(x => x.UseOracle(cadenaConexion, options => options.UseOracleSQLCompatibility("11")));
}


//// forma para leer solo el string de conexión (FORMA 3)
//{
//    IConfiguration configuration = builder.Configuration;
//    //forma de leer direcatamente el valor de la propiedad de appsettings.
//    var connection = configuration.GetValue<string>("ConnectionString");

//    builder.Services.AddDbContext<NAFContext>(options =>
//    {
//        options.UseOracle(connection, options => options.UseOracleSQLCompatibility("11"));
//    });
//}


// Path donde se ubican los recursos
// 1. 
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddLocalization(options=> options.ResourcesPath= "Resources");

// 2. 
builder.Services.AddControllersWithViews()
    .AddViewLocalization
    (LanguageViewLocationExpanderFormat.SubFolder)
    .AddDataAnnotationsLocalization();

// 3. 
builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[] { "es-MX","en-US", "fr", "de" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

});

//builder.Services.AddScoped<ProductService>();

//automapper
var mappingConfig = new MapperConfiguration(c =>
{
    c.AddProfile(new AutoMapping());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Globalization and Localization
var cultureProviders = new[]
{
                new QueryStringRequestCultureProvider()  //Deja solo este proveedor para poder pasar la cultura en el query string para efectos de prueba, pero que no se usen ni CookieRequestCultureProvider ni AcceptLanguageHeaderRequestCultureProvider (definida por el navegador). LCS.
            };

var supportedCultures = new[]
{
                new CultureInfo("en-US"),
                new CultureInfo("en"),
                new CultureInfo("es-ES"),
                new CultureInfo("es-MX"),
                new CultureInfo("es"),
            };

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
