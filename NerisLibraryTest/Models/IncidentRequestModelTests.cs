using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using NerisSharp.Models.ElementModels.Incident;
using NerisSharp.Models.RequestModels;
using System.Globalization;

namespace NerisSharpTest.Models;

public class IncidentRequestModelTests
{
    private const string BaseUri = "https://host/v1/incident/";

    private static IncidentRequestModel CreateValidModel() => new() { Neris_Id_Entity = "ENT1" };

    private static Dictionary<string, StringValues> Query(IncidentRequestModel model)
        => QueryHelpers.ParseQuery(new Uri(model.CreateQueryURI(BaseUri)).Query);

    // --- Validate ---

    [Fact]
    public void Validate_MinimalModelWithEntityId_ReturnsTrue()
        => CreateValidModel().Validate().Should().BeTrue();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_MissingEntityId_ReturnsFalse(string? entityId)
        => new IncidentRequestModel { Neris_Id_Entity = entityId! }.Validate().Should().BeFalse();

    [Theory]
    [InlineData("C")]
    [InlineData("CAL")]
    public void Validate_StateNotTwoCharacters_ReturnsFalse(string state)
    {
        var model = CreateValidModel();
        model.State = state;
        model.Validate().Should().BeFalse();
    }

    [Fact]
    public void State_Setter_UppercasesValue()
    {
        var model = CreateValidModel();
        model.State = "ca";
        model.State.Should().Be("CA");
        model.Validate().Should().BeTrue();
    }

    [Fact]
    public void State_SetNull_ShouldBeNull()
    {
        var model = CreateValidModel();
        model.State = null!;
        model.State.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Validate_PageSizeOutOfRange_ReturnsFalse(int pageSize)
    {
        var model = CreateValidModel();
        model.Page_Size = pageSize;
        model.Validate().Should().BeFalse();
    }

    [Fact]
    public void Validate_SortByNotInWhitelist_ReturnsFalse()
    {
        var model = CreateValidModel();
        model.Sort_By = "bogus_column";
        model.Validate().Should().BeFalse();
    }

    [Fact]
    public void Validate_SortByInWhitelist_ReturnsTrue()
    {
        var model = CreateValidModel();
        model.Sort_By = "call_create";
        model.Validate().Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidIncidentType_ReturnsFalse()
    {
        var model = CreateValidModel();
        model.Incident_Types = new List<IncidentType> { new IncidentType() }; //Uninitialized Incident Type is invalid
        model.Validate().Should().BeFalse();
    }

    // --- CreateQueryURI ---

    [Fact]
    public void CreateQueryURI_IncludesSetPropertiesAsLowercaseParamsAndSkipsNulls()
    {
        var model = CreateValidModel();
        model.Incident_Number = "2026-000123";

        var query = Query(model);

        query["neris_id_entity"].ToString().Should().Be("ENT1");
        query["incident_number"].ToString().Should().Be("2026-000123");
        query["page_size"].ToString().Should().Be("10");
        query.Should().NotContainKey("state", "null properties are skipped");
        query.Should().NotContainKey("sort_ascending", "excluded fields must not appear");
        query.Should().NotContainKey("sortbyvalues");
    }

    [Theory]
    [InlineData(true, "ASCENDING")]
    [InlineData(false, "DESCENDING")]
    public void CreateQueryURI_SortAscending_MapsToSortDirection(bool ascending, string expected)
    {
        var model = CreateValidModel();
        model.Sort_Ascending = ascending;
        Query(model)["sort_direction"].ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData(true, "geojson")]
    [InlineData(false, "url")]
    public void CreateQueryURI_GeoFormatJson_MapsToGeoFormat(bool geoJson, string expected)
    {
        var model = CreateValidModel();
        model.Geo_Format_Json = geoJson;
        var query = Query(model);
        query["geo_format"].ToString().Should().Be(expected);
        query.Should().NotContainKey("geo_format_json", "excluded fields must not appear");

    }

    [Fact]
    public void CreateQueryURI_DateTimeOffset_UsesRoundTripIsoFormat()
    {
        var start = new DateTimeOffset(2026, 1, 2, 3, 4, 5, TimeSpan.Zero);
        var model = CreateValidModel();
        model.Call_Create_Start = start;

        Query(model)["call_create_start"].ToString()
            .Should().Be(start.ToString("O", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CreateQueryURI_IncidentTypes_AppendedAsRepeatedParams()
    {
        var model = CreateValidModel();
        model.Incident_Types = new List<IncidentType>
        {
            new IncidentType (IncidentTypeEnum.FIRE),
            new IncidentType (IncidentTypeEnum.MEDICAL)
        };

        var query = Query(model);

        query["incident_types"].ToArray().Should().BeEquivalentTo("FIRE", "MEDICAL");
    }

}
