using System.Net;
using DallENet.Models;

namespace DallENet.Exceptions;

/// <summary>
/// Represents errors that occur during API invocation.
/// </summary>
public class DallEException : HttpRequestException
{
    /// <summary>
    /// Gets the detailed error information.
    /// </summary>
    /// <seealso cref="DallEError"/>
    public DallEError Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DallEException"/> class with the specified <paramref name="error"/> details.
    /// </summary>
    /// <param name="error">The detailed error information</param>
    /// <param name="statusCode">The HTTP status code</param>
    /// <seealso cref="DallEError"/>
    /// <seealso cref="HttpRequestException"/>
    public DallEException(DallEError? error, HttpStatusCode statusCode) : base(!string.IsNullOrWhiteSpace(error?.Message) ? error.Message : error?.Code ?? statusCode.ToString(), null, statusCode)
    {
        Error = error ?? new DallEError
        {
            Message = statusCode.ToString(),
            Code = ((int)statusCode).ToString()
        };
    }
}
