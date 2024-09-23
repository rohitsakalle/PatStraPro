using Microsoft.Azure.Cosmos;
using PatStraPro.Dashboard.Service;
using PatStraPro.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IDashboardService, DashboardService>();
// Register CosmosDbService
builder.Services.AddSingleton<CosmosDbService>(sp =>
{
    //Pass AccountKey and AccountEndpoint from System Environment or KeyVault
    var cosmosClient = new CosmosClient("AccountEndpoint=YourAccountEndPoint;AccountKey=YourAccountKey;");
    return new CosmosDbService();
});
//builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
