# DallEOptionsBuilder class

Builder class to define settings for configuring DALL·E.

```csharp
public class DallEOptionsBuilder
```

## Public Members

| name | description |
| --- | --- |
| [DallEOptionsBuilder](DallEOptionsBuilder/DallEOptionsBuilder.md)() | The default constructor. |
| [DefaultImageCount](DallEOptionsBuilder/DefaultImageCount.md) { get; set; } | Gets or sets the default number of images to generate for a request. Must be between 1 and 5 (default: 1). |
| [DefaultResolution](DallEOptionsBuilder/DefaultResolution.md) { get; set; } | Gets or sets the default resolution for image generation (default: 1024x1024). |
| [ThrowExceptionOnError](DallEOptionsBuilder/ThrowExceptionOnError.md) { get; set; } | Gets or sets a value that determines whether to throw a [`DallEException`](../DallENet.Exceptions/DallEException.md) when an error occurred (default: `true`). If this property is set to `false`, API errors are returned in the [`DallEImageGenerationResponse`](../DallENet.Models/DallEImageGenerationResponse.md) object. |

## See Also

* namespace [DallENet](../DallENet.md)
* [DallEOptionsBuilder.cs](https://github.com/marcominerva/DallENet/tree/master/src/DallENet/DallEOptionsBuilder.cs)

<!-- DO NOT EDIT: generated by xmldocmd for DallENet.dll -->
