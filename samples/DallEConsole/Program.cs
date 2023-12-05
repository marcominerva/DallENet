using DallEConsole;
using DallENet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices)
    .Build();

var application = host.Services.GetRequiredService<Application>();
await application.ExecuteAsync();

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddSingleton<Application>();

    // Adds DALL·E service using settings from IConfiguration.
    services.AddDallE(context.Configuration);

    //// Adds DALL·E service and configure options via code.
    //services.AddDallE(options =>
    //{
    //    // Azure OpenAI Service.
    //    options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);

    //    options.DefaultSize = DallEImageSizes._1792x1024;              // Default: 1024x1024
    //    options.DefaultQuality = DallEImageQualities.HD;               // Default: Standard
    //    options.DefaultStyle = DallEImageStyles.Natural;               // Default: Vivid
    //    options.DefaultResponseFormat = DallEImageResponseFormats.Url; // Default: Url
    //});

    //// Adds DALL·E service using settings from IConfiguration and code.
    //services.AddDallE(options =>
    //{
    //    options.UseConfiguration(context.Configuration);

    //    options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);
    //    options.DefaultSize = DallEImageSizes._1792x1024;     // Default: 1024x1024
    //});
}