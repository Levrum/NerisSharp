using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    /// <summary>
    /// Models a Unit's response to an incident including Ids and timestamps of events.
    /// </summary>
    public class UnitResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Unit_Neris_Id { get; set; } //one of Unit_Neris_Id or Reported_Unit_Id is required
        public string Reported_Unit_Id { get; set; }
        public int? Staffing { get; set; } = null;
        public bool? Unable_To_Dispatch { get; set; } = null;
        public DateTimeOffset? Dispatch { get; set; } = null;
        public DateTimeOffset? Enroute_To_Scene { get; set; } = null;
        public DateTimeOffset? On_Scene { get; set; } = null;
        public DateTimeOffset? Canceled_Enroute { get; set; } = null;
        public DateTimeOffset? Staging { get; set; } = null;
        public DateTimeOffset? Unit_Clear { get; set; } = null;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseMode? Response_Mode { get; set; } = null;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseMode? Transport_Mode { get; set; } = null;
        public List<MedReponse> Med_Responses { get; set; }
    }

    public enum ResponseMode
    {
        EMERGENT,
        NON_EMERGENT
    }
}
