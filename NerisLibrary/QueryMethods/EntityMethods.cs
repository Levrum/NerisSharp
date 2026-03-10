using NerisLibrary.Exceptions;
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
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NerisLibrary //must use top level namespace for partial class to access all methods.
{
    public partial class NerisBase
    {
        /// <summary>
        /// Search entities by various parameters
        /// </summary>
        /// <param name="requestModel">Object containing parameters to search by</param>
        /// <returns>An EntityPageSet containing a list of found entities and pagination info</returns>
        /// <exception cref="Exception"></exception>
        public async Task<EntityPageSet> GetEntities(EntityRequestModel requestModel)
        {
            //check if request is valid
            if (!requestModel.Validate())
            {
                throw new ValidationException("RequestModel is not valid");
            }
            //check if logged in
            if (!await LoginIfTokenExpired())
            {
                //failed to login:
                throw new AuthorizationException(); 
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
                await CheckStatusCodeAndHandleError(response);
                entities = await response.Content.DeserializeCaseInsensitive<EntityPageSet>();

            }
            //handle results
            return entities;

        }

        public async Task<EntityModel?> GetEntity(string EntityId)
        {
            if (EntityId == null)
            {
                throw new ArgumentNullException(nameof(EntityId));
            }
            if (!await LoginIfTokenExpired())
            {
                throw new AuthorizationException();
            }
            string entityUri = GetRoute(RouteTypes.Entity);
            string entitySearchUri = UriUtils.AppendPath(entityUri, EntityId);
            EntityModel? entityModel = null;
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, entitySearchUri))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);


                entityModel = await response.Content.DeserializeCaseInsensitive<EntityModel>();
            }
            return entityModel;
        }

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
            if (!await LoginIfTokenExpired())
            {
                //failed to login:
                throw new AuthorizationException();
            }
            string nerisId = string.Empty;
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, stationUri))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);
                string content = SerializationExtensions.SerializeLowerCase<StationModel>(NewStation);
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);

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
        public async Task<string> PatchStation(EntityModel BaseEntity,  StationModel StationUpdate, HashSet<string> FieldsToNull = null)
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
            string stationID = StationUpdate.Neris_Id;

            if (String.IsNullOrWhiteSpace(StationUpdate.Neris_Id))
            {
                throw new ArgumentException("StationUpdate MUST have NERIS id to patch");
            }
            if (!await LoginIfTokenExpired())
            {
                //failed to login:
                throw new AuthorizationException();
            }
            string route = GetStationRoute(BaseEntityId);
            string fullRoute = UriUtils.AppendPath(route, StationUpdate.Neris_Id);
            string nerisId = string.Empty;
            using (var message = new HttpRequestMessage(HttpMethod.Patch, fullRoute))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);
                StationUpdate.Neris_Id = null;
                JsonNode contentNode = SerializationExtensions.SerializeToNodeLowerCase(StationUpdate);
                //check for nulls: 
                if (FieldsToNull != null)
                {
                    foreach (string field in FieldsToNull)
                    {
                        contentNode[field.ToLower()] = null;
                    }
                }
                string content = contentNode.ToJsonString();
                StationUpdate.Neris_Id = stationID;
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);

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
            if (string.IsNullOrEmpty(BaseEntityId)) { throw new ArgumentNullException(nameof(BaseEntityId)); }
            if (string.IsNullOrEmpty(StationId)) { throw new ArgumentNullException(nameof(StationId)); }

            if (!await LoginIfTokenExpired())
            {
                //failed to login:
                throw new AuthorizationException();
            }

            string baseUrl = GetStationRoute(BaseEntityId);
            string fullUrl = UriUtils.AppendPath(baseUrl, StationId);
            using (var message = new HttpRequestMessage(HttpMethod.Delete, fullUrl))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);
                return response.IsSuccessStatusCode;
            }
        }

        public string GetStationRoute(string BaseEntityId)
        {
            string baseUri = GetRoute(RouteTypes.Entity);
            baseUri = UriUtils.AppendPath(baseUri, BaseEntityId);
            baseUri = UriUtils.AppendPath(baseUri, "station");
            return baseUri;
        }
    }
}
