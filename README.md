# see sdk

A strongly typed asynchronous .NET client for the S.EE API, supporting .NET 8 and .NET 10.

## Installation

```bash
dotnet add package see.sdk
```

## Quick Start

You can use the SDK to create a short URL as follows:

```csharp
using See.Sdk;
using See.Sdk.Models;

using var httpClient = new HttpClient();
var client = new SeeClient(httpClient, Environment.GetEnvironmentVariable("SEE_API_KEY")!);

var response = await client.CreateShortUrlAsync(new CreateShortUrlRequest
{
    Domain = "s.ee",
    TargetUrl = "https://example.com",
    Title = "Example"
});

Console.WriteLine(response.Data?.ShortUrl);
```

Do not store API keys in source code or logs. In production, use dependency injection to manage the `HttpClient` lifetime and read the key from a secure configuration source.

## Features

- Short URLs: create, update, delete, history, visit statistics, simple mode, domains, and usage
- Text sharing: create, update, delete, history, and available domains
- Files: multipart upload, history, deletion, and private download URLs
- Large files: create, TUS PATCH/HEAD/DELETE, progress, completion, and cancellation
- QR codes: create, delete, and history
- Bio pages: create, update, delete, and history
- Tags and token validation

All methods support `CancellationToken`. Non-2xx HTTP responses throw `SeeApiException`, which contains the status code and raw response body. The `code`, `message`, and `data` fields from API responses are preserved.

See the [API reference](https://github.com/sdotee/sdk.cs/blob/main/docs/api-reference.md) for the complete method list and the [contribution guide](https://github.com/sdotee/sdk.cs/blob/main/CONTRIBUTING.md) for build and release instructions.

## Local Validation

```bash
dotnet restore See.DotNet.Sdk.slnx
dotnet build See.DotNet.Sdk.slnx --configuration Release --no-restore
dotnet test See.DotNet.Sdk.slnx --configuration Release --no-build
dotnet pack src/See.Sdk/See.Sdk.csproj --configuration Release --no-build --output artifacts
```

## License

See the [LICENSE file](https://github.com/sdotee/sdk.cs/blob/main/LICENSE) for details.