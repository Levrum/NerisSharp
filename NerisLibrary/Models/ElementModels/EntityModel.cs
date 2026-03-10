using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels
{
    public class EntityModel
    {
        public string Name { get; set; }
        public string Neris_Id { get; set; } 
        public string Address_Line_1 { get; set; } 
        public string City { get; set; } 
        public string State { get; set; } 
        public string Zip_Code { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public string Department_Type { get; set; } 
        public string Website {  get; set; } 
        public string Time_Zone { get; set; } 
        public string Location { get; set; } 
        public List<StationModel> Stations { get; set; }
    }
}
