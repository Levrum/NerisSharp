using FluentAssertions;
using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Utils;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NerisLibraryTest.Utils;

public class SerializationExtensionsTests
{
    private class SampleDto
    {
        public string? Some_Name { get; set; }
        public string? Not_Set { get; set; }
        public int Count_Value { get; set; }
    }

    private class SampleEnumClass
    {
        public AidTypeEnum Aid_Type { get; set; }
        public List<AidTypeEnum> Aid_Type_List { get; set; }
    }

    [Fact]
    public void SerializeLowerCase_SnakeCasesPropertyNamesAndOmitsNulls()
    {
        string json = SerializationExtensions.SerializeLowerCase(new SampleDto { Some_Name = "abc", Count_Value = 5 });

        using JsonDocument doc = JsonDocument.Parse(json);
        doc.RootElement.GetProperty("some_name").GetString().Should().Be("abc");
        doc.RootElement.GetProperty("count_value").GetInt32().Should().Be(5);
        doc.RootElement.TryGetProperty("not_set", out _).Should().BeFalse("null properties must be omitted");
    }

    [Fact]
    public void SerializeToJsonObjectLowerCase_ReturnsMutableObjectWithSnakeCaseKeys()
    {
        JsonObject obj = SerializationExtensions.SerializeToJsonObjectLowerCase(new SampleDto { Some_Name = "abc" });

        obj.ContainsKey("some_name").Should().BeTrue();
        obj.Remove("some_name").Should().BeTrue();
        obj.ContainsKey("some_name").Should().BeFalse();
    }

    [Fact]
    public async Task DeserializeCaseInsensitive_MixedCaseJson_MapsProperties()
    {
        var content = new StringContent("{\"SOME_name\":\"abc\",\"count_VALUE\":7}", Encoding.UTF8, "application/json");

        SampleDto? dto = await content.DeserializeCaseInsensitive<SampleDto>();

        dto!.Some_Name.Should().Be("abc");
        dto.Count_Value.Should().Be(7);
    }

    [Fact]
    public async Task SerializePolicies_SerializeEnums_AsStrings()
    {
        var sampleEnum = new SampleEnumClass()
        {
            Aid_Type = AidTypeEnum.SUPPORT_AID,
            Aid_Type_List = new List<AidTypeEnum>() { AidTypeEnum.IN_LIEU_AID, AidTypeEnum.ACTING_AS_AID }
        };

        string json = SerializationExtensions.SerializeLowerCase(sampleEnum);
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            doc.RootElement.GetProperty("aid_type").GetString().Should().Be("SUPPORT_AID");
            doc.RootElement.GetProperty("aid_type_list")[0].GetString().Should().Be("IN_LIEU_AID");
            doc.RootElement.GetProperty("aid_type_list")[1].GetString().Should().Be("ACTING_AS_AID");
        }

        JsonObject obj = SerializationExtensions.SerializeToJsonObjectLowerCase(sampleEnum);
        obj["aid_type"].GetValue<string>().Should().Be("SUPPORT_AID");
        obj["aid_type_list"][0].GetValue<string>().Should().Be("IN_LIEU_AID");
        obj["aid_type_list"][1].GetValue<string>().Should().Be("ACTING_AS_AID");



        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        SampleEnumClass newSampleEnum = await content.DeserializeCaseInsensitive<SampleEnumClass>();
        newSampleEnum.Aid_Type.Should().Be(AidTypeEnum.SUPPORT_AID);
        newSampleEnum.Aid_Type_List.Should().Equal(AidTypeEnum.IN_LIEU_AID, AidTypeEnum.ACTING_AS_AID);


    }
}
