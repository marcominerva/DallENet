using Microsoft.Extensions.Configuration;

namespace DallENet.ServiceConfigurations;

internal class AzureDallEServiceConfiguration : DallEServiceConfiguration
{
    private const string ApiVersion = "2023-06-01-preview";

    /// <summary>
    /// Gets or sets the name of the Azure OpenAI Resource.
    /// </summary>
    public string? ResourceName { get; set; }

    public AzureAuthenticationType AuthenticationType { get; set; }

    public AzureDallEServiceConfiguration()
    {
    }

    public AzureDallEServiceConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        ResourceName = configuration.GetValue<string>("ResourceName");
        ArgumentNullException.ThrowIfNull(nameof(ResourceName));

        AuthenticationType = configuration.GetValue<string>("AuthenticationType")?.ToLowerInvariant() switch
        {
            "activedirectory" or "azureactivedirectory" or "azure" or "azuread" or "ad" => AzureAuthenticationType.ActiveDirectory,
            _ => AzureAuthenticationType.ApiKey  // API Key is the default.
        };
    }

    public override Uri GetImageGenerationEndpoint()
    {
        var endpoint = new Uri($"https://{ResourceName}.openai.azure.com/openai/images/generations:submit?api-version={ApiVersion}");
        return endpoint;
    }

    public override Uri GetDeleteImageEndpoint(string operationId)
    {
        var endpoint = new Uri($"https://{ResourceName}.openai.azure.com/openai/operations/images/{operationId}?api-version={ApiVersion}");
        return endpoint;
    }

    public override IDictionary<string, string?> GetRequestHeaders()
    {
        var headers = new Dictionary<string, string?>();

        if (AuthenticationType == AzureAuthenticationType.ApiKey)
        {
            headers["api-key"] = ApiKey;
        }
        else
        {
            headers["Authorization"] = $"Bearer {ApiKey}";
        }

        return headers;
    }
}
