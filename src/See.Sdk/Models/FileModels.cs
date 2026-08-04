namespace See.Sdk.Models;

public sealed class UploadedFile
{
    public long CreatedAt { get; init; }
    public string? Delete { get; init; }
    public long FileId { get; init; }
    public string? Filename { get; init; }
    public string? Hash { get; init; }
    public long Height { get; init; }
    public string? MimeType { get; init; }
    public string? Page { get; init; }
    public string? Path { get; init; }
    public long Size { get; init; }
    public string? Storename { get; init; }
    public string? ThumbUrl { get; init; }
    public int UploadStatus { get; init; }
    public string? Url { get; init; }
    public long Width { get; init; }
}

public sealed class DeleteFileResponse
{
    public string? Code { get; init; }
    public string? Message { get; init; }
    public bool Success { get; init; }
}

public sealed class CreateLargeFileUploadRequest
{
    public required string FileName { get; init; }
    public required long FileSize { get; init; }
    public string? Alias { get; init; }
    public string? Description { get; init; }
    public string? Domain { get; init; }
    public long? ExpireAt { get; init; }
    public string? FileHash { get; init; }
    public int? IsPrivate { get; init; }
    public string? MimeType { get; init; }
    public string? Password { get; init; }
    public string? Title { get; init; }
}

public sealed class CreateLargeFileUploadData
{
    public UploadedFile? ExistingFile { get; init; }
    public long ExpiresAt { get; init; }
    public bool FastUpload { get; init; }
    public long FileSize { get; init; }
    public long Id { get; init; }
    public string? UploadId { get; init; }
    public string? UploadUrl { get; init; }
}

public sealed class UploadSessionRequest
{
    public required string UploadId { get; init; }
}

public sealed class CompleteLargeFileUploadData
{
    public UploadedFile? File { get; init; }
    public string? ShortLink { get; init; }
}

public sealed class LargeFileUploadProgress
{
    public long CreatedAt { get; init; }
    public string? FileName { get; init; }
    public long FileSize { get; init; }
    public double Progress { get; init; }
    public int Status { get; init; }
    public long UpdatedAt { get; init; }
    public string? UploadId { get; init; }
    public long UploadedSize { get; init; }
}

public sealed class PrivateFileDownload
{
    public long ExpiresAt { get; init; }
    public long FileId { get; init; }
    public string? Url { get; init; }
}

public sealed record TusUploadStatus(long Offset, long? Length);
