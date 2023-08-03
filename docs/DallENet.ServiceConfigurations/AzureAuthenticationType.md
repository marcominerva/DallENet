# AzureAuthenticationType enumeration

Enumerates the available Azure authentication types for OpenAI service.

```csharp
public enum AzureAuthenticationType
```

## Values

| name | value | description |
| --- | --- | --- |
| ApiKey | `0` | Authenticates using an API key. |
| ActiveDirectory | `1` | Authenticates using Azure Active Directory. |

## Remarks

See [Azure OpenAI Service Authentication](https://learn.microsoft.com/azure/cognitive-services/openai/reference#authentication) and [Authenticating with Azure Active Directory](https://learn.microsoft.com/azure/cognitive-services/openai/how-to/managed-identity) for more information about authentication.

## See Also

* namespace [DallENet.ServiceConfigurations](../DallENet.md)
* [AzureAuthenticationType.cs](https://github.com/marcominerva/DallENet/tree/master/src/DallENet/ServiceConfigurations/AzureAuthenticationType.cs)

<!-- DO NOT EDIT: generated by xmldocmd for DallENet.dll -->