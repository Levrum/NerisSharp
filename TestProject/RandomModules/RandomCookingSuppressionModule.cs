using NerisSharp.Models.ElementModels.Incident.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.RandomModules
{
    internal static class RandomCookingSuppressionModule
    {
        public static CookingFireSuppression Generate()
        {
            CookingFireSuppression cfs = new CookingFireSuppression();
            CookingFireSuppressionPresence presence = new CookingFireSuppressionPresence();
            cfs.Presence = presence;
            presence.Type = RandomModulesUtils.GetRandomPresence();
            if (presence.Type == AlarmPresenceEnum.PRESENT)
            {
                presence.Suppression_Types = RandomModulesUtils.GetRandomListEnumsOrNull<CookingFireSuppressionTypeEnum>();

                bool operationTypePresent = RandomModulesUtils.TryGetRandomEnumOrNull<CookingFireOperationTypeEnum>(out var optype);
                if (operationTypePresent)
                {
                    presence.Operation_Type = optype;
                }
            }
            return cfs;
        }
    }
}
