using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NerisLibrary.Utils
{
    internal static class SerializationExtensions
    {
        static JsonSerializerOptions PropertyCaseInsensitive = new JsonSerializerOptions() //reading policy
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        static JsonSerializerOptions LowerCaseNamingPolicy = new JsonSerializerOptions() //Writing policy
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        //used for deserializing responses
        public static async Task<T> DeserializeCaseInsensitive<T>(this HttpContent response)
        {
            return await response.ReadFromJsonAsync<T>(PropertyCaseInsensitive);
        }

        //used for serializing payloads
        public static string SerializeLowerCase<T>(T value)
        {
            string result = JsonSerializer.Serialize(value, LowerCaseNamingPolicy);
            return result;
        }

        //used for serializing updates that need to be edited after serialization (removing ids, nulling fields in json).
        public static JsonObject SerializeToJsonObjectLowerCase<T>(T value)
        {
            JsonNode result = JsonSerializer.SerializeToNode(value, LowerCaseNamingPolicy);
            return result.AsObject();
        }

    }
}
