using NerisLibrary.Models.ElementModels.Incident.PatchObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    /// <summary>
    /// Contains DateTimeOffsets for timing events that occur during an incident.
    /// </summary>
    public class TacticsTimestamps
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public DateTimeOffset? Command_Established { get; set; } = null;
        public DateTimeOffset? Completed_Sizeup { get; set; } = null;
        public DateTimeOffset? Suppression_Complete { get; set; } = null;
        public DateTimeOffset? Primary_Search_Begin { get; set; } = null;
        public DateTimeOffset? Primary_Search_Complete { get; set; } = null;
        public DateTimeOffset? Water_On_Fire { get; set; } = null;
        public DateTimeOffset? Fire_Under_Control { get; set; } = null;
        public DateTimeOffset? Fire_Knocked_Down { get; set; } = null;
        public DateTimeOffset? Extrication_Complete { get; set; } = null;
    }

    public class TacticsTimestampPatchProperties
    {
        public PatchAction<DateTimeOffset>? Command_Established { get; set; } = null;
        public PatchAction<DateTimeOffset>? Completed_Sizeup { get; set; } = null;
        public PatchAction<DateTimeOffset>? Suppression_Complete { get; set; } = null;
        public PatchAction<DateTimeOffset>? Primary_Search_Begin { get; set; } = null;
        public PatchAction<DateTimeOffset>? Primary_Search_Complete { get; set; } = null;
        public PatchAction<DateTimeOffset>? Water_On_Fire { get; set; } = null;
        public PatchAction<DateTimeOffset>? Fire_Under_Control { get; set; } = null;
        public PatchAction<DateTimeOffset>? Fire_Knocked_Down { get; set; } = null;
        public PatchAction<DateTimeOffset>? Extrication_Complete { get; set; } = null;
    }
}
