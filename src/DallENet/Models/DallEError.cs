using System.Text.Json.Serialization;

namespace DallENet.Models;

/// <summary>
/// Contains information about the error occurred while invoking the service.
/// </summary>
public class DallEError
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameter that caused the error.
    /// </summary>
    [JsonPropertyName("param")]
    public string? Parameter { get; set; }

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string? Code { get; set; }
}