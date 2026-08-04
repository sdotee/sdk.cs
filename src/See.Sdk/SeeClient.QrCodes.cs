using See.Sdk.Models;

namespace See.Sdk;

public sealed partial class SeeClient
{
    public Task<ApiResponse<CreateQrCodeData>> CreateQrCodeAsync(CreateQrCodeRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<CreateQrCodeData>>(HttpMethod.Post, "/api/v1/qrcode", request, cancellationToken);

    public Task<ApiResponse> DeleteQrCodeAsync(DeleteQrCodeRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse>(HttpMethod.Delete, "/api/v1/qrcode", request, cancellationToken);

    public Task<ApiResponse<QrCodeHistoryData>> GetQrCodeHistoryAsync(int? page = null, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<QrCodeHistoryData>>(HttpMethod.Get, Query("/api/v1/qrcodes", ("page", page)), null, cancellationToken);
}
