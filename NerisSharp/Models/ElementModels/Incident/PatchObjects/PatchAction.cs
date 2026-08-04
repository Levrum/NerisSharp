using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.PatchObjects
{
    public class PatchAction<T>
    {
        private PatchAction(T value, ActionTypes action)
        {
            Value = value;
            Action = action;
        }
        public ActionTypes Action { get; private set; }
        private T _value;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Value
        {
            get
            {
                if (Action == ActionTypes.unset)
                {
                    return default;
                }
                return _value;
            }
            private set
            {
                _value = value;
            }
        }
        public static PatchAction<T> CreateSetAction(T setValue)
        {
            return new PatchAction<T>(setValue, ActionTypes.set);
        }

        public static PatchAction<T> CreateUnsetAction()
        {
            return new PatchAction<T>(default, ActionTypes.unset);
        }
    }

    public enum ActionTypes
    {
        append,
        remove,
        patch,
        set,
        unset,
    }
}
