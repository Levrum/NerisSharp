using Microsoft.AspNetCore.WebUtilities;
using NerisLibrary.Exceptions;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Models.RequestModels;
using NerisLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NerisLibrary
{
    public partial class NerisBase
    {
        /// <summary>
        /// Retrieves a specific incident by its unique identifier for the specified base entity.
        /// </summary>
        /// <param name="baseEntity">The base entity of which the incident to search belongs. Cannot be null.</param>
        /// <param name="incidentId">The unique identifier of the incident to retrieve. Cannot be null or whitespace.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the incident model if found;
        /// otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="baseEntityId"/> or <paramref name="incidentId"/> is null or consists only
        /// of white-space characters.</exception>
        public async Task<IncidentModel> GetIncidentById(EntityModel baseEntity, string incidentId, bool geoformatGeoJson = false)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            return await GetIncidentById(baseEntity.Neris_Id, incidentId, geoformatGeoJson);
        }

        /// <summary>
        /// Retrieves a specific incident by its unique identifier for the specified base entity.
        /// </summary>
        /// <param name="baseEntityId">The unique identifier of the base entity to which the incident belongs. Cannot be null or whitespace.</param>
        /// <param name="incidentId">The unique identifier of the incident to retrieve. Cannot be null or whitespace.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the incident model if found;
        /// otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="baseEntityId"/> or <paramref name="incidentId"/> is null or consists only
        /// of white-space characters.</exception>
        public async Task<IncidentModel> GetIncidentById(string baseEntityId, string incidentId, bool geoformatGeoJson = false)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (string.IsNullOrWhiteSpace(incidentId)) throw new ArgumentNullException(nameof(incidentId));
            string endpoint = GetIncidentRoute(baseEntityId, incidentId);
            if (geoformatGeoJson) //for some reason, this doesn't matter, you'll get a url back either way.
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "geo_format", "geojson");
            }
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, endpoint))
            {
                HttpResponseMessage response = await _call(message);
                IncidentModel model = await response.Content.DeserializeCaseInsensitive<IncidentModel>();
                return model;
            }
        }

        /// <summary>
        /// Retrieves a paged set of incidents that match the specified search criteria.
        /// </summary>
        /// <param name="requestModel">An object containing the search parameters and pagination options for filtering incidents. Cannot be null
        /// and must pass validation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IncidentPageSet with the
        /// incidents matching the search criteria and pagination information.</returns>
        /// <exception cref="ArgumentNullException">Thrown if requestModel is null.</exception>
        /// <exception cref="ValidationException">Thrown if requestModel fails validation.</exception>
        public async Task<IncidentPageSet> GetIncidentsPage(IncidentRequestModel requestModel)
        {
            if (requestModel == null) throw new ArgumentNullException(nameof(requestModel));
            if (!requestModel.Validate()) throw new ValidationException("Parameter requestModel is not valid");
            string endpoint = GetIncidentRoute();
            string endpointWithSearchParams = requestModel.CreateQueryURI(endpoint);
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, endpointWithSearchParams))
            {
                HttpResponseMessage response = await _call(message);
                return await response.Content.DeserializeCaseInsensitive<IncidentPageSet>();
            }
        }

        /// <summary>
        /// Retrieves all incidents that match the specified request criteria.
        /// </summary>
        /// <remarks>This method automatically handles pagination and returns the complete set of matching
        /// incidents. The operation may be time-consuming if the result set is large.</remarks>
        /// <param name="requestModel">An object containing the filter and pagination options for retrieving incidents. Must not be null. The
        /// properties of this model determine which incidents are returned.</param>
        /// <returns>A list of incidents matching the specified criteria. The list will be empty if no incidents are found.</returns>
        public async Task<List<IncidentModel>> GetAllIncidents(IncidentRequestModel requestModel)
        {
            string nextCursor = string.Empty;
            List<IncidentModel> toReturn = new List<IncidentModel>();
            requestModel.Page_Size = 100; //minimze the number of calls to make
            do
            {
                IncidentPageSet pageSet = await GetIncidentsPage(requestModel);
                nextCursor = pageSet.Next_Cursor;
                requestModel.Cursor = nextCursor;
                toReturn.AddRange(pageSet.Incidents);
            } while (!string.IsNullOrWhiteSpace(nextCursor));

            return toReturn;
        }

        public List<IncidentModel> GetAllIncidentsSync(IncidentRequestModel requestModel)
        {
            return SyncRunner.RunSync(() => GetAllIncidents(requestModel));
        }

        /// <summary>
        /// Creates a new incident for the specified entity and returns the unique identifier of the created incident.
        /// </summary>
        /// <param name="baseEntity">The entity to which the incident will be associated. Cannot be null.</param>
        /// <param name="newIncident">An object containing the details of the incident to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the
        /// newly created incident as a string.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="baseEntity"/> is null or if its id is empty, or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newIncident"/> is null.</exception>
        public async Task<string> PostIncident(EntityModel baseEntity, IncidentModel newIncident)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            return await PostIncident(baseEntity.Neris_Id, newIncident);
        }

        /// <summary>
        /// Creates a new incident for the specified entity and returns the unique identifier of the created incident.
        /// </summary>
        /// <param name="baseEntityId">The unique identifier of the entity to which the incident will be associated. Cannot be null, empty, or
        /// consist only of white-space characters.</param>
        /// <param name="newIncident">An object containing the details of the incident to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the
        /// newly created incident as a string.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="baseEntityId"/> is null, empty, or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newIncident"/> is null.</exception>
        public async Task<string> PostIncident(string baseEntityId, IncidentModel newIncident)
        {
            CheckWriteAllowed();
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentException("Entity Id must not be null or whitespace");
            if (newIncident == null) throw new ArgumentNullException(nameof(newIncident));
            string endpoint = GetIncidentRoute(baseEntityId);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                string jsonContent = SerializationExtensions.SerializeLowerCase(newIncident);
                request.Content = CreateJsonContent(jsonContent);

                HttpResponseMessage response = await _call(request);

                return await ParseIdFromCreatedResult(response);
            }
        }


        /// <summary>
        /// Updates an existing incident for the specified entity using the provided incident data.
        /// </summary>
        /// <param name="baseEntity">The entity to which the incident belongs. Cannot be null.</param>
        /// <param name="incidentToPut">The incident data to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the incident
        /// was updated successfully.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntity"/> is null or its id is empty, or consists only of white-space characters, or if
        /// <paramref name="incidentToPut"/> is null.</exception>
        public async Task<bool> PutIncident(EntityModel baseEntity, IncidentModel incidentToPut)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            return await PutIncident(baseEntity.Neris_Id, incidentToPut);
        }

        /// <summary>
        /// Updates an existing incident for the specified entity using the provided incident data.
        /// </summary>
        /// <param name="baseEntityId">The unique identifier of the entity to which the incident belongs. Cannot be null, empty, or consist only of
        /// white-space characters.</param>
        /// <param name="incidentToPut">The incident data to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the incident
        /// was updated successfully.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="baseEntityId"/> is null, empty, or consists only of white-space characters, or if
        /// <paramref name="incidentToPut"/> is null.</exception>
        public async Task<bool> PutIncident(string baseEntityId, IncidentModel incidentToPut)
        {
            CheckWriteAllowed();
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (incidentToPut == null) throw new ArgumentNullException(nameof(incidentToPut));

            string endpoint = GetIncidentRoute(baseEntityId, incidentToPut.Neris_Id);
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, endpoint))
            {
                JsonObject contentObject = SerializationExtensions.SerializeToJsonObjectLowerCase(incidentToPut);
                contentObject.Remove("neris_id");
                contentObject.Remove("submitter_account_type");
                contentObject.Remove("incident_status");
                request.Content = CreateJsonContent(contentObject.ToJsonString());
                HttpResponseMessage response = await _call(request);

                return true;
            }
        }


        //NOT FULLY IMPLEMENTED.
        //public async Task<bool> PatchIncident(string baseEntityId, IncidentPatchPayload payload)
        //{
        //    if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
        //    if (payload == null) throw new ArgumentNullException(nameof(payload));

        //    string endpoint = GetIncidentRoute(baseEntityId, payload.Neris_Id);
        //    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, endpoint))
        //    {
        //        string jsonContent = SerializationExtensions.SerializeLowerCase(payload);
        //        request.Content = CreateJsonContent(jsonContent);
        //        HttpResponseMessage response = await _call(request);

        //        return true;
        //    }
        //}

        private string GetIncidentRoute(string entityId = null, string incidentId = null)
        {
            string baseRoute = GetRoute(RouteTypes.Incident);
            if (entityId != null)
            {
                baseRoute = UriUtils.AppendPath(baseRoute, entityId);
                if (incidentId != null)
                {
                    baseRoute = UriUtils.AppendPath(baseRoute, incidentId);
                }
            }
            return baseRoute;
        }
    }

    /// <summary>
    /// Represents a page's worth of results from the NERIS api. Contains a list of Incidents and the cursors for retrieving other pages.
    /// </summary>
    public class IncidentPageSet
    {
        public string Next_Cursor { get; set; }
        public string Prev_Cursor { get; set; }
        public List<IncidentModel> Incidents { get; set; }
    }
}
