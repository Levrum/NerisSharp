using NerisLibrary.Models.ElementModels;
using NerisLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NerisLibrary.Utils;
using System.Text.Json.Nodes;

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

            await LoginIfTokenExpired();

            string postURI = GetUnitRoute(baseEntityId, baseStationId);

            using (var message = new HttpRequestMessage(HttpMethod.Post, postURI))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);

                string content = SerializationExtensions.SerializeLowerCase(newUnit);
                message.Content = _CreateJsonContent(content);
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);

                return await ParseIdFromCreatedResult(response);
            }
        }

        public async Task<string> PatchUnit(EntityModel baseEntity, StationModel baseStation, UnitModel unitToUpdate, HashSet<string> fieldsToNull = null)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            if (baseStation == null) throw new ArgumentNullException(nameof(baseStation));
            return await PatchUnit(baseEntity.Neris_Id, baseStation.Neris_Id, unitToUpdate, fieldsToNull);
        }

        public async Task<string> PatchUnit(string baseEntityId, string baseStationId, UnitModel unitToUpdate, HashSet<string> fieldsToNull = null)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (string.IsNullOrWhiteSpace(baseStationId)) throw new ArgumentNullException(nameof(baseStationId));
            if (unitToUpdate == null) throw new ArgumentNullException(nameof(unitToUpdate));
            if (string.IsNullOrWhiteSpace(unitToUpdate.Neris_Id)) throw new ArgumentException("Unit to Update must contain Neris ID");

            string unitRoute = GetUnitRoute(baseEntityId, baseStationId, unitToUpdate.Neris_Id);

            using (var message = new HttpRequestMessage(HttpMethod.Patch, unitRoute))
            {
                JsonObject contentObj = SerializationExtensions.SerializeToJsonObjectLowerCase(unitToUpdate);
                contentObj.Remove("neris_id");
                if (fieldsToNull != null)
                {
                    foreach (string field in fieldsToNull)
                    {
                        contentObj[field.ToLower()] = null;
                    }
                }
                message.Content = _CreateJsonContent(contentObj.ToJsonString());
                HttpResponseMessage response = await _call(message);

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
