namespace DallENet.Models;

/// <summary>
/// Contains all the image qualities supported by DALL·E.
/// </summary>
public static class DallEImageQualities
{
    /// <summary>
    /// The standard image quality.
    /// </summary>    
    public const string Standard = "standard";

    /// <summary>
    /// Creates images with finer details and greater consistency across the image.
    /// </summary>
    public const string HD = "hd";
}