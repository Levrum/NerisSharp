using NerisLibrary.Models.ElementModels.Incident.PatchObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class IncidentModel
    {
        public string Neris_Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }

        //ignore? json string writer?
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SubmitterAccountTypes? Submitter_Account_Type { get; set; } = null; //cannot be submitted
        public IncidentBaseModel Base { get; set; }
        public List<IncidentType> Incident_Types { get; set; }
        public IncidentStatus Incident_Status { get; set; }
        public DispatchModel Dispatch { get; set; }
        public TacticsTimestamps Tactic_Timestamps { get; set; }
        public List<UnitResponse> Unit_Responses { get; set; }

    }

    public enum SubmitterAccountTypes
    {
        CAD,
        RMS,
        USER
    }
    public class IncidentPatchPayload
    {
        public IncidentPatchPayload(string nerisId, IncidentPatchProperties patchProperties)
        {
            Neris_Id = nerisId;
            Properties = patchProperties;
        }    
        public string Neris_Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ActionTypes Action { get; } = ActionTypes.patch;
        public IncidentPatchProperties Properties { get; set; }
    }
    public class IncidentPatchProperties
    {
        public BasePatchObject Tactic_Timestamps { get; set; }
        public BasePatchObject Base { get; set; }
    }


}
