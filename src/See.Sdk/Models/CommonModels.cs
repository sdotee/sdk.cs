namespace See.Sdk.Models;

/// <summary>Standard S.EE API response envelope.</summary>
public sealed class ApiResponse<T>
{
    public int Code { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public bool? Success { get; init; }
}

/// <summary>Response envelope without a typed payload.</summary>
public sealed class ApiResponse
{
    public int Code { get; init; }
    public string? Message { get; init; }
    public object? Data { get; init; }
}

public sealed class AvailableDomains
{
    public IReadOnlyList<string> Domains { get; init; } = [];
}

public sealed class TagInfo
{
    public long Id { get; init; }
    public string? Name { get; init; }
}

public sealed class TagsData
{
    public IReadOnlyList<TagInfo> Tags { get; init; } = [];
}

public sealed class TokenCheckRequest
{
    public required string Token { get; init; }
}

public sealed class TokenCheckData
{
    public string? Token { get; init; }
    public long ExpiresAt { get; init; }
    public bool Valid { get; init; }
}

public sealed class UsageData
{
    public long ApiCountDay { get; init; }
    public long ApiCountDayLimit { get; init; }
    public long ApiCountMonth { get; init; }
    public long ApiCountMonthLimit { get; init; }
    public long FileCount { get; init; }
    public long LinkCountDay { get; init; }
    public long LinkCountDayLimit { get; init; }
    public long LinkCountMonth { get; init; }
    public long LinkCountMonthLimit { get; init; }
    public long QrcodeCountDay { get; init; }
    public long QrcodeCountDayLimit { get; init; }
    public long QrcodeCountMonth { get; init; }
    public long QrcodeCountMonthLimit { get; init; }
    public string? StorageUsageLimitMb { get; init; }
    public string? StorageUsageMb { get; init; }
    public long TextCountDay { get; init; }
    public long TextCountDayLimit { get; init; }
    public long TextCountMonth { get; init; }
    public long TextCountMonthLimit { get; init; }
    public long UploadCountDay { get; init; }
    public long UploadCountDayLimit { get; init; }
    public long UploadCountMonth { get; init; }
    public long UploadCountMonthLimit { get; init; }
}
