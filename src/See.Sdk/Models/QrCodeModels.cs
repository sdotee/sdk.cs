namespace See.Sdk.Models;

public sealed class CreateQrCodeRequest
{
    public required string TargetUrl { get; init; }
    public required string Title { get; init; }
    public string? CustomSlug { get; init; }
    public string? Domain { get; init; }
}

public sealed class DeleteQrCodeRequest
{
    public required string Domain { get; init; }
    public required string Slug { get; init; }
}

public sealed class CreateQrCodeData
{
    public string? CustomSlug { get; init; }
    public string? PdfUrl { get; init; }
    public string? PngUrl { get; init; }
    public string? ShortUrl { get; init; }
    public string? Slug { get; init; }
    public string? SvgUrl { get; init; }
}

public sealed class QrCodeHistoryItem
{
    public long CreatedAt { get; init; }
    public string? Domain { get; init; }
    public string? PdfUrl { get; init; }
    public string? PngUrl { get; init; }
    public long ScanCount { get; init; }
    public string? ShortUrl { get; init; }
    public string? Slug { get; init; }
    public string? SvgUrl { get; init; }
    public string? Title { get; init; }
}

public sealed class QrCodeHistoryData
{
    public IReadOnlyList<QrCodeHistoryItem> Qrcodes { get; init; } = [];
    public long Total { get; init; }
}
