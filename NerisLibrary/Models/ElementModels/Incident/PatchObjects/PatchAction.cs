using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident.PatchObjects
{
    public class PatchAction<T>
    {
        private PatchAction(T value, ActionTypes action)
        {
            Value = value;
            Action = action;
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ActionTypes Action { get; private set; }
        private T _value;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Value { get
            {
                if (Action == ActionTypes.UNSET)
                {
                    return default(T);
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
            return new PatchAction<T>(setValue, ActionTypes.SET);
        }

        public static PatchAction<T> CreateUnsetAction()
        {
            return new PatchAction<T>(default(T), ActionTypes.UNSET);
        }
    }

    public enum ActionTypes
    {
        APPEND,
        REMOVE,
        PATCH,
        SET,
        UNSET,
    }
}
