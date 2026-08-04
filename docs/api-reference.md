# API Reference

The entry point is `See.Sdk.SeeClient`, and DTOs are in `See.Sdk.Models`. The constructor accepts an `HttpClient` and an API key. Requests use `https://s.ee/api/v1/` by default; configure `HttpClient.BaseAddress` to target another server.

| Area         | SDK Methods                                                                                                                                                                                             |
| ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Short URLs   | `GetAvailableDomainsAsync`, `CreateShortUrlAsync`, `CreateShortUrlSimpleAsync`, `UpdateShortUrlAsync`, `DeleteShortUrlAsync`, `GetLinkHistoryAsync`, `GetLinkVisitStatisticsAsync`                      |
| Text sharing | `GetTextAvailableDomainsAsync`, `CreateTextAsync`, `UpdateTextAsync`, `DeleteTextAsync`, `GetTextHistoryAsync`                                                                                          |
| Files        | `GetFileAvailableDomainsAsync`, `UploadFileAsync`, `GetFileHistoryAsync`, `DeleteFileAsync`, `GetPrivateFileDownloadUrlAsync`                                                                           |
| Large files  | `CreateLargeFileUploadAsync`, `UploadTusChunkAsync`, `GetTusUploadStatusAsync`, `DeleteTusUploadAsync`, `GetLargeFileUploadProgressAsync`, `CompleteLargeFileUploadAsync`, `CancelLargeFileUploadAsync` |
| QR codes     | `CreateQrCodeAsync`, `DeleteQrCodeAsync`, `GetQrCodeHistoryAsync`                                                                                                                                       |
| Bio pages    | `CreateBioPageAsync`, `UpdateBioPageAsync`, `DeleteBioPageAsync`, `GetBioPageHistoryAsync`                                                                                                              |
| Account      | `GetTagsAsync`, `GetUsageAsync`, `CheckTokenAsync`                                                                                                                                                      |

## Large File Upload Flow

1. Call `CreateLargeFileUploadAsync` to create a session. If `FastUpload` is `true`, no data transfer is required.
2. Call `UploadTusChunkAsync` according to the TUS 1.0 protocol, passing the current offset with each chunk.
3. Call `GetTusUploadStatusAsync` to resume an interrupted upload, or `GetLargeFileUploadProgressAsync` to retrieve application-level progress.
4. After uploading all bytes, call `CompleteLargeFileUploadAsync`.
5. To abort the upload, call `CancelLargeFileUploadAsync`. The underlying TUS resource can also be deleted with `DeleteTusUploadAsync`.

The SDK does not close file or chunk streams supplied by the caller.

## Error Handling

Non-success HTTP responses throw `SeeApiException`, which exposes the HTTP status code and raw response body. Successful JSON responses retain the API-level `code`, `message`, and `data` envelope.
