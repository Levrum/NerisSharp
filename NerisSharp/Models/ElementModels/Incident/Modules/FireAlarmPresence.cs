using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.Modules
{
    public class FireAlarmPresence
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public AlarmPresenceEnum Type { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public List<FireAlarmTypeEnum>? Alarm_Types { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public FireAlarmOperationTypeEnum? Operation_Type { get; set; }
    }
    public enum FireAlarmOperationTypeEnum
    {
        OPERATED_ALERTED_OCCUPANT,
        FAILED_TO_OPERATE,
        OPERATED_FAILED_TO_ALERT_OCCUPANT,
        NO_OCCUPANT_TO_NOTIFY,
        INSUFFICIENT_SOURCE,
    }

    public enum FireAlarmTypeEnum
    {
        AUTOMATIC,
        MANUAL,
        MANUAL_AND_AUTOMATIC,
    }
}
