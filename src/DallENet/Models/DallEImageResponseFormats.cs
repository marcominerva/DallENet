namespace DallENet.Models;

/// <summary>
/// Contains all the image response formats supported by DALL·E.
/// </summary>
public static class DallEImageResponseFormats
{
    /// <summary>
    /// The response will be a URL pointing to the image.
    /// </summary>    
    public const string Url = "url";

    /// <summary>
    /// The response will be the base 64 byte code in JSON format.
    /// </summary>
    public const string Base64Json = "b64_json";
}