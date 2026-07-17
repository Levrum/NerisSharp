using System.Text;
using FluentAssertions;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Utils;

namespace NerisLibraryTest.Models;

public class AccessTokenModelTests
{
    [Fact]
    public void ExpiresIn_PositiveSeconds_SetsExpiresAtInTheFuture()
    {
        var model = new AccessTokenModel { Expires_In = 3600 };
        model.expires_at.Should().BeCloseTo(DateTime.UtcNow.AddSeconds(3600), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ExpiresIn_NegativeSeconds_SetsExpiresAtInThePast()
    {
        var model = new AccessTokenModel { Expires_In = -10 };
        model.expires_at.Should().BeBefore(DateTime.UtcNow);
    }

    [Fact]
    public async Task DeserializeCaseInsensitive_TokenJson_MapsSnakeCaseFieldsAndComputesExpiry()
    {
        var content = new StringContent(
            "{\"access_token\":\"tok\",\"refresh_token\":\"ref\",\"expires_in\":1800}",
            Encoding.UTF8, "application/json");

        AccessTokenModel? model = await content.DeserializeCaseInsensitive<AccessTokenModel>();

        model!.Access_Token.Should().Be("tok");
        model.Refresh_Token.Should().Be("ref");
        model.expires_at.Should().BeCloseTo(DateTime.UtcNow.AddSeconds(1800), TimeSpan.FromSeconds(5));
    } //unneeded, this is tested in utils. 
}
