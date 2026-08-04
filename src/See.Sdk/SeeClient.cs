using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace See.Sdk;

/// <summary>Client for the S.EE API.</summary>
public sealed partial class SeeClient
{
    /// <summary>Gets the default S.EE API base URI.</summary>
    public static Uri BaseUri { get; } = new("https://s.ee/api/v1/");

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    /// <summary>Creates an S.EE API client.</summary>
    public SeeClient(HttpClient httpClient, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    private async Task<T> SendAsync<T>(
        HttpMethod method,
        string path,
        object? body,
        CancellationToken cancellationToken,
        bool authenticate = true)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: JsonOptions);
        }

        using var response = await SendAsync(request, cancellationToken, authenticate).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(content, JsonOptions)
            ?? throw new SeeApiException(response.StatusCode, "The API returned an empty or invalid JSON response.");
    }

    private async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken,
        bool authenticate = true)
    {
        if (request.RequestUri is { IsAbsoluteUri: false } && _httpClient.BaseAddress is null)
        {
            request.RequestUri = new Uri(BaseUri, request.RequestUri);
        }

        if (authenticate)
        {
            request.Headers.TryAddWithoutValidation("Authorization", _apiKey);
        }

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        using (response)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new SeeApiException(response.StatusCode, content);
        }
    }

    private static string Query(string path, params (string Name, object? Value)[] parameters)
    {
        var values = parameters
            .Where(parameter => parameter.Value is not null)
            .Select(parameter => $"{Uri.EscapeDataString(parameter.Name)}={Uri.EscapeDataString(Convert.ToString(parameter.Value, System.Globalization.CultureInfo.InvariantCulture)!)}");
        var query = string.Join("&", values);
        return query.Length == 0 ? path : $"{path}?{query}";
    }
}
