using NerisLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class LocationModel
    {
        public int? Neris_Uid { get; set; }
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
        public string Street_Prefix { get; set; }
        public string Street_Preposition_Type_Separator { get; set; }
        public string Street_Postfix { get; set; }
        public string Complete_Number { get; set; }
        public string Additional_Info { get; set; }

        public string StreetAddress()
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
