namespace See.Sdk.Models;

public sealed class BioCustomLink
{
    public required string Title { get; init; }
    public required string Url { get; init; }
    public string? Description { get; init; }
}

public sealed class CreateBioPageRequest
{
    public required string Title { get; init; }
    public IReadOnlyList<BioCustomLink>? CustomLinks { get; init; }
    public string? CustomSlug { get; init; }
    public string? Description { get; init; }
    public string? Domain { get; init; }
    public string? MastodonUrl { get; init; }
    public string? RssUrl { get; init; }
}

public sealed class UpdateBioPageRequest
{
    public required long Id { get; init; }
    public required string Title { get; init; }
    public IReadOnlyList<BioCustomLink>? CustomLinks { get; init; }
    public string? Description { get; init; }
    public string? MastodonUrl { get; init; }
    public string? RssUrl { get; init; }
}

public sealed class DeleteBioPageRequest
{
    public required long Id { get; init; }
}

public sealed class CreateBioPageData
{
    public long BioPageId { get; init; }
    public string? ShortUrl { get; init; }
}

public sealed class BioPageHistoryItem
{
    public long CreatedAt { get; init; }
    public IReadOnlyList<BioCustomLink> CustomLinks { get; init; } = [];
    public string? Description { get; init; }
    public string? Domain { get; init; }
    public long Id { get; init; }
    public string? Link { get; init; }
    public string? MastodonUrl { get; init; }
    public string? RssUrl { get; init; }
    public string? Slug { get; init; }
    public string? Title { get; init; }
}

public sealed class BioPageHistoryData
{
    public IReadOnlyList<BioPageHistoryItem> BioPages { get; init; } = [];
    public long Total { get; init; }
}
