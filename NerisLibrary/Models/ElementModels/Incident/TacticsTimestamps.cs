using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class TacticsTimestamps
    {
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
}
