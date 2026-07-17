using System.Net;
using System.Text.Json;
using FluentAssertions;
using NerisLibrary;
using NerisLibrary.Models.ElementModels;
using NerisLibraryTest.TestHelpers;

namespace NerisLibraryTest.QueryMethods;

public class StationMethodsTests
{
    private const string Base = NerisBaseFixture.BaseUrl;

    [Fact]
    public async Task PostStation_BuildsEntityNestedRouteAndReturnsNewId()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"NEW_ST\"}");
        NerisBase neris = fixture.CreateNerisBase();

        string id = await neris.PostStation("ENT1", new StationModel { Station_Id = "Station 5" });

        id.Should().Be("NEW_ST");
        RecordedRequest post = fixture.Handler.Requests[1];
        post.Method.Should().Be(HttpMethod.Post);
        post.Uri.ToString().Should().Be($"{Base}/entity/ENT1/station");
        using JsonDocument body = JsonDocument.Parse(post.Body!);
        body.RootElement.GetProperty("station_id").GetString().Should().Be("Station 5");
    }

    [Fact]
    public async Task PostStation_EntityModelOverload_UsesModelNerisId()
    {
        //is this something we really need to be testing? I mean its here and its free but damn I don't think this is important
        //like we aren't testing the spirit of it
        var fixture = new NerisBaseFixture(); 
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"NEW_ST\"}");
        NerisBase neris = fixture.CreateNerisBase();

        await neris.PostStation(new EntityModel { Neris_Id = "ENT1" }, new StationModel());

        fixture.Handler.Requests[1].Uri.ToString().Should().Be($"{Base}/entity/ENT1/station");
    }

    [Fact]
    public async Task PostStation_NullArguments_Throw()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        await ((Func<Task>)(() => neris.PostStation("  ", new StationModel())))
            .Should().ThrowAsync<ArgumentNullException>();
        await ((Func<Task>)(() => neris.PostStation("ENT1", null!)))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task PatchStation_RoutesToStationAndStripsNerisIdAndNullsFields()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"ST1\"}");
        NerisBase neris = fixture.CreateNerisBase();
        var update = new StationModel { Neris_Id = "ST1", Address_Line_1 = "123 Main St" };

        string id = await neris.PatchStation("ENT1", update, new HashSet<string> { "City" });

        id.Should().Be("ST1");
        RecordedRequest patch = fixture.Handler.Requests[1];
        patch.Method.Should().Be(HttpMethod.Patch);
        patch.Uri.ToString().Should().Be($"{Base}/entity/ENT1/station/ST1");
        using JsonDocument body = JsonDocument.Parse(patch.Body!);
        body.RootElement.TryGetProperty("neris_id", out _).Should().BeFalse();
        body.RootElement.GetProperty("address_line_1").GetString().Should().Be("123 Main St");
        body.RootElement.GetProperty("city").ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task PatchStation_MissingNerisId_ThrowsArgumentException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.PatchStation("ENT1", new StationModel());

        await act.Should().ThrowExactlyAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteStation_SendsDeleteToStationRouteAndReturnsTrue()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueResponse(new HttpResponseMessage(HttpStatusCode.OK));
        NerisBase neris = fixture.CreateNerisBase();

        bool result = await neris.DeleteStation("ENT1", "ST1");

        result.Should().BeTrue();
        fixture.Handler.Requests[1].Method.Should().Be(HttpMethod.Delete);
        fixture.Handler.Requests[1].Uri.ToString().Should().Be($"{Base}/entity/ENT1/station/ST1");
    }

    [Theory]
    [InlineData(null, "ST1")]
    [InlineData("ENT1", "  ")]
    public async Task DeleteStation_NullOrWhitespaceIds_ThrowsArgumentNullException(string? entityId, string? stationId)
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.DeleteStation(entityId!, stationId!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
