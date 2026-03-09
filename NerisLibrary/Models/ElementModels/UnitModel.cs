using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels
{
    public class UnitModel
    {
        public int Staffing { get; set; } = 0;
        public bool Dedicated_Staffing { get; set; } = true;
        public string Neris_Id { get; set; } 
        public int Version { get; set; } = 0;
        public string Type { get; set; } 
        public string Cad_Designation_1 { get; set; }
        public string Cad_Designation_2 {  get; set; }
    }
}
