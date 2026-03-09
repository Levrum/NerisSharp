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
        public string Address_Line_2 { get; set; } 
        public string City { get; set; } 
        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value.ToUpper();
            }
        }
        public int Staffing { get; set; } = 0;
        public string Internal_Id { get; set; } 
        public string Zip_Code { get; set; }
        public string Location { get; set; } 
        public List<UnitModel> Units { get; set; } = new List<UnitModel>();
    }
}
