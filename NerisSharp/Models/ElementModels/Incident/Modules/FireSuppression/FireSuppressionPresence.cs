using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.Modules
{
    public class FireSuppressionPresence
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public AlarmPresenceEnum Type { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public List<FireSuppressionType>? Suppression_Types { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public FireSuppressionOperation? Operation_Type { get; set; }
    }

    public class FireSuppressionType
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public FireSuppressionTypeEnum Type { get; set; }
        public FullPartialEnum? Full_Partial { get; set; }
    }

    public enum FireSuppressionTypeEnum
    {
        CLEAN_AGENT_SYSTEM,
        DELUGE_SYSTEM,
        DRY_PIPE_SPRINKLER_SYSTEM,
        INDUSTRIAL_DRY_CHEM_SYSTEM,
        OTHER,
        PRE_ACTION_SYSTEM,
        UNKNOWN,
        WET_PIPE_SPRINKLER_SYSTEM,
    }

    public enum FullPartialEnum
    {
        EXTENT_UNKNOWN,
        FULL,
        PARTIAL,
    }
}
