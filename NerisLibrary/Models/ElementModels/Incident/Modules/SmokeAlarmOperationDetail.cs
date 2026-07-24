using System;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident.Modules
{
    public class SmokeAlarmOperationDetail
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public SmokeAlarmOperationDetailTypeEnum Type { get; set; }

        /// <summary>
        /// OPERATED_ALERTED_OCCUPANT Type Only
        /// </summary>
        public OccupantActionEnum? Occupant_Action { get; set; }

        /// <summary>
        /// FAILED_TO_OPERATE Type Only
        /// </summary>
        public FailureReasonEnum? Failure_Reason { get; set; }
    }

    public enum SmokeAlarmOperationDetailTypeEnum
    {
        OPERATED_ALERTED_OCCUPANT,
        FAILED_TO_OPERATE,
        OPERATED_FAILED_TO_ALERT_OCCUPANT,
        NO_OCCUPANT_TO_NOTIFY,
        INSUFFICIENT_SOURCE,
    }

    public enum OccupantActionEnum
    {
        ATTEMPTED_TO_EXTINGUISH,
        ATTEMPTED_TO_RESCUE_ANIMALS,
        ATTEMPTED_TO_RESCUE_OCCUPANTS,
        EVACUATED,
        IGNORED_ALARM,
        UNABLE_TO_RESPOND,
        UNKNOWN,
    }

    public enum FailureReasonEnum
    {
        DEVICE_MALFUNCTION,
        EXPIRED,
        IMPROPER_INSTALLATION,
        NO_BATTERY,
        OTHER_NON_FUNCTIONAL_CAUSE,
        TAMPER,
        UNABLE_TO_DETERMINE,
    }
}
