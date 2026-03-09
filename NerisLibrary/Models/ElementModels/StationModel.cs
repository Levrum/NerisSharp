using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels
{
    public class StationModel
    {
        public string Neris_Id { get; set; } = string.Empty;
        public string Station_Id { get; set; } = string.Empty;
        public string Address_Line_1 { get; set; } = string.Empty;
        public string Address_Line_2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
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
        public string Internal_Id { get; set; } = string.Empty;
        public string Zip_Code { get; set; }= string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<UnitModel> Units { get; set; } = new List<UnitModel>();
    }
}
