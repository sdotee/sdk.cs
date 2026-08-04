using System.Net;

namespace See.Sdk;

internal sealed class NonDisposingStreamContent(Stream source) : HttpContent
{
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) =>
        source.CopyToAsync(stream);

    protected override Task SerializeToStreamAsync(
        Stream stream,
        TransportContext? context,
        CancellationToken cancellationToken) =>
        source.CopyToAsync(stream, cancellationToken);

    protected override bool TryComputeLength(out long length)
    {
        if (source.CanSeek)
        {
            length = source.Length - source.Position;
            return true;
        }

        length = 0;
        return false;
    }
}
