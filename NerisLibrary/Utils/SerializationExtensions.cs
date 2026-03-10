using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
   using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NerisLibrary.Utils
{
    public static class SerializationExtensions
    {
        static JsonSerializerOptions PropertyCaseInsensitive = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        static JsonSerializerOptions LowerCaseNamingPolicy = new JsonSerializerOptions() 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, 
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull 
        };
        public static async Task<T> DeserializeCaseInsensitive<T>(this HttpContent response)
        {
            return await response.ReadFromJsonAsync<T>(PropertyCaseInsensitive);
        }

        public static string SerializeLowerCase<T>(T value)
        {
            string result = JsonSerializer.Serialize(value, LowerCaseNamingPolicy);
            return result;
        }
    }
}
