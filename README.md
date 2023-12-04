# DallENet

[![Lint Code Base](https://github.com/marcominerva/DallENet/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/DallENet/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/DallENet/actions/workflows/codeql.yml/badge.svg)](https://github.com/marcominerva/DallENet/actions/workflows/codeql.yml)
[![NuGet](https://img.shields.io/nuget/v/DallENet.svg?style=flat-square)](https://www.nuget.org/packages/DallENet)
[![Nuget](https://img.shields.io/nuget/dt/DallENet)](https://www.nuget.org/packages/DallENet)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/DallENet/blob/master/LICENSE)

A DALL·E integration library for .NET

> **Note**
The current version of the library requires a DALL·E 3 resource. If you want to use DALL·E 2, plese refer to [this version](https://github.com/marcominerva/DallENet/tree/v1.0.13).

## Installation

The library is available on [NuGet](https://www.nuget.org/packages/DallENet). Just search for *DallENet* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

    dotnet add package DallENet

## Configuration

Register DALL·E service at application startup:

    builder.Services.AddDallE(options =>
    {
        // Azure OpenAI Service.
        options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);

        options.DefaultSize = DallEImageSizes._1792x1024;              // Default: 1024x1024
        options.DefaultQuality = DallEImageQualities.HD;               // Default: Standard
        options.DefaultStyle = DallEImageStyles.Natural;               // Default: Vivid
        options.DefaultResponseFormat = DallEImageResponseFormats.Url; // Default: Url
    });


Currently, **DallENet** supports Azure OpenAI Service only. Support for OpenAI will be added in a future version. The required configuration parameters are the following:

- _ResourceName_: the name of your Azure OpenAI Resource (required).
- _ApiKey_: Azure OpenAI provides two methods for authentication. You can use either API Keys or Azure Active Directory (required).
- _ApiVersion_: the version of the API to use (optional). Allowed values:
  - 2023-12-01-preview (default)
- _AuthenticationType_: it specifies if the key is an actual API Key or an [Azure Active Directory token](https://learn.microsoft.com/azure/cognitive-services/openai/how-to/managed-identity) (optional, default: "ApiKey").

### Default Image Size

DALL·E 3 is able to generate images at different resolutions:

- 1024x1024
- 1792x1024
- 1024x1792

Using the *DefaultSize* property, it is possible to specify the default image size, unless you pass an explicit value in the **GenerateImageAsync** or **GetImageStreamAsync** methods. The default resolution is 1024x1024.

### Default Quality
DALL·E 3 is able to generate images in standard or HD quality, that is, with finer details and greater consistency across the image. Using the *DefaultQuality* property, it is possible to specify the default quality, unless you pass an explicit value in the **GenerateImageAsync** or **GetImageStreamAsync** methods. The default quality is _Standard_.

### Default Style
DALL·E 3 is able to generate images usinga vivid of natural style:

- Vivid generates hyper-real and dramatic images.
- Natural produces more natural, less hyper-real looking images.

Using the *DefaultStyle* property, it is possible to specify the default style, unless you pass an explicit value in the **GenerateImageAsync** or **GetImageStreamAsync** methods. The default style is _Vivid_.

### Default Response format
DALL·E 3 is able to return the URL of the generated image of its Base64 encoding. Using the *DefaultResponseFormat* property, it is possible to specify the default quality, unless you pass an explicit value in the **GenerateImageAsync** or **GetImageStreamAsync** methods. The default quality is _Url_.

### Configuration using an external source

The configuration can be automatically read from [IConfiguration](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration), using for example a _DallE_ section in the _appsettings.json_ file:

    "DallE": {
        "Provider": "Azure",                // Optional. Currently only Azure is supported
        "ApiKey": "",                       // Required
        "ResourceName": "",                 // Required 
        "ApiVersion": "2023-12-01-preview", // Optional, used only by Azure OpenAI Service. Allowed values: 2023-12-01-preview (default)
        "AuthenticationType": "ApiKey",     // Optional, Allowed values: ApiKey (default) or ActiveDirectory

        "DefaultModel": "dall-e-3",         // Required
        "DefaultSize": "1792x1024",         // Optional, Allowed values: 1024x1024 (default), 1792x1024 or 1024x1792
        "DefaultQuality": "standard",       // Optional, Allowed values: standard (default) or hd
        "DefaultResponseFormat": "url",     // Optional, Allowed values: url (default) or b64_json
        "DefaultStyle": "vivid",            // Optional, Allowed values: natural (default), or vivid
        "ThrowExceptionOnError": true
    }

And then use the corresponding overload of che **AddDallE** method:

    // Adds DALL·E service using settings from IConfiguration.
    builder.Services.AddDallE(builder.Configuration);

### Configuring DallENet dinamically

The **AddDallE** method has also an overload that accepts an [IServiceProvider](https://learn.microsoft.com/dotnet/api/system.iserviceprovider) as argument. It can be used, for example, if we're in a Web API and we need to support scenarios in which every user has a different API Key that can be retrieved accessing a database via Dependency Injection:

    builder.Services.AddDallE((services, options) =>
    {
        var accountService = services.GetRequiredService<IAccountService>();

        // Dynamically gets the Resource name and the API Key from the service.
        var resourceName = "...";
        var apiKey = "..."

        options.UseAzure(resourceName, apiKey);
    });

### Configuring DallENet using both IConfiguration and code

In more complex scenarios, it is possible to configure **DallENet** using both code and [IConfiguration](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration). This can be useful if we want to set a bunch of common properties, but at the same time we need some configuration logic. For example:

    builder.Services.AddDallE((services, options) =>
    {
        // Configure common properties (default size, default style, ecc.) using IConfiguration.
        options.UseConfiguration(builder.Configuration);

        var accountService = services.GetRequiredService<IAccountService>();

        // Dynamically gets the Resource name and the API Key from the service.
        var resourceName = "...";
        var apiKey = "..."

        options.UseAzure(resourceName, apiKey);
    });

## Usage

The library can be used in any .NET application built with .NET 6.0 or later. For example, we can create a Minimal API in this way:

    app.MapPost("/api/image", async (Request request, IDallEClient dallEClient) =>
    {
        var response = await dallEClient.GenerateImagesAsync(request.Prompt);
        return TypedResults.Ok(response);
    })
    .WithOpenApi();

    public record class Request(string Prompt);

In particular, the response contains the URL of the generated image. If we just want to retrieve the URL of the first generated image, we can call the **GetImageUrl** method:

    var imageUrl = response.GetImageUrl();

> **Note**
Generated images are automatically deleted after 24 hours.

Check the [Samples folder](https://github.com/marcominerva/DallENet/tree/master/samples) for more information about the different implementations.

## Documentation

The full technical documentation is available [here](https://github.com/marcominerva/DallENet/tree/master/docs).

## Contribute

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 

> **Warning**
Remember to work on the **develop** branch, don't use the **master** branch directly. Create Pull Requests targeting **develop**.
