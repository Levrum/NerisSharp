using NerisSharp.Exceptions;
using NerisSharp.Models.ElementModels;
using NerisSharp.Models.RequestModels;
using NerisSharp.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NerisSharp //must use top level namespace for partial class to access all methods.
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


            //build request uri
            string baseUri = GetRoute(RouteTypes.Entity);
            string uri = requestModel.CreateQueryURI(baseUri);
            EntityPageSet entities = null;
            //make request
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                HttpResponseMessage response = await _call(message);
                entities = await response.Content.DeserializeCaseInsensitive<EntityPageSet>();
            }
            //handle results
            return entities;

        }

        /// <summary>
        /// Retrieves the entity associated with the specified entity identifier.
        /// </summary>
        /// <param name="EntityId">The unique identifier of the entity to retrieve. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity model if found;
        /// otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if EntityId is null.</exception>
        public async Task<EntityModel?> GetEntity(string EntityId)
        {
            if (EntityId == null)
            {
                throw new ArgumentNullException(nameof(EntityId));
            }

            string entityUri = GetRoute(RouteTypes.Entity);
            string entitySearchUri = UriUtils.AppendPath(entityUri, EntityId);
            EntityModel? entityModel = null;
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, entitySearchUri))
            {
                HttpResponseMessage response = await _call(message);

                entityModel = await response.Content.DeserializeCaseInsensitive<EntityModel>();
            }
            return entityModel;
        }

        /// <summary>
        /// Updates an existing entity with the specified values and optionally sets selected fields to null.
        /// </summary>
        /// <remarks>Only the fields present in EntityToUpdate are updated. Fields specified in
        /// FieldsToNull are explicitly set to null in the entity. The method performs a PATCH request to the underlying
        /// data store.</remarks>
        /// <param name="EntityToUpdate">The entity model containing updated values. The Neris_Id property must be set to identify the entity to
        /// update.</param>
        /// <param name="FieldsToNull">A set of field names to be set to null in the updated entity. If null, no fields are explicitly set to null.</param>
        /// <returns>A string containing the identifier of the updated entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown if EntityToUpdate is null.</exception>
        /// <exception cref="ArgumentException">Thrown if EntityToUpdate.Neris_Id is null or consists only of white-space characters.</exception>
        public async Task<string> PatchEntity(EntityModel EntityToUpdate, HashSet<string> FieldsToNull = null)
        {
            CheckWriteAllowed();
            if (EntityToUpdate == null) { throw new ArgumentNullException(nameof(EntityToUpdate)); }
            if (string.IsNullOrWhiteSpace(EntityToUpdate.Neris_Id)) { throw new ArgumentException("Neris_Id cannot be null or whitespace"); }
            string entityId = EntityToUpdate.Neris_Id;

            string baseUri = GetRoute(RouteTypes.Entity);
            string entityUri = UriUtils.AppendPath(baseUri, EntityToUpdate.Neris_Id);

            string neris_id = "";
            using (var message = new HttpRequestMessage(HttpMethod.Patch, entityUri))
            {
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
                HttpResponseMessage response = await _call(message);
                neris_id = await ParseIdFromCreatedResult(response);
            }
            return neris_id;
        }
    }
}
