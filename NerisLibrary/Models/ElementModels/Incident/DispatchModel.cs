using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    /// <summary>
    /// Contains info regarding the call when dispatched. Includes Dispatch center, original code, internal id, DateTimes for when call was processed by dispatch.
    /// Also contains versions of the Tactics Timestamps and Unit Responses. Unclear how these relate to the ones on the Incident model, NERIS system does not copy them to one another.
    /// Similarly contains a location.
    /// </summary>
    public class DispatchModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Center_Id { get; set; } //4 digit formatted string
        public string Incident_Number { get; set; }
        public string Determinant_Code { get; set; }
        public string Incident_Code { get; set; }
        public string Disposition { get; set; }
        public bool? Automatic_Alarm { get; set; } = null;
        public DateTimeOffset? Incident_Clear { get; set; } = null;
        public DateTimeOffset Call_Arrival { get; set; }
        public DateTimeOffset Call_Answered { get; set; }
        public DateTimeOffset Call_Create { get; set; }
        public LocationModel Location { get; set; }
        //comments
        public TacticsTimestamps Tactic_Timestamps { get; set; }
        public List<UnitResponse> Unit_Responses { get; set; } = new List<UnitResponse>(); //this field is required so an empty list is required rather than null

    }
}
