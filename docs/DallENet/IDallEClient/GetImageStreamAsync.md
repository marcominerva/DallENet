# IDallEClient.GetImageStreamAsync method

Requests a new image generation and directly returns the Stream containing the image.

```csharp
public Task<Stream> GetImageStreamAsync(string prompt, string? resolution = null, 
    CancellationToken cancellationToken = default)
```

| parameter | description |
| --- | --- |
| prompt | A text description of the desired image(s). The maximum length is 1000 characters. |
| resolution | The size of the generated images. If `null`, the resolution set in the [`DefaultResolution`](../DallEOptions/DefaultResolution.md) will be used (default: 1024x1024). |
| cancellationToken | The token to monitor for cancellation requests. |

## Return Value

The Stream containing the image

## Exceptions

| exception | condition |
| --- | --- |
| ArgumentNullException | *prompt* is `null`. |
| [DallEException](../../DallENet.Exceptions/DallEException.md) | An error occurred while calling the API. |

## Remarks

If an error occurred, this method throws a [`DallEException`](../../DallENet.Exceptions/DallEException.md) no matter the value of the [`ThrowExceptionOnError`](../DallEOptions/ThrowExceptionOnError.md) property.

## See Also

* class [DallEOptions](../DallEOptions.md)
* class [DallEException](../../DallENet.Exceptions/DallEException.md)
* interface [IDallEClient](../IDallEClient.md)
* namespace [DallENet](../../DallENet.md)

<!-- DO NOT EDIT: generated by xmldocmd for DallENet.dll -->
