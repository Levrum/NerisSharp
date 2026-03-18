using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class DispatchModel
    {
        public int? Neris_Uid { get; set; } = null;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Center_Id { get; set; } //4 digit formatted string
        public string Incident_Number { get; set; }
        public string Determinant_Code { get; set; }
        public string Incident_Code { get; set; }
        public string Disposition { get; set; }
        public bool? Automatic_Alarm { get; set; } = null;
        public string Incident_Clear { get; set; }
        public DateTimeOffset Call_Arrival { get; set; }
        public DateTimeOffset Call_Answered { get; set; }
        public DateTimeOffset Call_Create { get; set; }
        public LocationModel Location { get; set; }
        //comments
        public TacticsTimestamps Tactic_Timestamps { get; set; }
        public List<UnitResponse> Unit_Responses { get; set; }

    }
}
