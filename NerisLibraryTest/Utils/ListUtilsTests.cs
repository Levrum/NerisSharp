using FluentAssertions;
using NerisLibrary.Utils;

namespace NerisLibraryTest.Utils;

public class ListUtilsTests
{
    [Fact]
    public void AddIfNotNull_NonNullValue_AddsToList()
    {
        var list = new List<string>();
        list.AddIfNotNull("value");
        list.Should().Equal("value");
    }

    [Fact]
    public void AddIfNotNull_NullValue_DoesNotAdd()
    {
        var list = new List<string>();
        list.AddIfNotNull(null!);
        list.Should().BeEmpty();
    }
}
