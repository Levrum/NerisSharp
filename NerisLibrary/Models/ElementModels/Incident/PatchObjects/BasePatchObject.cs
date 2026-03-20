using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident.PatchObjects
{
    public abstract class BasePatchObject
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ActionTypes Action { get; internal set; }
    }

    public class PatchObject<T> : BasePatchObject
    {
        public PatchObject()
        {
            Action = ActionTypes.PATCH;
        }
        public string Neris_Uid { get; set; }
        public T Properties { get; set; }
    }

    public class SetObject<T> : BasePatchObject
    {
        public SetObject()
        {
            Action = ActionTypes.SET;
        }
        public T Value { get; set; }
    }

    public class UnsetObject : BasePatchObject
    {
        public UnsetObject()
        {
            Action = ActionTypes.UNSET;
        }
    }
}
