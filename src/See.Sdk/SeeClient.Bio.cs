using See.Sdk.Models;

namespace See.Sdk;

public sealed partial class SeeClient
{
    public Task<ApiResponse<CreateBioPageData>> CreateBioPageAsync(CreateBioPageRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<CreateBioPageData>>(HttpMethod.Post, "/api/v1/bio", request, cancellationToken);

    public Task<ApiResponse> UpdateBioPageAsync(UpdateBioPageRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse>(HttpMethod.Put, "/api/v1/bio", request, cancellationToken);

    public Task<ApiResponse> DeleteBioPageAsync(DeleteBioPageRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse>(HttpMethod.Delete, "/api/v1/bio", request, cancellationToken);

    public Task<ApiResponse<BioPageHistoryData>> GetBioPageHistoryAsync(int? page = null, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<BioPageHistoryData>>(HttpMethod.Get, Query("/api/v1/bios", ("page", page)), null, cancellationToken);
}
