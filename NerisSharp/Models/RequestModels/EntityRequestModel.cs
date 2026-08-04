using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NerisSharp.Models.RequestModels
{
    public class EntityRequestModel : IRequestModel
    {
        public string Name { get; set; }
        public string Neris_id { get; set; }
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
        public EntityClassTypes? Entity_Class_Enum { get; set; } = null;
        public string Entity_Class
        {
            get
            {
                return Entity_Class_Enum?.ToString() ?? null;
            }
        }
        public EntitySubtypes? Entity_Subtype_Enum { get; set; } = null;
        public string Entity_Subtype
        {
            get
            {
                return Entity_Subtype_Enum?.ToString() ?? null;
            }
        }
        public string Last_Modified { get; set; }
        public int Page_Number { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
        public string Sort_by { get; set; }
        public bool Sort_Ascending { get; set; } = true;

        public enum EntityClassTypes
        {
            FEDERAL_AGENCY,
            FIRE_DEPARTMENT,
            FIRE_MARSHAL
        }

        public enum EntitySubtypes
        {
            CONTRACT,
            FEDERAL,
            LOCAL,
            OTHER,
            PRIVATE,
            STATE,
            TRANSPORTATION,
            TRIBAL
        }

        public bool Validate()
        {
            //check: state is valid (2 characters), last modifed string?, Sort_By and Sort_Order are valid. 
            bool valid = true;
            if (this.State != null && this.State.Length != 2)
            {
                valid = false;
            }

            //last modified = too hard
            if (this.Sort_by != null && !SortByValues.Contains(this.Sort_by))
            {
                valid = false;
            }

            return valid;
        }

        public HashSet<string> SortByValues { get; } = new HashSet<string>()
        {
            "name",
            "neris_id",
            "address_line_1",
            "city",
            "state",
            "zip_code",
            "department_type",
            "website",
            "last_modified"
        };

        HashSet<string> excludedFields = new HashSet<string>()
        {
            "Entity_Class_Enum",
            "Entity_Subtype_Enum",
            "Sort_Ascending",
            "SortByValues"
        };

        //Method to add to URI
        //uses reflection to add each property minus those excluded into a dictonary to build the query
        public string CreateQueryURI(string baseUri)
        {
            Dictionary<string, string> queryValues = new Dictionary<string, string>();
            //add sort:
            queryValues.Add("sort_direction", this.Sort_Ascending ? "ASCENDING" : "DESCENDING");

            //add the rest through reflection:
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                //skip excluded:
                if (excludedFields.Contains(property.Name))
                {
                    continue;
                }

                //only use string or int properties
                if (property.PropertyType == typeof(string) || property.PropertyType == typeof(int))
                {
                    string name = property.Name.ToLower();
                    object value = property.GetValue(this);
                    if (value == null) { continue; }
                    queryValues.Add(name, value.ToString());
                }
            }

            string uriString = QueryHelpers.AddQueryString(baseUri, queryValues);
            return uriString;
        }
    }
}
