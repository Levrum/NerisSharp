using NerisSharp.Models.ElementModels.Incident.Modules;
using System;
using System.Collections.Generic;

namespace TestProject.RandomModules
{
    internal static class RandomFireSuppressionGenerator
    {
        public static FireSuppression Generate(bool forcePresent = false)
        {
            FireSuppression toReturn = new FireSuppression();
            FireSuppressionPresence presence = new FireSuppressionPresence();
            toReturn.Presence = presence;
            Random rng = new Random();

            presence.Type = RandomModulesUtils.GetRandomPresence(forcePresent);
            if (presence.Type == AlarmPresenceEnum.PRESENT)
            {
                //add suppression types
                List<FireSuppressionTypeEnum> typesPresent = RandomModulesUtils.GetRandomListEnumsOrNull<FireSuppressionTypeEnum>();
                if (typesPresent != null) //if null, leave null on presence object
                {
                    List<FireSuppressionType> suppressionTypes = new List<FireSuppressionType>();
                    foreach (FireSuppressionTypeEnum type in typesPresent)
                    {
                        bool fullPartialPresent = RandomModulesUtils.TryGetRandomEnumOrNull(out FullPartialEnum fullPartial);
                        suppressionTypes.Add(new FireSuppressionType()
                        {
                            Type = type,
                            Full_Partial = (fullPartialPresent) ? fullPartial : null
                        });
                    }
                    presence.Suppression_Types = suppressionTypes;
                }

                //add operation types
                FireSuppressionOperation operation = null;
                bool effectivenessPresent = RandomModulesUtils.TryGetRandomEnumOrNull(out FireSuppressionEffectivenessTypeEnum effectivenessType);
                if (effectivenessPresent)
                {
                    FireSuppressionEffectiveness effectiveness = new FireSuppressionEffectiveness()
                    {
                        Type = effectivenessType
                    };
                    operation = new FireSuppressionOperation() { Effectiveness = effectiveness };
                    int numActivate = rng.Next(-2, 11);
                    bool failureReasonPresent = RandomModulesUtils.TryGetRandomEnumOrNull(out FireSuppressionFailureReasonEnum failureReason);
                    switch (effectivenessType)
                    {
                        case FireSuppressionEffectivenessTypeEnum.OPERATED_EFFECTIVE:
                            effectiveness.Sprinklers_Activated = (numActivate < 0) ? null : numActivate;
                            break;
                        case FireSuppressionEffectivenessTypeEnum.OPERATED_NOT_EFFECTIVE:
                            effectiveness.Sprinklers_Activated = (numActivate < 0) ? null : numActivate;
                            effectiveness.Failure_Reason = (failureReasonPresent) ? failureReason : null;
                            break;
                        case FireSuppressionEffectivenessTypeEnum.NO_OPERATION:
                            effectiveness.Failure_Reason = (failureReasonPresent) ? failureReason : null;
                            break;
                    }
                }
                presence.Operation_Type = operation;
            }
            return toReturn;
        }
    }
}
