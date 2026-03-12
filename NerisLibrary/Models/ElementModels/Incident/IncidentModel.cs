using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class IncidentModel
    {
        public string Neris_Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }

        //ignore? json string writer?
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SubmitterAccountTypes Submitter_Account_Type { get; set; }
        public IncidentBaseModel Base { get; set; }
    }

    public enum SubmitterAccountTypes
    {
        CAD,
    }
}
