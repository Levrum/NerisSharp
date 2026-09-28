using System;
using System.Collections.Generic;
using System.Text;

namespace NerisSharp.Models
{
    public struct LatLong
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public override string ToString()
        {
            return string.Format("{0},{1}", Longitude, Latitude);
        }
        public override bool Equals(object obj)
        {
            return obj is LatLong && Equals((LatLong)obj);
        }
        public bool Equals(LatLong other) 
        {
            return (Longitude == other.Longitude && Latitude == other.Latitude);
        }
    }
}
