using NerisLibrary.Exceptions;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NerisLibrary
{
    public partial class NerisBase
    {
        public async Task<string> PostStation(EntityModel BaseEntity, StationModel NewStation)
        {
            if (BaseEntity == null)
            {
                throw new ArgumentNullException(nameof(BaseEntity));
            }
            return await PostStation(BaseEntity.Neris_Id, NewStation);
        }


        public async Task<string> PostStation(string BaseEntityId, StationModel NewStation)
        {
            string stationUri = GetStationRoute(BaseEntityId);
            string nerisId = string.Empty;
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, stationUri))
            {
                string content = SerializationExtensions.SerializeLowerCase<StationModel>(NewStation);
                message.Content = CreateJsonContent(content);
                HttpResponseMessage response = await _call(message);

                //parse new ID out
                nerisId = await ParseIdFromCreatedResult(response);
            }
            return nerisId;
        }

        /// <summary>
        /// Updates an existing station entity with the specified changes and returns the NERIS identifier of the
        /// updated station.
        /// </summary>
        /// <remarks>Only the fields provided in <paramref name="StationUpdate"/> are updated. Fields
        /// listed in <paramref name="FieldsToNull"/> are explicitly set to null in the update. The method requires a
        /// valid authentication token and will attempt to re-authenticate if the token has expired.</remarks>
        /// <param name="BaseEntity">The Entity of which the station belongs. Cannot be null.</param>
        /// <param name="StationUpdate">An object containing the updated values for the station. Must include a valid NERIS identifier. Cannot be
        /// null.</param>
        /// <param name="FieldsToNull">A set of property names to be explicitly set to null in the update request. If null, no fields are set to
        /// null.</param>
        /// <returns>A string containing the NERIS identifier of the updated station.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="BaseEntityId"/> or <paramref name="StationUpdate"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="StationUpdate"/> does not contain a valid NERIS identifier.</exception>
        /// <exception cref="AuthorizationException">Thrown if the current authentication token is invalid and re-authentication fails.</exception>
        public async Task<string> PatchStation(EntityModel BaseEntity, StationModel StationUpdate, HashSet<string> FieldsToNull = null)
        {
            if (BaseEntity == null)
            {
                throw new ArgumentNullException(nameof(BaseEntity));
            }
            return await PatchStation(BaseEntity.Neris_Id, StationUpdate, FieldsToNull);
        }

        /// <summary>
        /// Updates an existing station entity with the specified changes and returns the NERIS identifier of the
        /// updated station.
        /// </summary>
        /// <remarks>Only the fields provided in <paramref name="StationUpdate"/> are updated. Fields
        /// listed in <paramref name="FieldsToNull"/> are explicitly set to null in the update. The method requires a
        /// valid authentication token and will attempt to re-authenticate if the token has expired.</remarks>
        /// <param name="BaseEntityId">The unique identifier of the base entity to which the station belongs. Cannot be null.</param>
        /// <param name="StationUpdate">An object containing the updated values for the station. Must include a valid NERIS identifier. Cannot be
        /// null.</param>
        /// <param name="FieldsToNull">A set of property names to be explicitly set to null in the update request. If null, no fields are set to
        /// null.</param>
        /// <returns>A string containing the NERIS identifier of the updated station.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="BaseEntityId"/> or <paramref name="StationUpdate"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="StationUpdate"/> does not contain a valid NERIS identifier.</exception>
        /// <exception cref="AuthorizationException">Thrown if the current authentication token is invalid and re-authentication fails.</exception>
        public async Task<string> PatchStation(string BaseEntityId, StationModel StationUpdate, HashSet<string> FieldsToNull = null)
        {
            if (BaseEntityId == null || StationUpdate == null)
            {
                throw new ArgumentNullException();
            }

            if (String.IsNullOrWhiteSpace(StationUpdate.Neris_Id))
            {
                throw new ArgumentException("StationUpdate MUST have NERIS id to patch");
            }

            string fullRoute = GetStationRoute(BaseEntityId, StationUpdate.Neris_Id);
            string nerisId = string.Empty;
            using (var message = new HttpRequestMessage(HttpMethod.Patch, fullRoute))
            {
                JsonObject contentNode = SerializationExtensions.SerializeToJsonObjectLowerCase(StationUpdate);
                contentNode.Remove("neris_id");
                //check for nulls: 
                if (FieldsToNull != null)
                {
                    foreach (string field in FieldsToNull)
                    {
                        contentNode[field.ToLower()] = null;
                    }
                }
                string content = contentNode.ToJsonString();
                message.Content = CreateJsonContent(content);
                HttpResponseMessage response = await _call(message);

                nerisId = await ParseIdFromCreatedResult(response);
            }
            return nerisId;

        }

        public async Task<bool> DeleteStation(EntityModel BaseEntity, StationModel StationToDelete)
        {
            if (StationToDelete == null) { throw new ArgumentNullException(nameof(StationToDelete)); }
            if (BaseEntity == null) { throw new ArgumentNullException(nameof(BaseEntity)); }

            return await DeleteStation(BaseEntity.Neris_Id, StationToDelete.Neris_Id);
        }

        public async Task<bool> DeleteStation(string BaseEntityId, string StationId)
        {
            if (string.IsNullOrWhiteSpace(BaseEntityId)) { throw new ArgumentNullException(nameof(BaseEntityId)); }
            if (string.IsNullOrWhiteSpace(StationId)) { throw new ArgumentNullException(nameof(StationId)); }


            string fullUrl = GetStationRoute(BaseEntityId, StationId);
            using (var message = new HttpRequestMessage(HttpMethod.Delete, fullUrl))
            {
                HttpResponseMessage response = await _call(message);
                return response.IsSuccessStatusCode;
            }
        }

        public string GetStationRoute(string BaseEntityId, string stationId = null)
        {
            string baseUri = GetRoute(RouteTypes.Entity);
            baseUri = UriUtils.AppendPath(baseUri, BaseEntityId);
            baseUri = UriUtils.AppendPath(baseUri, "station");
            if (!string.IsNullOrWhiteSpace(stationId))
            {
                baseUri = UriUtils.AppendPath(baseUri, stationId);
            }
            return baseUri;
        }
    }
}

