using NerisSharp.Models.ElementModels.Incident.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.RandomModules
{
    internal static class RandomSmokeAlarmGenerator
    {
        public static SmokeAlarm Generate(bool forcePresent = false)
        {
            SmokeAlarm toReturn = new SmokeAlarm();
            SmokeAlarmPresence presence = new SmokeAlarmPresence();
            toReturn.Presence = presence;
            presence.Type = RandomModulesUtils.GetRandomPresence(forcePresent);
            Random rng = new Random();

            if (presence.Type == AlarmPresenceEnum.PRESENT)
            {
                //working
                int workingValue = rng.Next(-1, 2);
                presence.Working = (workingValue < 0) ? null : workingValue == 1;

                //alarm types
                presence.Alarm_Types = RandomModulesUtils.GetRandomListEnumsOrNull<SmokeAlarmTypeEnum>();

                //operation
                SmokeAlarmOperation operation = null;
                bool operationTypePresent = RandomModulesUtils.TryGetRandomEnumOrNull(out SmokeAlarmOperationDetailTypeEnum operationType);
                if (operationTypePresent)
                {
                    operation = new SmokeAlarmOperation();
                    SmokeAlarmOperationDetail detail = new SmokeAlarmOperationDetail();
                    operation.Alerted_Failed_Other = detail;
                    detail.Type = operationType;
                    if (operationType == SmokeAlarmOperationDetailTypeEnum.OPERATED_ALERTED_OCCUPANT)
                    {
                        bool occupantActionPresent = RandomModulesUtils.TryGetRandomEnumOrNull(out OccupantActionEnum occupantActionEnum);
                        detail.Occupant_Action = (occupantActionPresent) ? occupantActionEnum : null;
                    } else if (operationType == SmokeAlarmOperationDetailTypeEnum.FAILED_TO_OPERATE)
                    {
                        bool failureReasonPresent = RandomModulesUtils.TryGetRandomEnumOrNull(out FailureReasonEnum failureReasonEnum);
                        detail.Failure_Reason = (failureReasonPresent) ? failureReasonEnum : null;
                    } else
                    {
                        //no op
                    }
                }
                presence.Operation = operation;
            }
            return toReturn;
        }
    }
}
