using System;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.Modules
{
    public class FireSuppression
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public FireSuppressionPresence Presence { get; set; }
    }
}
