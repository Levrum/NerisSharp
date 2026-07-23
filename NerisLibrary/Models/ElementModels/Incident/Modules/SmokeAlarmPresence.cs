using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident.Modules
{
    public class SmokeAlarmPresence
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public AlarmPresenceEnum Type { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public bool? Working { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public List<SmokeAlarmTypeEnum>? Alarm_Types { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public SmokeAlarmOperation? Operation { get; set; }
    }

    public enum AlarmPresenceEnum
    {
        PRESENT,
        NOT_PRESENT,
        NOT_APPLICABLE,
    }

    public enum SmokeAlarmTypeEnum
    {
        BED_SHAKER,
        COMBINATION,
        HARDWIRED,
        HARD_OF_HEARING_WITH_STROBE,
        INTERCONNECTED,
        LONG_LIFE_BATTERY_POWERED,
        REPLACEABLE_BATTERY_POWERED,
        UNKNOWN,
    }
}
