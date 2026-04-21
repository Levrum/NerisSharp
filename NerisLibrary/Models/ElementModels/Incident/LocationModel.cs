using NerisLibrary.Utils;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    /// <summary>
    /// Class containing information about the addressed location of the incident. Contains all info for a street address plus some additional info.
    /// </summary>
    public class LocationModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }
        public Dictionary<string, string> Additional_Attributes { get; set; }
        public string Place_Type { get; set; } //TODO ADD ENUM
        public string County { get; set; }
        public string State { get; set; }
        public string Postal_Code { get; set; }
        public string Street_Prefix_Modifier { get; set; }
        public string Street_Prefix_Direction { get; set; }
        public string Street { get; set; }
        public string Street_Postfix_Direction { get; set; }
        public string Street_Postfix_Modifier { get; set; }
        private string _street_prefix;
        public string Street_Prefix { get { return _street_prefix; } set { _street_prefix = value?.ToUpper(); } } //CAPITALIZED
        public string Street_Preposition_Type_Separator { get; set; }
        private string _street_postfix;
        public string Street_Postfix { get { return _street_postfix; } set { _street_postfix = value?.ToUpper(); } } //CAPITALIZED
        public int? Number { get; set; }
        public string Complete_Number { get; set; } //this is null for some reason?
        public string Unit_Value { get; set; }
        public string Distance_Marker { get; set; }
        public string Additional_Info { get; set; }

        public string GetStreetAddress()
        {
            //StringBuilder sb = new StringBuilder();
            //sb.Append(Street_Prefix_Modifier);
            //sb.AppendFormat(" {0}", Street_Prefix_Direction);
            //sb.AppendFormat(" {0}", Street_Prefix);
            //sb.AppendFormat(" {0}", Street_Preposition_Type_Separator);
            //sb.AppendFormat(" {0}", Street);
            //sb.AppendFormat(" {0}", Street_Postfix);
            //sb.AppendFormat(" {0}", Street_Postfix_Direction);
            //sb.AppendFormat(" {0}", Street_Postfix_Modifier);
            List<string> streetsInOrder = new List<string>();
            string? number = (Complete_Number == null) ? Number?.ToString() ?? null : Complete_Number;
            streetsInOrder.AddIfNotNull(number);
            streetsInOrder.AddIfNotNull(Street_Prefix_Modifier);
            streetsInOrder.AddIfNotNull(Street_Prefix_Direction);
            streetsInOrder.AddIfNotNull(Street_Prefix);
            streetsInOrder.AddIfNotNull(Street_Preposition_Type_Separator);
            streetsInOrder.AddIfNotNull(Street);
            streetsInOrder.AddIfNotNull(Street_Postfix);
            streetsInOrder.AddIfNotNull(Street_Postfix_Direction);
            streetsInOrder.AddIfNotNull(Street_Postfix_Modifier);
            StringBuilder sb = new StringBuilder();
            sb.AppendJoin(" ", streetsInOrder.ToArray());
            return sb.ToString();
        }
    }
}
