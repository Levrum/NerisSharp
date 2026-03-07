using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels
{
    public class StationModel
    {
        public string Neris_Id { get; set; }
        public string Station_Id { get; set; }
        public string Address_Line_1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip_Code { get; set; }
        public string Location { get; set; }
    }
}
