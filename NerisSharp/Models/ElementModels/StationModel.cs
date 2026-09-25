using System.Collections.Generic;

namespace NerisSharp.Models.ElementModels
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
        public int? Staffing { get; set; } = null;
        public string Internal_Id { get; set; }
        public string Zip_Code { get; set; }
        public string Location { get; set; }
        /// <summary>
        /// Whether the station is in service. Set to false when a station is taken offline for
        /// renovations or repair so its NERIS id is preserved; not intended for real-time availability.
        /// The server defaults this to true. Left null here so a PATCH omits it unless the caller sets it explicitly.
        /// </summary>
        public bool? In_Service { get; set; } = null;
        public List<UnitModel> Units { get; set; }
    }
}
