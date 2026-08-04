using See.Sdk.Models;

namespace See.Sdk;

public sealed partial class SeeClient
{
    public Task<ApiResponse<AvailableDomains>> GetTextAvailableDomainsAsync(CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<AvailableDomains>>(HttpMethod.Get, "/api/v1/text/domains", null, cancellationToken);

    public Task<ApiResponse<CreateTextData>> CreateTextAsync(CreateTextRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<CreateTextData>>(HttpMethod.Post, "/api/v1/text", request, cancellationToken);

    public Task<ApiResponse> UpdateTextAsync(UpdateTextRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse>(HttpMethod.Put, "/api/v1/text", request, cancellationToken);

    public Task<ApiResponse> DeleteTextAsync(DeleteTextRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse>(HttpMethod.Delete, "/api/v1/text", request, cancellationToken);

    public Task<ApiResponse<IReadOnlyList<TextHistoryItem>>> GetTextHistoryAsync(int? page = null, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<IReadOnlyList<TextHistoryItem>>>(HttpMethod.Get, Query("/api/v1/texts", ("page", page)), null, cancellationToken);
}
