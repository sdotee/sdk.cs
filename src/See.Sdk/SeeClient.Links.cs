using See.Sdk.Models;

namespace See.Sdk;

public sealed partial class SeeClient
{
    public Task<ApiResponse<AvailableDomains>> GetAvailableDomainsAsync(CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<AvailableDomains>>(HttpMethod.Get, "/api/v1/domains", null, cancellationToken);

    public Task<ApiResponse<CreateShortUrlData>> CreateShortUrlAsync(CreateShortUrlRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<CreateShortUrlData>>(HttpMethod.Post, "/api/v1/shorten", request, cancellationToken);

    public Task<ApiResponse> UpdateShortUrlAsync(UpdateShortUrlRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse>(HttpMethod.Put, "/api/v1/shorten", request, cancellationToken);

    public Task<ApiResponse> DeleteShortUrlAsync(DeleteShortUrlRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse>(HttpMethod.Delete, "/api/v1/shorten", request, cancellationToken);

    public async Task<string> CreateShortUrlSimpleAsync(SimpleShortUrlRequest request, bool json = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var path = Query(
            "/api/v1/shorten",
            ("signature", request.Signature),
            ("url", request.Url),
            ("domain", request.Domain),
            ("custom_slug", request.CustomSlug),
            ("title", request.Title),
            ("tag_ids", request.TagIds is null ? null : string.Join(',', request.TagIds)),
            ("password", request.Password),
            ("expire_at", request.ExpireAt),
            ("json", json.ToString().ToLowerInvariant()));
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, path);
        using var response = await SendAsync(httpRequest, cancellationToken, authenticate: false).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task<ApiResponse<IReadOnlyList<LinkHistoryItem>>> GetLinkHistoryAsync(int? page = null, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<IReadOnlyList<LinkHistoryItem>>>(HttpMethod.Get, Query("/api/v1/links", ("page", page)), null, cancellationToken);

    public Task<ApiResponse<LinkVisitStatistics>> GetLinkVisitStatisticsAsync(
        string domain,
        string slug,
        StatisticsPeriod period = StatisticsPeriod.Totally,
        CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<LinkVisitStatistics>>(
            HttpMethod.Get,
            Query("/api/v1/link/visit-stat", ("domain", domain), ("slug", slug), ("period", period.ToString().ToLowerInvariant())),
            null,
            cancellationToken);

    public Task<ApiResponse<TagsData>> GetTagsAsync(CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<TagsData>>(HttpMethod.Get, "/api/v1/tags", null, cancellationToken);

    public Task<ApiResponse<UsageData>> GetUsageAsync(CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<UsageData>>(HttpMethod.Get, "/api/v1/usage", null, cancellationToken);

    public Task<ApiResponse<TokenCheckData>> CheckTokenAsync(TokenCheckRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<TokenCheckData>>(HttpMethod.Post, "/api/v1/token/check", request, cancellationToken, authenticate: false);
}
