using NerisSharp.Models.ElementModels.Incident.PatchObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident
{
    /// <summary>
    /// Model for containing base information about the incident including Location and Narrative. Generally high level details.
    /// </summary>
    public class IncidentBase
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public int Neris_Uid { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public bool? People_Present { get; set; }
        public int? Animals_Rescued { get; set; }
        public string Impediment_Narrative { get; set; }
        public string Outcome_Narrative { get; set; }
        public int? Displacement_Count { get; set; }
        public string Department_Neris_Id { get; set; }
        public string Incident_Number { get; set; }
        public List<DisplacementCausesEnum> Displacement_Causes { get; set; }
        public LocationModel Location { get; set; }
        //location use
        public JsonNode Point { get; set; }

        /// <summary>
        /// Attempts to extract latitude and longitude coordinates from the current object's point data.
        /// </summary>
        /// <remarks>This method may thrown an exception if the point geojson format has changed. If so, contact David</remarks>
        /// <param name="latlong">When this method returns, contains the extracted latitude and longitude if the operation succeeds;
        /// otherwise, contains the default value.</param>
        /// <returns>true if the latitude and longitude were successfully extracted; otherwise, false.</returns>
        public bool TryGetLatLong(out LatLong latlong)
        {
            latlong = new LatLong();
            if (Point == null)
            {
                return false;
            }
            if (Point.GetValueKind() == JsonValueKind.Object)
            {
                //good
                JsonNode geometryNode = Point["geometry"];
                JsonNode coords = geometryNode["coordinates"];
                if (coords.GetValueKind() == JsonValueKind.Array)
                {
                    JsonArray coordArray = coords.AsArray();
                    latlong.Longitude = coordArray[0].GetValue<double>();
                    latlong.Latitude = coordArray[1].GetValue<double>();
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }
    }

    public class IncidentBaseModelPatchProperties
    {
        public PatchAction<bool> People_Present { get; set; }
        public PatchAction<int> Animals_Rescued { get; set; }
        public PatchAction<string> Impediment_Narrative { get; set; }
        public PatchAction<string> Outcome_Narrative { get; set; }
        public int? Displacement_Count { get; set; }
        public PatchAction<string> Department_Neris_Id { get; set; }
        public PatchAction<string> Incident_Number { get; set; }
        public PatchAction<List<string>> Displacement_Causes { get; set; }
        //public LocationModel Location { get; set; }
        //location use
        public PatchAction<string> Point { get; set; }
        public PatchAction<string> Polygon { get; set; }
    }

    public struct LatLong
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public enum DisplacementCausesEnum
    {
        COLLAPSE,
        FIRE,
        HAZARDOUS_SITUATION,
        OTHER,
        SMOKE,
        UTILITIES,
        WATER,
    }

}
