using FluentAssertions;
using NerisSharp.Models;

namespace NerisSharpTest.Models;

public class ConfigTests
{
    [Fact]
    public void CreateClientCredentialConfig_Default_UsesLiveUrl()
    {
        Config config = Config.CreateClientCredentialConfig("id", "secret");
        config.Url.Should().Be(new Uri("https://api.neris.fsri.org/v1/"));
    }

    [Fact]
    public void CreateClientCredentialConfig_TestUrlType_UsesTestUrl()
    {
        Config config = Config.CreateClientCredentialConfig("id", "secret", UrlType.Test);
        config.Url.Should().Be(new Uri("https://api-test.neris.fsri.org/v1/"));
    }

    [Fact]
    public void CreateClientCredentialConfig_StoresCredentialsAndType()
    {
        Config config = Config.CreateClientCredentialConfig("my-id", "my-secret", UrlType.Test);

        config.CredentialType.Should().Be(CredentialType.ClientCredentials);
        config.ClientId.Should().Be("my-id");
        config.ClientSecret.Should().Be("my-secret");
    }
}
