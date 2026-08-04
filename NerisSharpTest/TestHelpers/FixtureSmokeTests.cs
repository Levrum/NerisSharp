using FluentAssertions;
using NerisSharp;
using NerisSharp.Models.ElementModels;

namespace NerisSharpTest.TestHelpers;

public class FixtureSmokeTests
{
    [Fact]
    public async Task GetEntity_WithQueuedTokenAndResponse_ReturnsEntityAndRecordsRequests()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"FD24001\",\"name\":\"Test FD\"}");
        NerisBase neris = fixture.CreateNerisBase();

        EntityModel? entity = await neris.GetEntity("FD24001");

        entity.Should().NotBeNull();
        entity!.Neris_Id.Should().Be("FD24001");
        entity.Name.Should().Be("Test FD");
        fixture.Handler.Requests.Should().HaveCount(2);
        fixture.Handler.Requests[0].Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/token/");
        fixture.Handler.Requests[1].Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/entity/FD24001");
        fixture.Handler.Requests[1].AuthScheme.Should().Be("Bearer");
        fixture.Handler.Requests[1].AuthParameter.Should().Be(NerisBaseFixture.AccessToken);
    }
}
