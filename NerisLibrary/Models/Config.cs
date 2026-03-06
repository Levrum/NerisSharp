using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace NerisLibrary.Models
{
    public sealed class Config
    {
        public Uri Url { get; set; } = null;
        public CredentialType CredentialType { get; set; } = CredentialType.Password;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";

        private Config(UrlType BaseUrl) 
        {
            Url = (BaseUrl == UrlType.Live) ? new Uri("https://api.neris.fsri.org/v1/") : new Uri("https://api-test.neris.fsri.org/v1/");
        }

        public static Config CreatePasswordConfig(string username, string password, UrlType BaseUrl = UrlType.Live)
        {
            var config = new Config(BaseUrl) { CredentialType = CredentialType.Password, UserName = username, Password = password };
            return config;
        }

        public static Config CreateClientCredentialConfig(string clientId, string clientSecret, UrlType BaseUrl = UrlType.Live)
        {
            Config config = new Config(BaseUrl) { CredentialType = CredentialType.ClientCredentials, ClientId = clientId, ClientSecret = clientSecret };
            return config;
        }

    }
    public enum CredentialType
    {
        Password = 0,
        ClientCredentials = 1
    }

    public enum UrlType
    {
        Live = 0,
        Test = 1,
    }
}
