using Microsoft.Extensions.Logging;
using Moq;
using NerisSharp;
using NerisSharp.Models;

namespace NerisSharpTest.TestHelpers;

/// <summary>
/// Builds a NerisBase wired to a FakeHttpMessageHandler with a Test-environment config.
/// Call QueueTokenResponse() before the first NerisBase call so the auto-login succeeds.
/// </summary>
public sealed class NerisBaseFixture
{
    public const string ClientId = "test-client-id";
    public const string ClientSecret = "test-client-secret";
    public const string BaseUrl = "https://api-test.neris.fsri.org/v1";
    public const string AccessToken = "fake-access-token";

    public FakeHttpMessageHandler Handler { get; } = new();
    public Mock<ILogger<NerisBase>> Logger { get; } = new();

    public NerisBase CreateNerisBase(bool denyWriteActions = false, bool withLogger = false)
    {
        Config config = Config.CreateClientCredentialConfig(ClientId, ClientSecret, UrlType.Test);
        return CreateNerisBase(config, denyWriteActions, withLogger);
    }

    public NerisBase CreateNerisBase(Config config, bool denyWriteActions = false, bool withLogger = false)
    {
        var client = new HttpClient(Handler);
        return new NerisBase(config, client, withLogger ? Logger.Object : null!, denyWriteActions);
    }

    public void QueueTokenResponse(int expiresInSeconds = 3600, string accessToken = AccessToken)
    {
        Handler.QueueJsonResponse(
            $"{{\"access_token\":\"{accessToken}\",\"refresh_token\":\"fake-refresh\",\"expires_in\":{expiresInSeconds}}}");
    }
}
