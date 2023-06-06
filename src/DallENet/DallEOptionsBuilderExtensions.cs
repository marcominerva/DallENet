using DallENet.ServiceConfigurations;
using Microsoft.Extensions.Configuration;

namespace DallENet;

/// <summary>
/// Provides extensions to configure settings for accessing DALL·E service.
/// </summary>
public static class DallEOptionsBuilderExtensions
{
    /// <summary>
    /// Configures Azure OpenAI Service settings.
    /// </summary>
    /// <param name="builder">The <see cref="DallEOptionsBuilder"/> object to configure.</param>
    /// <param name="resourceName">The name of the Azure OpenAI Resource.</param>
    /// <param name="apiKey">The access key to access the service.</param>
    /// <param name="authenticationType">Specify if <paramref name="apiKey"/> is an actual API Key or an Azure Active Directory token.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resourceName"/> or <paramref name="apiKey"/> are <see langword="null"/>.</exception>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/azure/cognitive-services/openai/reference#authentication">Azure OpenAI Service Authentication</see> and <see href="https://learn.microsoft.com/azure/cognitive-services/openai/how-to/managed-identity">Authenticating with Azure Active Directory</see> for more information about authentication.
    /// </remarks>
    /// <seealso cref="DallEOptionsBuilder"/>
    public static DallEOptionsBuilder UseAzure(this DallEOptionsBuilder builder, string resourceName, string apiKey, AzureAuthenticationType authenticationType = AzureAuthenticationType.ApiKey)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(resourceName);
        ArgumentNullException.ThrowIfNull(apiKey);

        builder.ServiceConfiguration = new AzureDallEServiceConfiguration
        {
            ResourceName = resourceName,
            ApiKey = apiKey,
            AuthenticationType = authenticationType
        };

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="DallEClient"/> reading configuration from the specified <see cref="IConfiguration"/> source.
    /// </summary>
    /// <param name="builder">The <see cref="DallEOptionsBuilder"/> object to configure.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> being bound.</param>
    /// <param name="sectionName">The name of the configuration section that holds DALL·E settings (default: DallE).</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <seealso cref="DallEOptionsBuilder"/>
    /// <seealso cref="IConfiguration"/>
    public static DallEOptionsBuilder UseConfiguration(this DallEOptionsBuilder builder, IConfiguration configuration, string sectionName = "DallE")
    {
        var configurationSection = configuration.GetSection(sectionName);
        configurationSection.Bind(builder);

        // Creates the service configuration (OpenAI or Azure) according to the configuration settings.
        builder.ServiceConfiguration = DallEServiceConfiguration.Create(configurationSection);

        return builder;
    }
}
