using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using NerisSharp.Models.RequestModels;

namespace NerisSharpTest.Models;

public class EntityRequestModelTests
{
    private const string BaseUri = "https://host/v1/entity/";

    private static Dictionary<string, StringValues> Query(EntityRequestModel model)
        => QueryHelpers.ParseQuery(new Uri(model.CreateQueryURI(BaseUri)).Query);

    [Fact]
    public void Validate_DefaultModel_ReturnsTrue()
        => new EntityRequestModel().Validate().Should().BeTrue();

    [Theory]
    [InlineData("C")]
    [InlineData("CAL")]
    public void Validate_StateNotTwoCharacters_ReturnsFalse(string state)
        => new EntityRequestModel { State = state }.Validate().Should().BeFalse();

    [Fact]
    public void State_Setter_UppercasesValue()
    {
        var model = new EntityRequestModel { State = "or" };
        model.State.Should().Be("OR");
        model.Validate().Should().BeTrue();
    }

    [Fact]
    public void State_SetNull_ShouldBeNull()
    {
        var model = new EntityRequestModel();
        model.State = null;
        model.State.Should().BeNull();
    }

    [Fact]
    public void Validate_SortByNotInWhitelist_ReturnsFalse()
        => new EntityRequestModel { Sort_by = "bogus" }.Validate().Should().BeFalse();

    [Fact]
    public void Validate_SortByInWhitelist_ReturnsTrue()
        => new EntityRequestModel { Sort_by = "name" }.Validate().Should().BeTrue();

    [Fact]
    public void CreateQueryURI_IncludesPropertiesAndSortDirection()
    {
        var model = new EntityRequestModel { Name = "Test FD", Neris_id = "ENT1", Sort_Ascending = false, Page_Number = 2, Page_Size = 20 };

        var query = Query(model);

        query["name"].ToString().Should().Be("Test FD");
        query["neris_id"].ToString().Should().Be("ENT1");
        query["sort_direction"].ToString().Should().Be("DESCENDING");
        query.Should().NotContainKey("state", "null properties are skipped");
        query["page_number"].ToString().Should().Be("2");
        query["page_size"].ToString().Should().Be("20");
    }

    [Fact]
    public void CreateQueryURI_EnumBackedStringProperties_UseEnumNameAndSkipRawEnums()
    {
        var model = new EntityRequestModel
        {
            Entity_Class_Enum = EntityRequestModel.EntityClassTypes.FIRE_DEPARTMENT,
            Entity_Subtype_Enum = EntityRequestModel.EntitySubtypes.LOCAL
        };

        var query = Query(model);

        query["entity_class"].ToString().Should().Be("FIRE_DEPARTMENT");
        query["entity_subtype"].ToString().Should().Be("LOCAL");
        query.Should().NotContainKey("entity_class_enum", "raw enum properties are excluded");
        query.Should().NotContainKey("entity_subtype_enum");
    }
}
