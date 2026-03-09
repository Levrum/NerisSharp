using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.RequestModels;
using NerisLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NerisLibrary //must use top level namespace for partial class to access all methods.
{
    public partial class NerisBase
    {
        public async Task<EntityPageSet> GetEntities(EntityRequestModel requestModel)
        {
            //check if request is valid
            if (!requestModel.Validate())
            {
                throw new Exception("Error"); //TODO add exception types;
            }
            //check if logged in
            if (!await LoginIfTokenExpired())
            {
                //failed to login:
                throw new Exception("Not Logged In"); //TODO Add exception types
            }

            //build request uri
            string baseUri = GetRoute(RouteTypes.Entity);
            string uri = requestModel.CreateQueryURI(baseUri);
            EntityPageSet entities = null;
            //make request
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);

                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();
                entities = await response.Content.DeserializeCaseInsensitive<EntityPageSet>();

            }
            //handle results
            return entities;

        }

        public async Task<string> PostStation(EntityModel BaseEntity, StationModel NewStation)
        {
            if (BaseEntity == null)
            {
                throw new ArgumentNullException(nameof(BaseEntity));
            }
            return await PostStation(BaseEntity.Neris_Id, NewStation);
        }


        private async Task<string> PostStation(string BaseEntityId, StationModel NewStation)
        {
            string stationUri = GetStationRoute(BaseEntityId);
            if (!await LoginIfTokenExpired())
            {
                //failed to login:
                throw new Exception("Not Logged In"); //TODO Add exception types
            }
            string nerisId = string.Empty;
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, stationUri))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);
                //string JsonContent
                string content = JsonSerializer.Serialize(NewStation);
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                response.EnsureSuccessStatusCode();

                //parse new ID out
            }
            return nerisId;
        }

        public string GetStationRoute(string BaseEntityId)
        {
            string baseUri = GetRoute(RouteTypes.Entity);
            if (baseUri.Last() != '/')
            {
                baseUri += '/';
            }
            baseUri += BaseEntityId + "/" + "station";
            return baseUri;
        }
    }
}
