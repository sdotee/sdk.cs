namespace See.Sdk.Models;

public sealed class CreateShortUrlRequest
{
    public required string Domain { get; init; }
    public required string TargetUrl { get; init; }
    public string? CustomSlug { get; init; }
    public string? ExpirationRedirectUrl { get; init; }
    public long? ExpireAt { get; init; }
    public string? Password { get; init; }
    public IReadOnlyList<long>? TagIds { get; init; }
    public string? Title { get; init; }
}

public sealed class UpdateShortUrlRequest
{
    public required string Domain { get; init; }
    public required string Slug { get; init; }
    public required string TargetUrl { get; init; }
    public required string Title { get; init; }
}

public sealed class DeleteShortUrlRequest
{
    public required string Domain { get; init; }
    public required string Slug { get; init; }
}

public sealed class CreateShortUrlData
{
    public string? CustomSlug { get; init; }
    public string? ShortUrl { get; init; }
    public string? Slug { get; init; }
}

public sealed class LinkHistoryItem
{
    public long CreatedAt { get; init; }
    public string? Domain { get; init; }
    public int ObjectType { get; init; }
    public string? ShortUrl { get; init; }
    public string? Slug { get; init; }
    public string? TargetUrl { get; init; }
    public string? Title { get; init; }
    public long VisitCount { get; init; }
}

public sealed class LinkVisitStatistics
{
    public long VisitCount { get; init; }
}

public enum StatisticsPeriod
{
    Totally,
    Daily,
    Monthly
}

public sealed class SimpleShortUrlRequest
{
    public required string Signature { get; init; }
    public required string Url { get; init; }
    public string? Domain { get; init; }
    public string? CustomSlug { get; init; }
    public string? Title { get; init; }
    public IReadOnlyList<long>? TagIds { get; init; }
    public string? Password { get; init; }
    public long? ExpireAt { get; init; }
}
