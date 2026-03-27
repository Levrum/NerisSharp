using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    /// <summary>
    /// Supplemental class for Medical Response modeling timestamps for medical transport events.
    /// </summary>
    public class MedReponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Hospital_Destination { get; set; }
        public DateTimeOffset? At_Patient { get; set; } = null;
        public DateTimeOffset? Enroute_To_Hospital { get; set; } = null;
        public DateTimeOffset? Arrived_At_Hospital { get; set; } = null;
        public DateTimeOffset? Transferred_to_Agency { get; set; } = null;
        public DateTimeOffset? Transferred_to_Facility { get; set; } = null;
        public DateTimeOffset? Hospital_Cleared { get; set; } = null;

    }
}
