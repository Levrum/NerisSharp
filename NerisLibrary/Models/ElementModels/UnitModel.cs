using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels
{
    public class UnitModel
    {
        public int Staffing { get; set; } = 0;
        public bool Dedicated_Staffing { get; set; } = true;
        public string Neris_Id { get; set; } = string.Empty;
        public int Version { get; set; } = 0;
        public string Type { get; set; } = string.Empty;
        public string Cad_Designation_1 { get; set; } = String.Empty;
        public string Cad_Designation_2 {  get; set; } = String.Empty;
    }
}
