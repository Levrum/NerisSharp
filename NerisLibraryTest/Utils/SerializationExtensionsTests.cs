using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using NerisLibrary.Utils;

namespace NerisLibraryTest.Utils;

public class SerializationExtensionsTests
{
    private class SampleDto
    {
        public string? Some_Name { get; set; }
        public string? Not_Set { get; set; }
        public int Count_Value { get; set; }
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
}
