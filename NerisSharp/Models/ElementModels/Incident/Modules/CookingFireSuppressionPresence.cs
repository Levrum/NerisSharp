using System.Collections.Generic;

namespace NerisSharp.Models.ElementModels.Incident.Modules
{
    public class CookingFireSuppressionPresence
    {
        public AlarmPresenceEnum Type { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public List<CookingFireSuppressionTypeEnum>? Suppression_Types { get; set; }

        /// <summary>
        /// Present Type Only
        /// </summary>
        public CookingFireOperationTypeEnum? Operation_Type { get; set; }
    }

    public enum CookingFireSuppressionTypeEnum
    {
        COMMERCIAL_HOOD_SUPPRESSION,
        ELECTRIC_POWER_CUTOFF_DEVICE,
        OTHER,
        RESIDENTIAL_HOOD_MOUNTED,
        TEMPERATURE_LIMITING_STOVE,
    }

    public enum CookingFireOperationTypeEnum
    {
        NO_OPERATION,
        OPERATED_EFFECTIVE,
        OPERATED_NOT_EFFECTIVE,
    }
}
