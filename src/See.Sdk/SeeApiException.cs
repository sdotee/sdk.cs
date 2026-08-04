using System.Net;

namespace See.Sdk;

/// <summary>Represents an unsuccessful S.EE API response.</summary>
public sealed class SeeApiException : HttpRequestException
{
    /// <summary>Creates an API exception.</summary>
    public SeeApiException(HttpStatusCode statusCode, string responseBody)
        : base($"S.EE API request failed with HTTP {(int)statusCode} ({statusCode}).", null, statusCode)
    {
        ResponseBody = responseBody;
    }

    /// <summary>Gets the raw response body.</summary>
    public string ResponseBody { get; }
}
