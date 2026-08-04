using NerisSharp.Models.ElementModels.Incident.Modules;

namespace TestProject.RandomModules
{
    internal static class RandomOtherAlarmGenerator
    {
        public static OtherAlarm Generate(bool forcePresent = false)
        {
            OtherAlarm toReturn = new OtherAlarm();
            OtherAlarmPresence otherAlarmPresence = new OtherAlarmPresence();
            toReturn.Presence = otherAlarmPresence;

            otherAlarmPresence.Type = RandomModulesUtils.GetRandomPresence(forcePresent);

            if (otherAlarmPresence.Type == AlarmPresenceEnum.PRESENT) //READ OVER, VALIDATE, EXTRACT
            {
                otherAlarmPresence.Alarm_Types = RandomModulesUtils.GetRandomListEnumsOrNull<OtherAlarmTypeEnum>();
            }

            return toReturn;
        }
    }
}
