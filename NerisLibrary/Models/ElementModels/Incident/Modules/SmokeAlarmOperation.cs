using System;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident.Modules
{
    public class SmokeAlarmOperation
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public SmokeAlarmOperationDetail Alerted_Failed_Other { get; set; }
    }
}
