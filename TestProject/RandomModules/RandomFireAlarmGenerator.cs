using NerisSharp.Models.ElementModels.Incident.Modules;
using System;

namespace TestProject.RandomModules
{
    public static class RandomFireAlarmGenerator
    {
        public static FireAlarm Generate(bool forcePresent = false)
        {
            Random rng = new Random();

            FireAlarm toReturn = new FireAlarm(); //no fields required.
            FireAlarmPresence fireAlarmPresence = new FireAlarmPresence(); //has fields to add.
            toReturn.Presence = fireAlarmPresence;

            fireAlarmPresence.Type = RandomModulesUtils.GetRandomPresence(forcePresent);

            //Present Type has some extra fields:
            if (fireAlarmPresence.Type == AlarmPresenceEnum.PRESENT)
            {
                //alarm types
                fireAlarmPresence.Alarm_Types = RandomModulesUtils.GetRandomListEnumsOrNull<FireAlarmTypeEnum>();

                //operation type:
                bool hasOpType = RandomModulesUtils.TryGetRandomEnumOrNull(out FireAlarmOperationTypeEnum opType);
                fireAlarmPresence.Operation_Type = (hasOpType) ? opType : null;
            }

            //nothing needed if not present
            return toReturn;
        }
    }
}
