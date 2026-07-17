using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using NerisLibrary;
using NerisLibrary.Exceptions;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.RequestModels;
using NerisLibraryTest.TestHelpers;

namespace NerisLibraryTest.QueryMethods;

public class EntityMethodsTests
{
    private const string BaseUrl = NerisBaseFixture.BaseUrl;

    [Fact]
    public async Task GetEntity_NullId_ThrowsArgumentNullException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetEntity(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEntities_InvalidModel_ThrowsValidationException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetEntities(new EntityRequestModel { State = "CAL" });

        await act.Should().ThrowAsync<ValidationException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEntities_BuildsQueryAndReturnsPageSet()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse(
            "{\"next_cursor\":null,\"entities\":[{\"neris_id\":\"ENT1\",\"name\":\"Test FD\"}]}");
        NerisBase neris = fixture.CreateNerisBase();

        EntityPageSet page = await neris.GetEntities(new EntityRequestModel { Name = "Test FD" });

        page.Entities.Should().ContainSingle(e => e.Neris_Id == "ENT1");
        var query = QueryHelpers.ParseQuery(fixture.Handler.Requests[1].Uri.Query);
        query["name"].ToString().Should().Be("Test FD");
        fixture.Handler.Requests[1].Uri.AbsolutePath.Should().EndWith("/entity/");
    }

    [Fact]
    public async Task PatchEntity_RemovesNerisIdAndNullsRequestedFields()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{\"neris_id\":\"ENT1\"}");
        NerisBase neris = fixture.CreateNerisBase();
        var entity = new EntityModel { Neris_Id = "ENT1", Name = "New Name" };

        string id = await neris.PatchEntity(entity, new HashSet<string> { "Website" });

        id.Should().Be("ENT1");
        RecordedRequest patch = fixture.Handler.Requests[1];
        patch.Method.Should().Be(HttpMethod.Patch);
        patch.Uri.ToString().Should().Be($"{BaseUrl}/entity/ENT1");
        using JsonDocument body = JsonDocument.Parse(patch.Body!);
        body.RootElement.TryGetProperty("neris_id", out _).Should().BeFalse();
        body.RootElement.GetProperty("name").GetString().Should().Be("New Name");
        body.RootElement.GetProperty("website").ValueKind.Should().Be(JsonValueKind.Null,
            "fieldsToNull entries must be explicitly nulled, not omitted");
    }

    [Fact]
    public async Task PatchEntity_NullEntity_ThrowsArgumentNullException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.PatchEntity(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task PatchEntity_MissingNerisId_ThrowsArgumentException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.PatchEntity(new EntityModel { Neris_Id = " " });

        await act.Should().ThrowExactlyAsync<ArgumentException>();
    }
}
