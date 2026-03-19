using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class UnitResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Unit_Neris_Id { get; set; }
        public string Reported_Unit_Id { get; set; }
        public int? Staffing { get; set; } = null;
        public bool? Unable_To_Dispatch { get; set; } = null;
        public DateTimeOffset Dispatch { get; set; }
        public DateTimeOffset Enroute_To_Scene { get; set; }
        public DateTimeOffset On_Scene { get; set; }
        public DateTimeOffset Canceled_Enroute { get; set; }
        public DateTimeOffset Staging { get; set; }
        public DateTimeOffset Unit_Clear { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseMode? Response_Mode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseMode? Transport_Mode { get; set; }
        public List<MedReponse> Med_Responses { get; set; }
    }

    public enum ResponseMode
    {
        EMERGENT,
        NON_EMERGENT
    }
}
