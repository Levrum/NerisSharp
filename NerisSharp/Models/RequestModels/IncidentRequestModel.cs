using Microsoft.AspNetCore.WebUtilities;
using NerisSharp.Models.ElementModels.Incident;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace NerisSharp.Models.RequestModels
{
    public class IncidentRequestModel : IRequestModel
    {
        public string Neris_Id_Entity { get; set; }
        public string Nuid_Entity_Set { get; set; }
        public List<IncidentType> Incident_Types { get; set; }
        public IncidentTypeSetOperation? Incident_Types_Set_Operation { get; set; }
        public DateTimeOffset? Call_Create_Start { get; set; } = null;
        public DateTimeOffset? Call_Create_End { get; set; } = null;
        public string Incident_Number { get; set; }
        public string Dispatch_Incident_Number { get; set; }
        public IncludeAid? Include_Aid { get; set; }
        public IncidentStatusTypes? Status { get; set; }
        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value?.ToUpper() ?? null;
            }
        }
        public string Last_Modified { get; set; }
        public string Sort_By { get; set; }
        public bool Sort_Ascending { get; set; } = true;
        public int Page_Size { get; set; } = 10; //max 100 min 1
        public string Cursor { get; set; }
        public bool Geo_Format_Json { get; set; } = true;

        public HashSet<string> SortByValues { get; } = new HashSet<string>()
        {
            "call_create",
            "neris_id_entity",
            "incident_number",
            "dispatch_incident_number",
            "status",
            "last_modified"
        };
        public bool Validate()
        {
            //check: state is valid (2 characters), last modifed string?, Sort_By and Sort_Order are valid. 
            bool valid = true;
            if (this.State != null && this.State.Length != 2)
            {
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(Neris_Id_Entity))
            {
                valid = false;
            }

            if (Page_Size < 1 || Page_Size > 100)
            {
                valid = false;
            }

            //last modified = too hard
            if (this.Sort_By != null && !SortByValues.Contains(this.Sort_By))
            {
                valid = false;
            }

            if (Incident_Types != null)
            {
                foreach (IncidentType iType in Incident_Types)
                {
                    if (!iType.Valid) valid = false;
                }
            }

            return valid;
        }


        HashSet<string> excludedFields = new HashSet<string>()
        {
            "Sort_Ascending",
            "Incident_Types",
            "Geo_Format_Json",
            "SortByValues"
        };
        //Method to add to URI
        //uses reflection to add each property minus those excluded into a dictonary to build the query
        //should this throw an error if not valid?
        public string CreateQueryURI(string baseUri)
        {
            Dictionary<string, string> queryValues = new Dictionary<string, string>();
            //add sort:
            queryValues.Add("sort_direction", this.Sort_Ascending ? "ASCENDING" : "DESCENDING");
            queryValues.Add("geo_format", this.Geo_Format_Json ? "geojson" : "url");

            //add the rest through reflection:
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                //skip excluded:
                if (excludedFields.Contains(property.Name))
                {
                    continue;
                }

                string name = property.Name.ToLower();
                object value = property.GetValue(this);
                if (value == null) { continue; }

                if (property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?))
                {
                    DateTimeOffset dto = (DateTimeOffset)value;
                    string dateIsoFormat = dto.ToString("O", CultureInfo.InvariantCulture);
                    queryValues.Add(name, dateIsoFormat);
                } else
                {
                    queryValues.Add(name, value.ToString());
                }
            }

            string uriString = QueryHelpers.AddQueryString(baseUri, queryValues);

            if (Incident_Types != null)
            {
                foreach (IncidentType incidentType in Incident_Types)
                {
                    uriString = QueryHelpers.AddQueryString(uriString, "incident_types", incidentType.Type);
                }
            }
            return uriString;
        }
    }


    public enum IncidentTypeSetOperation
    {
        UNION,
        INTERSECTION
    }
    public enum IncludeAid
    {
        ALL,
        GIVEN,
        RECEIVED
    }
}
