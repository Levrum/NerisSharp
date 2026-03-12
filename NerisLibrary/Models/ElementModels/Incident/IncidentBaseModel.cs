using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class IncidentBaseModel
    {
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public bool? People_Present { get; set; }
        public int? Animals_Rescued { get; set; }
        public string Impediment_Narrative { get; set; }
        public string Outcome_Narrative { get; set; }
        public int? Displacement_Count { get; set; }
        public string Department_Neris_Id { get; set; }
        public string Incident_Number { get; set; }
        public List<string> Displacement_Causes { get; set; }
        public LocationModel Location { get; set; }
        //location use
        public string Point { get; set; }
        public string Polygon { get; set; }
    }
}
