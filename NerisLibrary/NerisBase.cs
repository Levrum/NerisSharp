using Microsoft.Extensions.Logging;
using NerisLibrary.Models;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.RequestModels;
using NerisLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NerisLibrary
{
    public partial class NerisBase //NerisAPIBase?
    {

        //will hold methods for interacting with Neris API
        //Also needs to login with user
        private Config _config { get; set; }
        private ILogger<NerisBase>? _logger { get; set; } = null;
        private HttpClient _httpClient { get; set; }
        private AccessTokenModel? _accessToken { get; set; }
        public bool Initialized { get {  return _accessToken != null && _accessToken.Access_Token != string.Empty; } } 
        public NerisBase(Config config, HttpClient client, ILogger<NerisBase> logger = null)
        {
            _config = config;
            _logger = logger;
            _httpClient = client;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(".NET/3.1");
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

        private async Task LoginIfTokenExpired()
        {
            if (!Initialized || _accessToken.expires_at <= DateTime.UtcNow)
            {
                await Login();
            }
            if (!Initialized)
            {
                throw new AuthorizationException();
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
                message.Content = new FormUrlEncodedContent(formContent);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);
                tokenModel = await response.Content.DeserializeCaseInsensitive<AccessTokenModel>();
            }

            _accessToken = tokenModel;
        }

        private enum RouteTypes
        {
            Token,
            Entity,
        }
        private string GetRoute(RouteTypes type)
        {
            string routeAppend = "/";
            switch (type)
            {
                case RouteTypes.Token:
                    routeAppend = "token/";
                    break;
                case RouteTypes.Entity:
                    routeAppend = "entity/";
                    break;
                default:
                    break;
            }
            return new Uri(_config.Url, routeAppend).ToString();
        }

        private async Task<string> ParseIdFromCreatedResult(HttpResponseMessage response)
        {
            string reponseContent = await response.Content.ReadAsStringAsync();
            JsonDocument jsonDocument = JsonDocument.Parse(reponseContent);
            bool success = jsonDocument.RootElement.TryGetProperty("neris_id", out JsonElement nerisIdJson);
            if (success)
            {
                return nerisIdJson.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task CheckStatusCodeAndHandleError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string responseMessage = await response.Content.ReadAsStringAsync();
                LogMessage(responseMessage, Microsoft.Extensions.Logging.LogLevel.Error);
                throw new HttpRequestException(responseMessage);
            }
        }

        private void LogMessage(string message, LogLevel severity = LogLevel.Information)
        {
            if (_logger != null)
            {
                _logger.Log(severity, message);
            } else
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
