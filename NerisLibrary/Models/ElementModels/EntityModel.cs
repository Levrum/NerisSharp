using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels
{
    public class EntityModel
    {
        public string Name { get; set; } = string.Empty;
        public string Neris_Id { get; set; } = string.Empty;
        public string Address_Line_1 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip_Code { get; set; } = string.Empty;
        public DateTimeOffset Last_Modified { get; set; }
        public string Department_Type { get; set; } = string.Empty;
        public string Website {  get; set; } = string.Empty;
        public string Time_Zone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<StationModel> Stations { get; set; } = new List<StationModel>();
    }
}
