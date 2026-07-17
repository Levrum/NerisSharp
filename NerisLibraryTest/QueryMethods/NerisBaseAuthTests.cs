using System.Text;
using FluentAssertions;
using NerisLibrary;
using NerisLibrary.Exceptions;
using NerisLibrary.Models;
using NerisLibraryTest.TestHelpers;

namespace NerisLibraryTest.QueryMethods;

public class NerisBaseAuthTests
{
    [Fact]
    public async Task FirstCall_SendsTokenRequestWithBasicAuthAndClientCredentialsGrant()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{}");
        NerisBase neris = fixture.CreateNerisBase();

        await neris.GetEntity("ENT1");

        RecordedRequest tokenRequest = fixture.Handler.Requests[0];
        tokenRequest.Method.Should().Be(HttpMethod.Post);
        tokenRequest.Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/token/");
        tokenRequest.AuthScheme.Should().Be("Basic");
        string expectedCredentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{NerisBaseFixture.ClientId}:{NerisBaseFixture.ClientSecret}"));
        tokenRequest.AuthParameter.Should().Be(expectedCredentials);
        tokenRequest.Body.Should().Contain("grant_type=client_credentials")
            .And.Contain("generate_refresh_token=true");
    }

    [Fact]
    public async Task Login_CredentialsWithSurroundingWhitespace_AreTrimmed()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{}");
        Config config = Config.CreateClientCredentialConfig("  my-id  ", "  my-secret  ", UrlType.Test);
        NerisBase neris = fixture.CreateNerisBase(config);

        await neris.GetEntity("ENT1");

        string expected = Convert.ToBase64String(Encoding.UTF8.GetBytes("my-id:my-secret"));
        fixture.Handler.Requests[0].AuthParameter.Should().Be(expected);
    }

    [Fact]
    public async Task SecondCall_TokenStillValid_DoesNotLoginAgain()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse(expiresInSeconds: 3600);
        fixture.Handler.QueueJsonResponse("{}");
        fixture.Handler.QueueJsonResponse("{}");
        NerisBase neris = fixture.CreateNerisBase();

        await neris.GetEntity("ENT1");
        await neris.GetEntity("ENT2");

        fixture.Handler.Requests.Should().HaveCount(3);
        fixture.Handler.Requests.Skip(1).Should().OnlyContain(
            r => !r.Uri.ToString().Contains("token"), "a valid token must be reused");
    }

    [Fact]
    public async Task NextCall_TokenExpired_LogsInAgain()
    {
        var fixture = new NerisBaseFixture();
        // First login returns an already-expired token. The first call still proceeds
        // (Initialized only checks the token exists); the SECOND call must re-login.
        fixture.QueueTokenResponse(expiresInSeconds: -10);
        fixture.Handler.QueueJsonResponse("{}");
        fixture.QueueTokenResponse(expiresInSeconds: 3600);
        fixture.Handler.QueueJsonResponse("{}");
        NerisBase neris = fixture.CreateNerisBase();

        await neris.GetEntity("ENT1");
        await neris.GetEntity("ENT2");

        fixture.Handler.Requests.Should().HaveCount(4);
        fixture.Handler.Requests[2].Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/token/");
    }

    [Fact]
    public async Task Call_LoginReturnsEmptyToken_ThrowsAuthorizationException()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse(accessToken: "");
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.GetEntity("ENT1");

        await act.Should().ThrowAsync<AuthorizationException>();
        fixture.Handler.Requests.Should().HaveCount(1, "only the token request should have been sent");
    }

    [Fact]
    public async Task Login_PasswordCredentialType_ThrowsNotImplementedException()
    {
        var fixture = new NerisBaseFixture();
        Config config = Config.CreateClientCredentialConfig("id", "secret", UrlType.Test);
        config.CredentialType = CredentialType.Password; //this exposes some unintentional behavior, I think CredentialType should only be set on creation.
        NerisBase neris = fixture.CreateNerisBase(config);

        Func<Task> act = () => neris.Login();

        await act.Should().ThrowAsync<NotImplementedException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }
}
