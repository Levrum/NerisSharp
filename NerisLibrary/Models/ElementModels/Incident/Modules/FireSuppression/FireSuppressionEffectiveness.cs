using System;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.Modules
{
    public class FireSuppressionEffectiveness
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public FireSuppressionEffectivenessTypeEnum Type { get; set; }

        /// <summary>
        /// Operated Effective and Operated Not Effective Types Only
        /// </summary>
        public int? Sprinklers_Activated { get; set; }

        /// <summary>
        /// Operated Not Effective and No Operation Types Only
        /// </summary>
        public FireSuppressionFailureReasonEnum? Failure_Reason { get; set; }
    }

    public enum FireSuppressionEffectivenessTypeEnum
    {
        OPERATED_EFFECTIVE,
        OPERATED_NOT_EFFECTIVE,
        NO_OPERATION,
    }

    public enum FireSuppressionFailureReasonEnum
    {
        INSUFFICIENT_SOURCE,
        INSUFFICIENT_WATER_SUPPLY,
        SYSTEM_DAMAGED_COMPROMISED,
        SYSTEM_INOPERABLE,
        SYSTEM_NOT_SUITABLE,
        SYSTEM_SHUTOFF_DURING_INCIDENT,
        SYSTEM_SHUTOFF_PRIOR_TO_INCIDENT,
        UNABLE_TO_DETERMINE,
    }
}
