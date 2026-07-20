using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using NerisLibrary;
using NerisLibrary.Exceptions;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Models.RequestModels;
using NerisLibraryTest.TestHelpers;

namespace NerisLibraryTest.QueryMethods;

public class IncidentMethodsTests
{
    private const string BaseUrl = NerisBaseFixture.BaseUrl;

    private static (NerisBaseFixture fixture, NerisBase neris) CreateLoggedIn(params string[] apiResponses)
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        foreach (string response in apiResponses)
        {
            fixture.Handler.QueueJsonResponse(response);
        }
        return (fixture, fixture.CreateNerisBase());
    }

    // --- GetIncidentById ---

    [Fact]
    public async Task GetIncidentById_BuildsNestedRouteAndReturnsModel()
    {
        var (fixture, neris) = CreateLoggedIn("{\"neris_id\":\"INC1\"}");

        IncidentModel model = await neris.GetIncidentById("ENT1", "INC1");

        model.Neris_Id.Should().Be("INC1");
        fixture.Handler.Requests[1].Method.Should().Be(HttpMethod.Get);
        fixture.Handler.Requests[1].Uri.ToString().Should().Be($"{BaseUrl}/incident/ENT1/INC1");
    }

    [Fact]
    public async Task GetIncidentById_GeoJsonFlag_AddsGeoFormatQueryParam()
    {
        var (fixture, neris) = CreateLoggedIn("{}");

        await neris.GetIncidentById("ENT1", "INC1", geoformatGeoJson: true);

        var query = QueryHelpers.ParseQuery(fixture.Handler.Requests[1].Uri.Query);
        query["geo_format"].ToString().Should().Be("geojson");
    }

    [Fact]
    public async Task GetIncidentById_EntityModelOverload_UsesModelNerisId()
    {
        var (fixture, neris) = CreateLoggedIn("{}");

        await neris.GetIncidentById(new EntityModel { Neris_Id = "ENT1" }, "INC1");

        fixture.Handler.Requests[1].Uri.ToString().Should().Be($"{BaseUrl}/incident/ENT1/INC1");
    }

    [Theory]
    [InlineData(null, "INC1")]
    [InlineData("  ", "INC1")]
    [InlineData("ENT1", null)]
    [InlineData("ENT1", "  ")]
    public async Task GetIncidentById_NullOrWhitespaceIds_ThrowsArgumentNullException(string? entityId, string? incidentId)
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetIncidentById(entityId!, incidentId!);

        await act.Should().ThrowAsync<ArgumentNullException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }

    // --- GetIncidentsPage ---

    [Fact]
    public async Task GetIncidentsPage_NullModel_ThrowsArgumentNullException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetIncidentsPage(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetIncidentsPage_InvalidModel_ThrowsValidationException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetIncidentsPage(new IncidentRequestModel()); // missing Neris_Id_Entity

        await act.Should().ThrowAsync<ValidationException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task GetIncidentsPage_BuildsQueryFromModelAndReturnsPageSet()
    {
        var (fixture, neris) = CreateLoggedIn(
            "{\"next_cursor\":\"CUR\",\"prev_cursor\":null,\"incidents\":[{\"neris_id\":\"A\"}]}");

        IncidentPageSet page = await neris.GetIncidentsPage(new IncidentRequestModel { Neris_Id_Entity = "ENT1" });

        page.Next_Cursor.Should().Be("CUR");
        page.Incidents.Should().ContainSingle(i => i.Neris_Id == "A");
        var query = QueryHelpers.ParseQuery(fixture.Handler.Requests[1].Uri.Query);
        query["neris_id_entity"].ToString().Should().Be("ENT1");
        fixture.Handler.Requests[1].Uri.AbsolutePath.Should().EndWith("/incident/");
    }

    // --- GetAllIncidents ---

    [Fact]
    public async Task GetAllIncidents_FollowsNextCursorAcrossPagesAndForcesPageSize100()
    {
        var (fixture, neris) = CreateLoggedIn(
            "{\"next_cursor\":\"CURSOR_2\",\"incidents\":[{\"neris_id\":\"A\"}]}",
            "{\"next_cursor\":null,\"incidents\":[{\"neris_id\":\"B\"}]}");

        List<IncidentModel> all = await neris.GetAllIncidents(new IncidentRequestModel { Neris_Id_Entity = "ENT1" });

        all.Select(i => i.Neris_Id).Should().Equal("A", "B");
        fixture.Handler.Requests.Should().HaveCount(3, "token + two pages");
        var firstPageQuery = QueryHelpers.ParseQuery(fixture.Handler.Requests[1].Uri.Query);
        firstPageQuery["page_size"].ToString().Should().Be("100");
        firstPageQuery.Should().NotContainKey("cursor");
        var secondPageQuery = QueryHelpers.ParseQuery(fixture.Handler.Requests[2].Uri.Query);
        secondPageQuery["cursor"].ToString().Should().Be("CURSOR_2");
    }

    // --- PostIncident ---

    [Fact]
    public async Task PostIncident_SendsSnakeCaseBodyAndReturnsNewId()
    {
        var (fixture, neris) = CreateLoggedIn("{\"neris_id\":\"NEW_INC\"}");

        string id = await neris.PostIncident("ENT1", new IncidentModel { Neris_Id = "LOCAL_ID" });

        id.Should().Be("NEW_INC");
        RecordedRequest post = fixture.Handler.Requests[1];
        post.Method.Should().Be(HttpMethod.Post);
        post.Uri.ToString().Should().Be($"{BaseUrl}/incident/ENT1");
        post.ContentType.Should().Be("application/json");
        using JsonDocument body = JsonDocument.Parse(post.Body!);
        body.RootElement.GetProperty("neris_id").GetString().Should().Be("LOCAL_ID");
    }

    [Fact]
    public async Task PostIncident_WhitespaceEntityId_ThrowsArgumentException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.PostIncident("  ", new IncidentModel());

        await act.Should().ThrowExactlyAsync<ArgumentException>();
    }

    [Fact]
    public async Task PostIncident_NullIncident_ThrowsArgumentNullException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.PostIncident("ENT1", null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    // --- PutIncident ---

    [Fact]
    public async Task PutIncident_StripsServerManagedFieldsFromBody()
    {
        var (fixture, neris) = CreateLoggedIn("{}");
        var incident = new IncidentModel
        {
            Neris_Id = "INC1",
            Submitter_Account_Type = SubmitterAccountTypes.CAD,
            Incident_Status = new IncidentStatus()
        };

        bool result = await neris.PutIncident("ENT1", incident);

        result.Should().BeTrue();
        RecordedRequest put = fixture.Handler.Requests[1];
        put.Method.Should().Be(HttpMethod.Put);
        put.Uri.ToString().Should().Be($"{BaseUrl}/incident/ENT1/INC1");
        using JsonDocument body = JsonDocument.Parse(put.Body!);
        body.RootElement.TryGetProperty("neris_id", out _).Should().BeFalse();
        body.RootElement.TryGetProperty("submitter_account_type", out _).Should().BeFalse();
        body.RootElement.TryGetProperty("incident_status", out _).Should().BeFalse();
    }

    [Fact]
    public async Task PutIncident_NullArguments_Throw()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        await ((Func<Task>)(() => neris.PutIncident("  ", new IncidentModel())))
            .Should().ThrowAsync<ArgumentNullException>();
        await ((Func<Task>)(() => neris.PutIncident("ENT1", null!)))
            .Should().ThrowAsync<ArgumentNullException>();
    }
}
