using System.Net.Http.Headers;
using See.Sdk.Models;

namespace See.Sdk;

public sealed partial class SeeClient
{
    public Task<ApiResponse<AvailableDomains>> GetFileAvailableDomainsAsync(CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<AvailableDomains>>(HttpMethod.Get, "/api/v1/file/domains", null, cancellationToken);

    public async Task<ApiResponse<UploadedFile>> UploadFileAsync(
        Stream file,
        string fileName,
        string? domain = null,
        string? customSlug = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        using var content = new MultipartFormDataContent();
        content.Add(new NonDisposingStreamContent(file), "file", fileName);
        if (!string.IsNullOrWhiteSpace(domain))
        {
            content.Add(new StringContent(domain), "domain");
        }
        if (!string.IsNullOrWhiteSpace(customSlug))
        {
            content.Add(new StringContent(customSlug), "custom_slug");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/file/upload") { Content = content };
        using var response = await SendAsync(request, cancellationToken).ConfigureAwait(false);
        var payload = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return System.Text.Json.JsonSerializer.Deserialize<ApiResponse<UploadedFile>>(payload, JsonOptions)
            ?? throw new SeeApiException(response.StatusCode, "The API returned an empty or invalid JSON response.");
    }

    public Task<ApiResponse<IReadOnlyList<UploadedFile>>> GetFileHistoryAsync(int? page = null, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<IReadOnlyList<UploadedFile>>>(HttpMethod.Get, Query("/api/v1/files", ("page", page)), null, cancellationToken);

    public Task<DeleteFileResponse> DeleteFileAsync(string deleteKey, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deleteKey);
        return SendAsync<DeleteFileResponse>(HttpMethod.Get, $"/api/v1/file/delete/{Uri.EscapeDataString(deleteKey)}", null, cancellationToken);
    }

    public Task<ApiResponse<PrivateFileDownload>> GetPrivateFileDownloadUrlAsync(long fileId, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fileId);
        return SendAsync<ApiResponse<PrivateFileDownload>>(HttpMethod.Get, Query("/api/v1/file/private/download-url", ("file_id", fileId)), null, cancellationToken);
    }

    public Task<ApiResponse<CreateLargeFileUploadData>> CreateLargeFileUploadAsync(CreateLargeFileUploadRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<ApiResponse<CreateLargeFileUploadData>>(HttpMethod.Post, "/api/v1/file/large-file/create", request, cancellationToken);

    public Task<ApiResponse<CompleteLargeFileUploadData>> CompleteLargeFileUploadAsync(string uploadId, CancellationToken cancellationToken = default)
    {
        ValidateUploadId(uploadId);
        return SendAsync<ApiResponse<CompleteLargeFileUploadData>>(HttpMethod.Post, "/api/v1/file/large-file/complete", new UploadSessionRequest { UploadId = uploadId }, cancellationToken);
    }

    public Task<ApiResponse> CancelLargeFileUploadAsync(string uploadId, CancellationToken cancellationToken = default)
    {
        ValidateUploadId(uploadId);
        return SendAsync<ApiResponse>(HttpMethod.Delete, "/api/v1/file/large-file/cancel", new UploadSessionRequest { UploadId = uploadId }, cancellationToken);
    }

    public Task<ApiResponse<LargeFileUploadProgress>> GetLargeFileUploadProgressAsync(string uploadId, CancellationToken cancellationToken = default)
    {
        ValidateUploadId(uploadId);
        return SendAsync<ApiResponse<LargeFileUploadProgress>>(HttpMethod.Get, Query("/api/v1/file/large-file/progress", ("upload_id", uploadId)), null, cancellationToken);
    }

    public async Task UploadTusChunkAsync(string uploadId, Stream chunk, long offset, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chunk);
        ValidateUploadId(uploadId);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        using var content = new NonDisposingStreamContent(chunk);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/offset+octet-stream");
        using var request = CreateTusRequest(HttpMethod.Patch, uploadId);
        request.Headers.TryAddWithoutValidation("Upload-Offset", offset.ToString(System.Globalization.CultureInfo.InvariantCulture));
        request.Content = content;
        using var response = await SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TusUploadStatus> GetTusUploadStatusAsync(string uploadId, CancellationToken cancellationToken = default)
    {
        ValidateUploadId(uploadId);
        using var request = CreateTusRequest(HttpMethod.Head, uploadId);
        using var response = await SendAsync(request, cancellationToken).ConfigureAwait(false);
        var offset = ReadInt64Header(response, "Upload-Offset")
            ?? throw new SeeApiException(response.StatusCode, "The TUS response is missing a valid Upload-Offset header.");
        return new TusUploadStatus(offset, ReadInt64Header(response, "Upload-Length"));
    }

    public async Task DeleteTusUploadAsync(string uploadId, CancellationToken cancellationToken = default)
    {
        ValidateUploadId(uploadId);
        using var request = CreateTusRequest(HttpMethod.Delete, uploadId);
        using var response = await SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static HttpRequestMessage CreateTusRequest(HttpMethod method, string uploadId)
    {
        var request = new HttpRequestMessage(method, $"/api/v1/file/large-file-tus/{Uri.EscapeDataString(uploadId)}");
        request.Headers.TryAddWithoutValidation("Tus-Resumable", "1.0.0");
        return request;
    }

    private static long? ReadInt64Header(HttpResponseMessage response, string name) =>
        response.Headers.TryGetValues(name, out var values) && long.TryParse(values.FirstOrDefault(), out var value) ? value : null;

    private static void ValidateUploadId(string uploadId) =>
        ArgumentException.ThrowIfNullOrWhiteSpace(uploadId);
}
