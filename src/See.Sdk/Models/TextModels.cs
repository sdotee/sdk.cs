namespace See.Sdk.Models;

public sealed class CreateTextRequest
{
    public required string Content { get; init; }
    public required string Title { get; init; }
    public string? CustomSlug { get; init; }
    public string? Domain { get; init; }
    public long? ExpireAt { get; init; }
    public string? Password { get; init; }
    public IReadOnlyList<long>? TagIds { get; init; }
    public string? TextType { get; init; }
}

public sealed class UpdateTextRequest
{
    public required string Content { get; init; }
    public required string Domain { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
}

public sealed class DeleteTextRequest
{
    public required string Domain { get; init; }
    public required string Slug { get; init; }
}

public sealed class CreateTextData
{
    public string? CustomSlug { get; init; }
    public string? ShortUrl { get; init; }
    public string? Slug { get; init; }
}

public sealed class TextHistoryItem
{
    public string? ContentPreview { get; init; }
    public long CreatedAt { get; init; }
    public string? Domain { get; init; }
    public long Id { get; init; }
    public bool IsExpired { get; init; }
    public string? ShortUrl { get; init; }
    public string? Slug { get; init; }
    public string? TextType { get; init; }
    public string? Title { get; init; }
}
