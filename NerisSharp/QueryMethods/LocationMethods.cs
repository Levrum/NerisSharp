using NerisSharp.Models;
using NerisSharp.Models.ElementModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NerisSharp
{
    public partial class NerisBase
    {
        public async Task<LatLong> GetLocation(StationModel station)
        {
            if (station == null || String.IsNullOrWhiteSpace(station.Location))
            {
                throw new ArgumentException("Station and Station Location must not be null");
            }
            return await GetLocation(station.Location);
        }
        public async Task<LatLong> GetLocation(EntityModel entity)
        {
            if (entity == null || String.IsNullOrWhiteSpace(entity.Location))
            {
                throw new ArgumentException("Station and Station Location must not be null");
            }
            return await GetLocation(entity.Location);
        }

        public async Task<LatLong> GetLocation(string locationURI)
        {
            string uri = string.Format("{0}?f=json", locationURI);
            JsonNode? json = null;
            LatLong toReturn = new LatLong();
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, locationURI))
            {
                HttpResponseMessage response = await _call(request);
                json = await response.Content.ReadFromJsonAsync<JsonNode>();
            }
            if (json != null)
            {
                JsonNode geometryNode = json["geometry"];
                JsonNode coords = geometryNode["coordinates"];
                if (coords.GetValueKind() == JsonValueKind.Array)
                {
                    JsonArray coordArray = coords.AsArray();
                    toReturn.Longitude = coordArray[0].GetValue<double>();
                    toReturn.Latitude = coordArray[1].GetValue<double>();
                } else
                {
                    throw new JsonException("Could not access coordinates");
                }
            } else
            {
                throw new JsonException("Could not process json response");
            }
            return toReturn;
        }
    }
}
