using FluentAssertions;
using NerisSharp;
using NerisSharp.Exceptions;
using NerisSharp.Models;
using NerisSharpTest.TestHelpers;

namespace NerisSharpTest.QueryMethods;

/// <summary>
/// Covers the username/password grant, the MFA challenge round trip, and the
/// refresh-token path added alongside them.
/// </summary>
public class NerisBasePasswordAuthTests
{
    /// <summary>Drives a fixture through password login + challenge answer so a usable token is stored.</summary>
    private static async Task<NerisBase> LoggedInViaChallenge(
        NerisBaseFixture fixture,
        int expiresInSeconds = 3600,
        string refreshToken = NerisBaseFixture.RefreshToken)
    {
        fixture.QueueChallengeResponse();
        fixture.QueueTokenResponse(expiresInSeconds, refreshToken: refreshToken);
        NerisBase neris = fixture.CreatePasswordNerisBase();

        await neris.Login();
        await neris.LoginChallenge("123456");
        return neris;
    }

    [Fact]
    public async Task Login_PasswordConfig_PostsPasswordGrantWithoutBasicAuthHeader()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueChallengeResponse();
        NerisBase neris = fixture.CreatePasswordNerisBase();

        await neris.Login();

        RecordedRequest tokenRequest = fixture.Handler.Requests.Should().ContainSingle().Subject;
        tokenRequest.Method.Should().Be(HttpMethod.Post);
        tokenRequest.Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/token/");
        tokenRequest.AuthScheme.Should().BeNull("the password grant authenticates via the body, not a Basic header");
        tokenRequest.Body.Should().Contain("grant_type=password")
            .And.Contain($"username={NerisBaseFixture.UserName}")
            .And.Contain($"password={NerisBaseFixture.Password}")
            .And.Contain("generate_refresh_token=true");
    }

    [Fact]
    public async Task Login_PasswordConfigReturnsChallenge_SetsRequiresChallengeResponseAndStaysUninitialized()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueChallengeResponse();
        NerisBase neris = fixture.CreatePasswordNerisBase();

        await neris.Login();

        neris.RequiresChallengeResponse.Should().BeTrue();
        neris.Initialized.Should().BeFalse();
    }

    [Fact]
    public async Task Login_PasswordConfigReturns200Token_SkipsChallengeAndStoresToken()
    {
        var fixture = new NerisBaseFixture();
        // MFA not required: the token endpoint answers 200 with a token rather than 202 with a challenge.
        fixture.QueueTokenResponse();
        fixture.Handler.QueueJsonResponse("{}");
        NerisBase neris = fixture.CreatePasswordNerisBase();

        await neris.Login();

        neris.RequiresChallengeResponse.Should().BeFalse("a 200 token response is not a challenge");
        neris.Initialized.Should().BeTrue();

        await neris.GetEntity("ENT1");

        fixture.Handler.Requests[1].AuthScheme.Should().Be("Bearer");
        fixture.Handler.Requests[1].AuthParameter.Should().Be(NerisBaseFixture.AccessToken);
    }

    [Fact]
    public async Task Call_PasswordTokenNearExpiryWithoutRefreshToken_ReLoginsWithPasswordGrant()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueTokenResponse(expiresInSeconds: 60, refreshToken: "");
        fixture.QueueTokenResponse(expiresInSeconds: 3600, accessToken: "second-access-token");
        fixture.Handler.QueueJsonResponse("{}");
        NerisBase neris = fixture.CreatePasswordNerisBase();
        await neris.Login();

        await neris.GetEntity("ENT1");

        fixture.Handler.Requests[1].Body.Should().Contain("grant_type=password")
            .And.NotContain("grant_type=refresh_token", "there is no refresh token to send");
        fixture.Handler.Requests[2].AuthParameter.Should().Be("second-access-token");
    }

    [Fact]
    public async Task Call_WithChallengePending_ThrowsMFARequiredAndSendsNoRequest()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueChallengeResponse();
        NerisBase neris = fixture.CreatePasswordNerisBase();
        await neris.Login();
        fixture.Handler.Requests.Clear();

        Func<Task> act = () => neris.GetEntity("ENT1");

        await act.Should().ThrowAsync<MFARequiredException>();
        fixture.Handler.Requests.Should().BeEmpty("a hanging challenge must short-circuit before any HTTP call");
    }

    [Fact]
    public async Task FirstCall_PasswordConfig_TriggersLoginThenThrowsMFARequired()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueChallengeResponse();
        NerisBase neris = fixture.CreatePasswordNerisBase();

        Func<Task> act = () => neris.GetEntity("ENT1");

        await act.Should().ThrowAsync<MFARequiredException>();
        fixture.Handler.Requests.Should().ContainSingle()
            .Which.Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/token/");
    }

    [Fact]
    public async Task LoginChallenge_PostsChallengeGrantWithSessionAndCode()
    {
        var fixture = new NerisBaseFixture();
        fixture.QueueChallengeResponse("SMS_MFA", "session-abc");
        fixture.QueueTokenResponse();
        NerisBase neris = fixture.CreatePasswordNerisBase();
        await neris.Login();

        await neris.LoginChallenge("654321");

        RecordedRequest challengeRequest = fixture.Handler.Requests[1];
        challengeRequest.Method.Should().Be(HttpMethod.Post);
        challengeRequest.Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/token/");
        challengeRequest.Body.Should().Contain("grant_type=SMS_MFA")
            .And.Contain($"username={NerisBaseFixture.UserName}")
            .And.Contain("session=session-abc")
            .And.Contain("SMS_MFA=654321")
            .And.Contain("generate_refresh_token=true");
    }

    [Fact]
    public async Task LoginChallenge_Success_ClearsChallengeAndStoresToken()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = await LoggedInViaChallenge(fixture);

        neris.RequiresChallengeResponse.Should().BeFalse();
        neris.Initialized.Should().BeTrue();

        fixture.Handler.QueueJsonResponse("{}");
        await neris.GetEntity("ENT1");

        RecordedRequest entityRequest = fixture.Handler.Requests[2];
        entityRequest.AuthScheme.Should().Be("Bearer");
        entityRequest.AuthParameter.Should().Be(NerisBaseFixture.AccessToken);
    }

    [Fact]
    public async Task LoginChallenge_NoPendingChallenge_ThrowsAuthorizationException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreatePasswordNerisBase();

        Func<Task> act = () => neris.LoginChallenge("123456");

        await act.Should().ThrowAsync<AuthorizationException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task LoginChallenge_ClientCredentialConfig_ThrowsAuthorizationException()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = fixture.CreateNerisBase();

        Func<Task> act = () => neris.LoginChallenge("123456");

        await act.Should().ThrowAsync<AuthorizationException>();
        fixture.Handler.Requests.Should().BeEmpty();
    }

    [Fact]
    public async Task Call_PasswordTokenNearExpiry_RefreshesWithRefreshTokenGrant()
    {
        var fixture = new NerisBaseFixture();
        // 60s of life left is inside the 5-minute renewal window.
        NerisBase neris = await LoggedInViaChallenge(fixture, expiresInSeconds: 60);
        fixture.QueueTokenResponse(accessToken: "refreshed-access-token", refreshToken: "next-refresh");
        fixture.Handler.QueueJsonResponse("{}");

        await neris.GetEntity("ENT1");

        RecordedRequest refreshRequest = fixture.Handler.Requests[2];
        refreshRequest.Uri.ToString().Should().Be($"{NerisBaseFixture.BaseUrl}/token/");
        refreshRequest.Body.Should().Contain("grant_type=refresh_token")
            .And.Contain($"refresh_token={NerisBaseFixture.RefreshToken}")
            .And.NotContain("grant_type=password");
        fixture.Handler.Requests[3].AuthParameter.Should().Be("refreshed-access-token",
            "the refreshed token must replace the old one");
    }

    [Fact]
    public async Task Call_PasswordTokenWellWithinLifetime_DoesNotRefresh()
    {
        var fixture = new NerisBaseFixture();
        NerisBase neris = await LoggedInViaChallenge(fixture, expiresInSeconds: 3600);
        fixture.Handler.QueueJsonResponse("{}");

        await neris.GetEntity("ENT1");

        fixture.Handler.Requests.Should().HaveCount(3, "no extra token call should be made");
        fixture.Handler.Requests[2].Uri.ToString().Should().NotContain("token");
    }

    [Fact]
    public async Task Call_ClientCredentialTokenNearExpiry_ReLoginsInsteadOfRefreshing()
    {
        var fixture = new NerisBaseFixture();
        // Client credentials never uses the refresh grant, even when a refresh token was returned.
        fixture.QueueTokenResponse(expiresInSeconds: 60);
        fixture.Handler.QueueJsonResponse("{}");
        fixture.QueueTokenResponse(expiresInSeconds: 3600, accessToken: "second-access-token");
        fixture.Handler.QueueJsonResponse("{}");
        NerisBase neris = fixture.CreateNerisBase();

        await neris.GetEntity("ENT1");
        await neris.GetEntity("ENT2");

        fixture.Handler.Requests.Should().HaveCount(4);
        fixture.Handler.Requests[2].Body.Should().Contain("grant_type=client_credentials");
        fixture.Handler.Requests[3].AuthParameter.Should().Be("second-access-token");
    }
}
