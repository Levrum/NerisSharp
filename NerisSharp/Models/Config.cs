using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace NerisSharp.Models
{
    /// <summary>
    /// Class for holding NERIS Connection settings and properties including type of connection, credentials, and endpoint.
    /// </summary>
    public sealed class Config
    {
        public Uri Url { get; set; } = null;
        public CredentialType CredentialType { get; private set; } = CredentialType.Password;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";

        private Config(UrlType BaseUrl) //private Constructor as we want to limit creation depending on credential type used. 
        {
            Url = (BaseUrl == UrlType.Live) ? new Uri("https://api.neris.fsri.org/v1/") : new Uri("https://api-test.neris.fsri.org/v1/");
        }

        // Username/PW Auth is not implemented.
        //public static Config CreatePasswordConfig(string username, string password, UrlType BaseUrl = UrlType.Live)
        //{
        //    var config = new Config(BaseUrl) { CredentialType = CredentialType.Password, UserName = username, Password = password };
        //    return config;
        //}

        /// <summary>
        /// Create a Config option for use with Client Id and Secret Authentication.
        /// </summary>
        /// <param name="clientId">Public Client Id to use for integration authentication</param>
        /// <param name="clientSecret">Client Secret to use for integration authentication</param>
        /// <param name="BaseUrl">Which NERIS endpoint to connect to, options are live or test, default is live.</param>
        /// <returns></returns>
        public static Config CreateClientCredentialConfig(string clientId, string clientSecret, UrlType BaseUrl = UrlType.Live)
        {
            Config config = new Config(BaseUrl) { CredentialType = CredentialType.ClientCredentials, ClientId = clientId, ClientSecret = clientSecret };
            return config;
        }

    }

    /// <summary>
    /// What kind of credentials to use for connecting to the NERIS API.
    /// </summary>
    public enum CredentialType
    {
        Password = 0,
        ClientCredentials = 1
    }

    /// <summary>
    /// What Url or Environment to target for connecting to the NERIS API.
    /// </summary>
    public enum UrlType
    {
        Live = 0,
        Test = 1,
    }
}
