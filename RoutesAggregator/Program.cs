using RoutesAggregator;
using RoutesAggregator.Providers;
using RoutesAggregator.Providers.ProviderOne;
using RoutesAggregator.Providers.ProviderTwo;
using RoutesAggregator.Providers.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ProvidersSettings>(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddTransient<ISearchResponseFactory, SearchResponseFactory>();
builder.Services.AddTransient<IApiClientFactory, OneApiClientFactory>();
builder.Services.AddTransient<IApiClientFactory, TwoApiClientFactory>();
builder.Services.AddSingleton<IClientsProvider, ClientsProvider>();
builder.Services.AddSingleton<ICacheWorker, CacheWorker>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

builder.Configuration.AddJsonFile("Providers/Settings/ProvidersSettings.json");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();