using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class IncidentStatus
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Created_By { get; set; }
        public IncidentStatusTypes Incident_Status { get; set; }
    }
    public enum IncidentStatusTypes
    {
        APPROVED = 0,
        DELETED,
        FAILED,
        PENDING_APPROVAL,
        PENDING_INCIDENT_DATA,
        REJECTED,
        SUBMITTED
    }
}
