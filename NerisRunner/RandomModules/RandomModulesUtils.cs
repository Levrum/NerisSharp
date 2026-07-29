using NerisLibrary.Models.ElementModels.Incident.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NerisRunner.RandomModules
{
    internal static class RandomModulesUtils
    {
        public static AlarmPresenceEnum GetRandomPresence(bool forcePresent = false)
        {
            Random rng = new Random();
            int numberOfPresenceTypes = Utils.GetEnumValueCount<AlarmPresenceEnum>();
            int alarmPresenceType = (forcePresent ? 0 : rng.Next(numberOfPresenceTypes));
            return (AlarmPresenceEnum)alarmPresenceType;
        }

        public static List<T> GetRandomListEnumsOrNull<T>()
        {
            Random rng = new Random();
            Type t = typeof(T);
            if (!t.IsEnum) throw new ArgumentException("Type must be Enum");

            int numberOfEnumValues = Utils.GetEnumValueCount<T>();

            int numberOfValuesToAdd = rng.Next(numberOfEnumValues + 1) - 1;
            if (numberOfValuesToAdd < 0)
            {
                return null;
            }
            else
            {
                List<T> values = new List<T>();
                for (int i = 0; i < numberOfValuesToAdd; i++)
                {
                    values.Add((T)Enum.ToObject(t, rng.Next(numberOfEnumValues)));
                }
                return values;
            }
        }

        public static T GetRandomEnum<T>() where T : Enum
        {
            Random rng = new Random();
            int numberOfEnumValues = Utils.GetEnumValueCount<T>();
            int enumValue = rng.Next(numberOfEnumValues);
            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        public static bool TryGetRandomEnumOrNull<T>(out T result, bool excludeNull = false) where T : Enum
        {
            Random rng = new Random();
            Type t = typeof(T);
            result = default(T);
            int numberOfEnumValues = Utils.GetEnumValueCount<T>();
            int toAdd = (excludeNull) ? 0 : 1;
            int enumVaue = rng.Next(numberOfEnumValues + toAdd);
            if (enumVaue == numberOfEnumValues)
            {
                return false;
            }
            else
            {
                
                result = (T)Enum.ToObject(t, rng.Next(numberOfEnumValues));
                return true;
            }
        }

        public static string GetRandomGuidStringOrNull()
        {
            Random rng = new Random();
            int result = rng.Next(10);
            if (result < 9)
            {
                return Guid.NewGuid().ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
