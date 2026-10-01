using System;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident
{
    public class IncidentSpecialModifiers
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int? Neris_Uid { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public IncidentSpecialModifierEnum Type { get; set; }
    }

    public enum IncidentSpecialModifierEnum
    {
        ACTIVE_ASSAILANT,
        CIVIL_UNREST,
        COUNTY_LOCAL_DECLARED_DISASTER,
        FEDERAL_DECLARED_DISASTER,
        MCI,
        STATE_DECLARED_DISASTER,
        URBAN_CONFLAGRATION,
        VIOLENCE_AGAINST_RESPONDER,
        [Obsolete("Deprecated in NERIS 1.5.")]
        WORLD_CUP_2026,
    }
}
