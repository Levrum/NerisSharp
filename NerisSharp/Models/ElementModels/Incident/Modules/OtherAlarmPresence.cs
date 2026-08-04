using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.Modules
{
    public class OtherAlarmPresence
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }

        public AlarmPresenceEnum Type { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public List<OtherAlarmTypeEnum>? Alarm_Types { get; set; }
    }

    public enum OtherAlarmTypeEnum
    {
        CARBON_MONOXIDE,
        HEAT_DETECTOR,
        NATURAL_GAS,
        OTHER_CHEMICAL_DETECTOR,
    }
}
