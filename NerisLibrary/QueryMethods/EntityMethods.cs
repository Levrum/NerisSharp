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
        /// <exception cref="ValidationException">Indicates EntityRequestModel is not valid</exception>
        /// <exception cref="AuthorizationException">Indicates failure to login and receive NERIS Auth token</exception>
        public async Task<EntityPageSet> GetEntities(EntityRequestModel requestModel)
        {
            //check if request is valid
            if (!requestModel.Validate())
            {
                throw new ValidationException("RequestModel is not valid");
            }
            //check if logged in
            await LoginIfTokenExpired();

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
            await LoginIfTokenExpired();

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

        public async Task<string> PatchEntity(EntityModel EntityToUpdate, HashSet<string> FieldsToNull = null)
        {
            if (EntityToUpdate == null) { throw new ArgumentNullException(nameof(EntityToUpdate)); }
            if (string.IsNullOrWhiteSpace(EntityToUpdate.Neris_Id)) { throw new ArgumentException("Neris_Id cannot be null or whitespace"); }
            string entityId = EntityToUpdate.Neris_Id;
            await LoginIfTokenExpired();

            string baseUri = GetRoute(RouteTypes.Entity);
            string entityUri = UriUtils.AppendPath(baseUri, EntityToUpdate.Neris_Id);

            string neris_id = "";
            using (var message = new HttpRequestMessage(HttpMethod.Patch, entityUri))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Access_Token);
                JsonObject nodeContent = SerializationExtensions.SerializeToJsonObjectLowerCase(EntityToUpdate);
                nodeContent.Remove("neris_id");
                if (FieldsToNull != null)
                {
                    foreach (string field in FieldsToNull)
                    {
                        nodeContent[field.ToLower()] = null;
                    }
                }
                message.Content = CreateJsonContent(nodeContent.ToJsonString());
                HttpResponseMessage response = await _httpClient.SendAsync(message);
                await CheckStatusCodeAndHandleError(response);
                neris_id = await ParseIdFromCreatedResult(response);
            }
            return neris_id;
        }
    }
}
