using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels
{
    internal class AccessTokenModel
    {
        public string Access_Token { get; set; } = string.Empty;
        public string Refresh_Token { get; set; } = string.Empty;
        private int _expires_in;
        public int Expires_In { get {
                return _expires_in;
            } set 
            {
                _expires_in = value;
                expires_at = DateTime.UtcNow.AddSeconds(value);
            } 
        }

        [JsonIgnore]
        public DateTime expires_at { get; set; }
    }
}
