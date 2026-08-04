using System;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident
{
    public class IncidentAid
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Department_Neris_Id { get; set; }
        public AidTypeEnum Aid_Type { get; set; }
        public AidDirectionEnum Aid_Direction { get; set; }

    }

    public enum AidTypeEnum
    {
        ACTING_AS_AID,
        IN_LIEU_AID,
        SUPPORT_AID
    }

    public enum AidDirectionEnum
    {
        GIVEN,
        RECEIVED
    }
}
