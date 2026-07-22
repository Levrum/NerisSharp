using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
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
        COUNTY_LOCAL_DECLARED_DISASTER,
        FEDERAL_DECLARED_DISASTER,
        MCI,
        STATE_DECLARED_DISASTER,
        URBAN_CONFLAGRATION,
        VIOLENCE_AGAINST_RESPONDER,
        WORLD_CUP_2026,
    }
}
