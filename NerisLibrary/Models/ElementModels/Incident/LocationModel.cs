using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class LocationModel
    {
        public int? Neris_Uid { get; set; }
        public Dictionary<string, string> Additional_Attributes { get; set; }
        public string Place_Type { get; set; }
        public string Count { get; set; }
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
            StringBuilder sb = new StringBuilder();
            sb.Append(Street_Prefix_Modifier);
            sb.AppendJoin(" ", Street_Prefix_Direction);
            sb.AppendJoin(" ", Street_Prefix);
            sb.AppendJoin(" ", Street_Preposition_Type_Separator);
            sb.AppendJoin(" ", Street);
            sb.AppendJoin(" ", Street_Postfix);
            sb.AppendJoin(" ", Street_Postfix_Direction);
            sb.AppendJoin(" ", Street_Postfix_Modifier);
            return sb.ToString();
        }
    }
}
