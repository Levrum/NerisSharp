using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NerisLibrary
{
    public partial class NerisBase
    {
        public async Task<IncidentModel> GetIncidentById(EntityModel baseEntity, string incidentId)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            return await GetIncidentById(baseEntity.Neris_Id, incidentId);
        }

        public async Task<IncidentModel> GetIncidentById(string baseEntityId, string incidentId)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentNullException(nameof(baseEntityId));
            if (string.IsNullOrWhiteSpace(incidentId)) throw new ArgumentNullException(nameof(incidentId));
            string endpoint = GetIncidentRoute(baseEntityId, incidentId);
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, endpoint))
            {
                HttpResponseMessage response = await _call(message);
                IncidentModel model = await response.Content.DeserializeCaseInsensitive<IncidentModel>();
                return model;
            }
        }
        public async Task<string> PostIncident(EntityModel baseEntity, IncidentModel newIncident)
        {
            if (baseEntity == null) throw new ArgumentNullException(nameof(baseEntity));
            return await PostIncident(baseEntity.Neris_Id, newIncident);
        }

        public async Task<string> PostIncident(string baseEntityId, IncidentModel newIncident)
        {
            if (string.IsNullOrWhiteSpace(baseEntityId)) throw new ArgumentException("Entity Id must not be null or whitespace");
            if (newIncident == null) throw new ArgumentNullException(nameof(newIncident));
            string endpoint = GetIncidentRoute(baseEntityId);

            using (HttpRequestMessage request =  new HttpRequestMessage(HttpMethod.Post, endpoint))
            {
                string jsonContent = SerializationExtensions.SerializeLowerCase(newIncident);
                request.Content = CreateJsonContent(jsonContent);

                HttpResponseMessage response = await _call(request);

                return await ParseIdFromCreatedResult(response);
            }

        }

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
}
