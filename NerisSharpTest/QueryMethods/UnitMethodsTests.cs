using FluentAssertions;
using NerisSharp;
using NerisSharp.Models.ElementModels;
using NerisSharpTest.TestHelpers;
using System.Net;
using System.Text.Json;

namespace NerisSharpTest.QueryMethods;

public class UnitMethodsTests
{
    private const string BaseUrl = NerisBaseFixture.BaseUrl;

    [Fact]
    public async Task PostUnit_BuildsEntityStationNestedRouteAndReturnsNewId()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"NEW_UNIT\"}");
        NerisBase neris = fixture.CreateNerisBase();

        string id = await neris.PostUnit("ENT1", "ST1", new UnitModel { Cad_Designation_1 = "E5" });

        id.Should().Be("NEW_UNIT");
        RecordedRequest post = fixture.Handler.Requests[1];
        post.Method.Should().Be(HttpMethod.Post);
        post.Uri.ToString().Should().Be($"{BaseUrl}/entity/ENT1/station/ST1/unit");
        using JsonDocument body = JsonDocument.Parse(post.Body!);
        body.RootElement.GetProperty("cad_designation_1").GetString().Should().Be("E5");
    }

    [Fact]
    public async Task PostUnit_ModelOverload_UsesModelIds()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"NEW_UNIT\"}");
        NerisBase neris = fixture.CreateNerisBase();

        await neris.PostUnit(
            new EntityModel { Neris_Id = "ENT1" },
            new StationModel { Neris_Id = "ST1" },
            new UnitModel());

        fixture.Handler.Requests[1].Uri.ToString().Should().Be($"{BaseUrl}/entity/ENT1/station/ST1/unit");
    }

    [Theory]
    [InlineData(null, "ST1")]
    [InlineData("ENT1", "  ")]
    public async Task PostUnit_NullOrWhitespaceIds_ThrowsArgumentNullException(string? entityId, string? stationId)
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.PostUnit(entityId!, stationId!, new UnitModel());

        await act.Should().ThrowAsync<ArgumentNullException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task PatchUnit_RoutesToUnitAndStripsNerisIdAndNullsFields()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"U1\"}");
        NerisBase neris = fixture.CreateNerisBase();
        var update = new UnitModel { Neris_Id = "U1", Cad_Designation_1 = "E5" };

        string id = await neris.PatchUnit("ENT1", "ST1", update, new HashSet<string> { "Cad_Designation_2" });

        id.Should().Be("U1");
        RecordedRequest patch = fixture.Handler.Requests[1];
        patch.Method.Should().Be(HttpMethod.Patch);
        patch.Uri.ToString().Should().Be($"{BaseUrl}/entity/ENT1/station/ST1/unit/U1");
        using JsonDocument body = JsonDocument.Parse(patch.Body!);
        body.RootElement.TryGetProperty("neris_id", out _).Should().BeFalse();
        body.RootElement.GetProperty("cad_designation_1").GetString().Should().Be("E5");
        body.RootElement.GetProperty("cad_designation_2").ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task PatchUnit_MissingNerisId_ThrowsArgumentException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.PatchUnit("ENT1", "ST1", new UnitModel());

        await act.Should().ThrowExactlyAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteUnit_SendsDeleteToUnitRouteAndReturnsTrue()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueResponse(new HttpResponseMessage(HttpStatusCode.OK));
        NerisBase neris = fixture.CreateNerisBase();

        bool result = await neris.DeleteUnit("ENT1", "ST1", "U1");

        result.Should().BeTrue();
        fixture.Handler.Requests[1].Method.Should().Be(HttpMethod.Delete);
        fixture.Handler.Requests[1].Uri.ToString().Should().Be($"{BaseUrl}/entity/ENT1/station/ST1/unit/U1");
    }

    [Theory]
    [InlineData(null, "ST1", "U1")]
    [InlineData("ENT1", "  ", "U1")]
    [InlineData("ENT1", "ST1", "")]
    public async Task DeleteUnit_NullOrWhitespaceIds_ThrowsArgumentNullException(
        string? entityId, string? stationId, string? unitId)
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.DeleteUnit(entityId!, stationId!, unitId!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
