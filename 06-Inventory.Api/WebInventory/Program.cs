using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using WebInventory;
using WebInventory.Services.Inventory;

var builder = WebApplication.CreateBuilder(args);

// Configuracio para recursos
builder.Services.AddLocalization(options => options.ResourcesPath = "Resource");
builder.Services.AddMvc().AddMvcLocalization();

//configuracion lectura de datos appsettings
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<SettingsValue>(configuration);
var serviceProvider = builder.Services.BuildServiceProvider();
var settings = serviceProvider.GetService<IOptions<SettingsValue>>();


// Add services to the container.
builder.Services.AddRazorPages();

// configurar la interfaz
builder.Services.AddHttpClient<IInventoryService, InventoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var cultureProviders = new[]
{
    new QueryStringRequestCultureProvider()
};

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("en"),
    new CultureInfo("es-ES"),
    new CultureInfo("es-MX"),
    new CultureInfo("es"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(settings.Value.DefaultCulture),
    RequestCultureProviders = cultureProviders,
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
