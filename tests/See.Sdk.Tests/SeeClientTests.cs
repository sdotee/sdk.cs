using System.Net;
using System.Text;
using See.Sdk.Models;
using Xunit;

namespace See.Sdk.Tests;

public sealed class SeeClientTests
{
    [Fact]
    public async Task Uses_default_base_uri_when_http_client_has_none()
    {
        var handler = new RecordingHandler("""{"code":200,"data":{}}""");
        var client = new SeeClient(new HttpClient(handler), "test-key");

        await client.GetUsageAsync(TestContext.Current.CancellationToken);

        Assert.Equal(new Uri("https://s.ee/api/v1/usage"), handler.Request!.RequestUri);
        Assert.Equal(new Uri("https://s.ee/api/v1/"), SeeClient.BaseUri);
    }

    [Fact]
    public async Task Does_not_mutate_http_client_when_applying_default_base_uri()
    {
        var handler = new RecordingHandler("""{"code":200,"data":{}}""");
        var httpClient = new HttpClient(handler);
        await httpClient.GetAsync("https://s.ee/health", TestContext.Current.CancellationToken);
        var client = new SeeClient(httpClient, "test-key");

        await client.GetUsageAsync(TestContext.Current.CancellationToken);

        Assert.Null(httpClient.BaseAddress);
        Assert.Equal(new Uri("https://s.ee/api/v1/usage"), handler.Request!.RequestUri);
    }

    [Fact]
    public async Task CreateShortUrl_sends_authorization_and_snake_case_json()
    {
        var handler = new RecordingHandler("""{"code":200,"message":"ok","data":{"short_url":"https://s.ee/x","slug":"x"}}""");
        var client = CreateClient(handler);

        var response = await client.CreateShortUrlAsync(new CreateShortUrlRequest
        {
            Domain = "s.ee",
            TargetUrl = "https://example.com",
            CustomSlug = "x"
        }, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Post, handler.Request!.Method);
        Assert.Equal("test-key", handler.Request.Headers.GetValues("Authorization").Single());
        Assert.Contains("\"target_url\":\"https://example.com\"", handler.Body);
        Assert.Equal("https://s.ee/x", response.Data!.ShortUrl);
    }

    [Fact]
    public async Task Visit_statistics_encodes_query_parameters()
    {
        var handler = new RecordingHandler("""{"code":200,"data":{"visit_count":7}}""");
        var client = CreateClient(handler);

        var response = await client.GetLinkVisitStatisticsAsync(
            "s.ee",
            "a/b",
            StatisticsPeriod.Monthly,
            TestContext.Current.CancellationToken);

        Assert.Equal("?domain=s.ee&slug=a%2Fb&period=monthly", handler.Request!.RequestUri!.Query);
        Assert.Equal(7, response.Data!.VisitCount);
    }

    [Fact]
    public async Task Non_success_response_throws_with_response_body()
    {
        var handler = new RecordingHandler("""{"code":401,"message":"expired"}""", HttpStatusCode.Unauthorized);
        var client = CreateClient(handler);

        var exception = await Assert.ThrowsAsync<SeeApiException>(
            () => client.GetUsageAsync(TestContext.Current.CancellationToken));

        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        Assert.Contains("expired", exception.ResponseBody);
    }

    [Fact]
    public async Task Token_check_does_not_send_client_authorization()
    {
        var handler = new RecordingHandler("""{"code":200,"data":{"valid":true}}""");
        var client = CreateClient(handler);

        await client.CheckTokenAsync(
            new TokenCheckRequest { Token = "token-to-check" },
            TestContext.Current.CancellationToken);

        Assert.False(handler.Request!.Headers.Contains("Authorization"));
    }

    [Fact]
    public async Task Upload_file_uses_multipart_contract()
    {
        var handler = new RecordingHandler("""{"code":200,"data":{"filename":"hello.txt"}}""");
        var client = CreateClient(handler);
        await using var file = new MemoryStream("hello"u8.ToArray());

        var response = await client.UploadFileAsync(
            file,
            "hello.txt",
            "s.ee",
            "hello",
            TestContext.Current.CancellationToken);

        Assert.StartsWith("multipart/form-data", handler.ContentType);
        Assert.Contains("name=file", handler.Body);
        Assert.Contains("name=custom_slug", handler.Body);
        Assert.Equal("hello.txt", response.Data!.Filename);
        Assert.True(file.CanRead);
    }

    [Fact]
    public async Task Tus_chunk_sends_protocol_headers()
    {
        var handler = new RecordingHandler(string.Empty, HttpStatusCode.NoContent);
        var client = CreateClient(handler);
        await using var chunk = new MemoryStream([1, 2, 3]);

        await client.UploadTusChunkAsync("upload/1", chunk, 42, TestContext.Current.CancellationToken);

        Assert.Equal(HttpMethod.Patch, handler.Request!.Method);
        Assert.EndsWith("/upload%2F1", handler.Request.RequestUri!.AbsolutePath);
        Assert.Equal("1.0.0", handler.Request.Headers.GetValues("Tus-Resumable").Single());
        Assert.Equal("42", handler.Request.Headers.GetValues("Upload-Offset").Single());
        Assert.Equal("application/offset+octet-stream", handler.ContentType);
        Assert.True(chunk.CanRead);
    }

    [Fact]
    public async Task Tus_status_requires_valid_upload_offset()
    {
        var handler = new RecordingHandler(string.Empty, HttpStatusCode.NoContent);
        var client = CreateClient(handler);

        var exception = await Assert.ThrowsAsync<SeeApiException>(
            () => client.GetTusUploadStatusAsync("upload-1", TestContext.Current.CancellationToken));

        Assert.Contains("Upload-Offset", exception.ResponseBody);
    }

    [Fact]
    public async Task Tus_chunk_rejects_invalid_arguments_before_sending()
    {
        var handler = new RecordingHandler(string.Empty, HttpStatusCode.NoContent);
        var client = CreateClient(handler);
        await using var chunk = new MemoryStream([1]);

        await Assert.ThrowsAsync<ArgumentException>(
            () => client.UploadTusChunkAsync(" ", chunk, 0, TestContext.Current.CancellationToken));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => client.UploadTusChunkAsync("upload-1", chunk, -1, TestContext.Current.CancellationToken));
        Assert.Null(handler.Request);
    }

    private static SeeClient CreateClient(HttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://api.s.ee") }, "test-key");

    private sealed class RecordingHandler(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK) : HttpMessageHandler
    {
        public HttpRequestMessage? Request { get; private set; }
        public string Body { get; private set; } = string.Empty;
        public string? ContentType { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            if (request.Content is not null)
            {
                Body = await request.Content.ReadAsStringAsync(cancellationToken);
                ContentType = request.Content.Headers.ContentType?.ToString();
            }

            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
            };
        }
    }
}
