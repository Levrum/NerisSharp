using NerisLibrary.Models.ElementModels;
using NerisLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NerisLibrary.Utils;

namespace NerisLibrary
{
    public partial class NerisBase
    {
        public async Task<string> PostUnit(EntityModel baseEntity, StationModel baseStation, UnitModel newUnit)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            if (baseStation == null) throw new ArgumentNullException(nameof(baseStation));

            return await PostUnit(baseEntity.Neris_Id, baseStation.Neris_Id, newUnit);
        }

        public async Task<string> PostUnit(string baseEntityId, string baseStationId, UnitModel newUnit)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (string.IsNullOrWhiteSpace(baseStationId)) throw new ArgumentNullException(nameof(baseStationId));
            if (newUnit == null) throw new ArgumentNullException(nameof(newUnit));

            if (!await LoginIfTokenExpired())
            {
                throw new AuthorizationException();
            }

            string postURI = GetUnitRoute(baseEntityId, baseStationId);

            using (var message = new HttpRequestMessage(HttpMethod.Post, postURI))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);

                string content = SerializationExtensions.SerializeLowerCase(newUnit);
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);

                return await ParseIdFromCreatedResult(response);
            }
        }

        private string GetUnitRoute(string entityId, string stationId, string unitId = null)
        {
            string baseUri = GetStationRoute(entityId, stationId);
            string withUnit = UriUtils.AppendPath(baseUri, "unit");
            if (!string.IsNullOrWhiteSpace(unitId))
            {
                withUnit = UriUtils.AppendPath(withUnit, unitId);
            }
            return withUnit;
        }
    }


}
