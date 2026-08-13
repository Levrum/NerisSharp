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

    [Fact]
    public void CreatePasswordConfig_StoresCredentialsAndType()
    {
        Config config = Config.CreatePasswordConfig("my-user", "my-password", UrlType.Test);

        config.CredentialType.Should().Be(CredentialType.Password);
        config.UserName.Should().Be("my-user");
        config.Password.Should().Be("my-password");
        config.ClientId.Should().BeEmpty();
        config.ClientSecret.Should().BeEmpty();
    }

    [Fact]
    public void CreatePasswordConfig_Default_UsesLiveUrl()
    {
        Config config = Config.CreatePasswordConfig("my-user", "my-password");
        config.Url.Should().Be(new Uri("https://api.neris.fsri.org/v1/"));
    }

    [Fact]
    public void CreatePasswordConfig_TestUrlType_UsesTestUrl()
    {
        Config config = Config.CreatePasswordConfig("my-user", "my-password", UrlType.Test);
        config.Url.Should().Be(new Uri("https://api-test.neris.fsri.org/v1/"));
    }
}
