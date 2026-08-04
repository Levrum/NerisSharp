using FluentAssertions;
using NerisSharp.Utils;

namespace NerisSharpTest.Utils;

public class UriUtilsTests
{
    [Theory]
    [InlineData("https://host/v1", "path")]
    [InlineData("https://host/v1/", "path")]
    [InlineData("https://host/v1", "/path")]
    [InlineData("https://host/v1/", "/path")]
    [InlineData("https://host/v1/", "/path/")]
    [InlineData("https://host/v1", "path/")]
    public void AppendPath_AnySlashCombination_JoinsWithSingleSlashAndNoTrailingSlash(string basePath, string suffix)
    {
        UriUtils.AppendPath(basePath, suffix).Should().Be("https://host/v1/path");
    }

    [Fact]
    public void AppendPath_MultipleSegmentsChained_BuildsNestedPath()
    {
        string result = UriUtils.AppendPath(UriUtils.AppendPath("https://host/v1/entity/", "ENT1"), "station");
        result.Should().Be("https://host/v1/entity/ENT1/station");
    }
}
