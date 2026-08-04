using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.PatchObjects
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(PatchObject))]
    [JsonDerivedType(typeof(SetObject))]
    [JsonDerivedType(typeof(UnsetObject))]
    public abstract class BasePatchObject
    {
        public ActionTypes Action { get; internal set; }
    }

    public class PatchObject : BasePatchObject
    {
        public PatchObject()
        {
            Action = ActionTypes.patch;
        }
        public int Neris_Uid { get; set; }
        public object Properties { get; set; }
    }

    public class SetObject : BasePatchObject
    {
        public SetObject()
        {
            Action = ActionTypes.set;
        }
        public object Value { get; set; }
    }

    public class UnsetObject : BasePatchObject
    {
        public UnsetObject()
        {
            Action = ActionTypes.unset;
        }
    }
}
