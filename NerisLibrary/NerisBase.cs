using Microsoft.Extensions.Logging;
using NerisLibrary.Models;
using NerisLibrary.Models.ElementModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NerisLibrary
{
    public class NerisBase //NerisAPIBase?
    {

        //will hold methods for interacting with Neris API
        //Also needs to login with user
        private Config _config { get; set; }
        private ILogger<NerisBase>? _logger { get; set; }
        private HttpClient _httpClient { get; set; }
        private AccessTokenModel? _accessToken { get; set; }
        public bool Initialized { get {  return _accessToken != null && _accessToken.access_token != string.Empty; } } 
        public NerisBase(Config config, HttpClient client, ILogger<NerisBase> logger = null)
        {
            _config = config;
            if (logger != null)
            {
                _logger = logger;
            }
            _httpClient = client;
        }

        public async Task Login() //i think we want this to throw an error if it fails.
        {
            //submit login information to token endpoint, get tokens, store
            try
            {
                switch (_config.CredentialType)
                {
                    case CredentialType.ClientCredentials:
                        {
                            await LoginClientCredentials();
                            break;
                        }
                    case CredentialType.Password:
                        {
                            throw new NotImplementedException();
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Error Logging In");
                throw ex;
            }
        }

        private async Task LoginClientCredentials()
        {
            string credentials = $"{_config.ClientId.Trim()}:{_config.ClientSecret.Trim()}";
            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            var formContent = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("generate_refresh_token", "true")
            };
            AccessTokenModel? tokenModel = null;
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, GetRoute(RouteTypes.Token)))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
                message.Headers.UserAgent.ParseAdd(".NET/3.1");
                message.Content = new FormUrlEncodedContent(formContent);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();
                tokenModel = await response.Content.ReadFromJsonAsync<AccessTokenModel>();
            }

            _accessToken = tokenModel;
        }

        private enum RouteTypes
        {
            Token,
        }
        private string GetRoute(RouteTypes type)
        {
            string routeAppend = "";
            switch (type)
            {
                case RouteTypes.Token:
                    routeAppend = "token";
                    break;
                default:
                    break;
            }
            return new Uri(_config.Url, routeAppend).ToString();
        }

        private void LogMessage(string message, LogLevel severity=LogLevel.Information)
        {
            if (_logger != null)
            {
                _logger.Log(severity, message);
            }else
            {
                Console.WriteLine(message);
            }
        }

        private void LogError(Exception ex, string message)
        {
            if (_logger != null)
            {
                _logger.LogError(ex, message);
            }else 
            { 
                Console.WriteLine(ex.Message); 
            }
        }

    }
}
