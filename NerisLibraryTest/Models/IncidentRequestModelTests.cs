using System.Globalization;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Models.RequestModels;

namespace NerisLibraryTest.Models;

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
    public void State_SetNull_ThrowsNullReferenceException()
    {
        // Pins current behavior — setter calls value.ToUpper() with no null check.
        // Flagged as a library bug in the design spec; do not "fix" here.
        var model = CreateValidModel();
        Action act = () => model.State = null!;
        act.Should().Throw<NullReferenceException>();
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
        model.Incident_Types = new List<IncidentType> { new IncidentType() }; // DTO ctor leaves Valid = false
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
        Query(model)["geo_format"].ToString().Should().Be(expected);
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
            new IncidentType { Type = "TYPE_A", Valid = true },
            new IncidentType { Type = "TYPE_B", Valid = true }
        };

        var query = Query(model);

        query["incident_types"].ToArray().Should().BeEquivalentTo("TYPE_A", "TYPE_B");
    }

    [Fact]
    public void CreateQueryURI_EmitsStrayGeoFormatJsonParam()
    {
        // Pins current behavior — excludedFields lists "Geo_Format_Url" but the property is
        // named Geo_Format_Json, so reflection also emits geo_format_json alongside geo_format.
        // Flagged as a library smell in the design spec; do not "fix" here.
        var query = Query(CreateValidModel());
        query.Should().ContainKey("geo_format_json");
    }
}
