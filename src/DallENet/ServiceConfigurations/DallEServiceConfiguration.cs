﻿using Microsoft.Extensions.Configuration;

namespace DallENet.ServiceConfigurations;

internal abstract class DallEServiceConfiguration
{
    public string? ApiKey { get; set; }

    public abstract Uri GetImageGenerationEndpoint(string? model);

    public abstract IDictionary<string, string?> GetRequestHeaders();

    internal static DallEServiceConfiguration Create(IConfiguration configuration)
    {
        DallEServiceConfiguration serviceConfiguration = configuration.GetValue<string>("Provider")?.ToLowerInvariant() switch
        {
            _ => new AzureDallEServiceConfiguration(configuration)
        };

        serviceConfiguration.ApiKey = configuration.GetValue<string>("ApiKey");

        return serviceConfiguration;
    }
}