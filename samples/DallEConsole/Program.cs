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

    // Adds DALLE·E service using settings from IConfiguration.
    services.AddDallE(context.Configuration);

    // Adds DALLE·E service and configure options via code.
    //services.AddDallE(options =>
    //{
    //    // Azure OpenAI Service.
    //    //options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);

    //    options.DefaultResolution = DallEImageResolutions.Medium;     // Default: Large (1024x1024)
    //    options.DefaultImageCount = 2;  // Default: 1
    //});

    // Adds DALLE·E service using settings from IConfiguration and code.
    //services.AddDallE(options =>
    //{
    //    options.UseConfiguration(context.Configuration);

    //    options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);
    //    options.DefaultResolution = DallEImageResolutions.Medium;     // Default: Large (1024x1024)
    //    options.DefaultImageCount = 2;  // Default: 1
    //});
}