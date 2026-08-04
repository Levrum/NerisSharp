namespace NerisSharpTest.TestHelpers;

/// <summary>
/// Snapshot of one outgoing request, captured before the library disposes the HttpRequestMessage.
/// </summary>
public sealed class RecordedRequest
{
    public RecordedRequest(HttpMethod method, Uri uri, string? authScheme, string? authParameter, string? body, string? contentType)
    {
        Method = method;
        Uri = uri;
        AuthScheme = authScheme;
        AuthParameter = authParameter;
        Body = body;
        ContentType = contentType;
    }

    public HttpMethod Method { get; }
    public Uri Uri { get; }
    public string? AuthScheme { get; }
    public string? AuthParameter { get; }
    public string? Body { get; }
    public string? ContentType { get; }
}
