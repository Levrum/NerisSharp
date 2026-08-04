using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NerisSharp;
using NerisSharpTest.TestHelpers;
using System.Net;

namespace NerisSharpTest.QueryMethods;

public class NerisBaseErrorTests
{
    [Fact]
    public async Task ApiCall_NonSuccessStatus_ThrowsHttpRequestExceptionContainingResponseBody()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"detail\":\"entity not found\"}", HttpStatusCode.NotFound);
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetEntity("MISSING");

        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("*entity not found*");
    }

    [Fact]
    public async Task ApiCall_NonSuccessStatus_LogsErrorThroughProvidedLogger()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"detail\":\"entity not found\"}", HttpStatusCode.NotFound);
        NerisBase neris = fixture.CreateNerisBase(withLogger: true);

        Func<Task> act = () => neris.GetEntity("MISSING");
        await act.Should().ThrowAsync<HttpRequestException>();

        fixture.Logger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("entity not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Login_TokenEndpointReturnsError_ThrowsHttpRequestExceptionContainingBody()
    {
        var fixture = new NerisBaseFixture();
        fixture.Handler.QueueJsonResponse("{\"detail\":\"bad credentials\"}", HttpStatusCode.Unauthorized);
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetEntity("ENT1");

        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("*bad credentials*");
        fixture.Handler.Requests.Should().HaveCount(1);
    }
}
