using System.Net;
using System.Text;

namespace NerisLibraryTest.TestHelpers;

/// <summary>
/// Replays scripted responses in FIFO order and records every request sent through it.
/// Throws if a request arrives with no queued response, so tests fail loudly.
/// </summary>
public sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();

    public List<RecordedRequest> Requests { get; } = new();

    public void QueueResponse(HttpResponseMessage response) => _responses.Enqueue(response);

    public void QueueJsonResponse(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        QueueResponse(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? body = request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        Requests.Add(new RecordedRequest(
            request.Method,
            request.RequestUri!,
            request.Headers.Authorization?.Scheme,
            request.Headers.Authorization?.Parameter,
            body,
            request.Content?.Headers.ContentType?.MediaType));

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException($"No response queued for {request.Method} {request.RequestUri}");
        }
        return _responses.Dequeue();
    }
}
