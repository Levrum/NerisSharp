using System.Collections.Generic;


namespace NerisLibrary.Models.ElementModels.Incident.Modules
{
    public class LocationDetail
    {
        public LocationDetailType Type { get; set; }

        /// <summary>
        /// Structure Fire Type Only
        /// </summary>
        public bool? Progression_Evident { get; set; }
        /// <summary>
        /// Structure Fire Type Only
        /// </summary>
        public int? Floor_Of_Origin { get; set; }

        /// <summary>
        /// Structure Fire Type Only
        /// </summary>
        public ArrivalConditionEnum? Arrival_Condition { get; set; }

        /// <summary>
        /// Structure Fire Type Only
        /// </summary>
        public DamageTypeEnum? Damage_Type { get; set; }

        /// <summary>
        /// Structure Fire Type Only
        /// </summary>
        public RoomOfOriginTypeEnum? Room_Of_Origin_Type { get; set; }

        /// <summary>
        /// Outside Fire Type Only
        /// </summary>
        public double? Acres_Burned { get; set; }

        /// <summary>
        /// Some causes may be exclusive to Structure or Outside fire type. You can verify using IsOutsideCause() or IsStructureCause()  
        /// </summary>
        public CauseEnum Cause { get; set; }

        private HashSet<CauseEnum> StructureCauses = new HashSet<CauseEnum>()
        {
            CauseEnum.ACT_OF_NATURE,
            CauseEnum.BATTERY_POWER_STORAGE,
            CauseEnum.CHEMICAL,
            CauseEnum.COOKING,
            CauseEnum.ELECTRICAL,
            CauseEnum.EXPLOSIVES_FIREWORKS,
            CauseEnum.HEAT_FROM_ANOTHER_OBJECT,
            CauseEnum.INCENDIARY,
            CauseEnum.OPEN_FLAME,
            CauseEnum.OPERATING_EQUIPMENT,
            CauseEnum.OTHER_HEAT_SOURCE,
            CauseEnum.SMOKING_MATERIALS_ILLICIT_DRUGS,
            CauseEnum.UNABLE_TO_BE_DETERMINED,
        };
        /// <summary>
        /// Determine whether a CauseEnum value is valid for a Structure Fire
        /// </summary>
        /// <param name="cause">CauseEnum value to test</param>
        /// <returns>True if valid for Structure Fire</returns>
        public bool IsStructureCause(CauseEnum cause) => StructureCauses.Contains(cause);

        private HashSet<CauseEnum> OutsideCauses = new HashSet<CauseEnum>()
        {
            CauseEnum.BATTERY_POWER_STORAGE,
            CauseEnum.DEBRIS_OPEN_BURNING,
            CauseEnum.EQUIPMENT_VEHICLE_USE,
            CauseEnum.FIREARMS_EXPLOSIVES,
            CauseEnum.FIREWORKS,
            CauseEnum.INCENDIARY,
            CauseEnum.NATURAL,
            CauseEnum.POWER_GEN_TRANS_DIST,
            CauseEnum.RAILROAD_OPS_MAINTENANCE,
            CauseEnum.RECREATION_CEREMONY,
            CauseEnum.SMOKING_MATERIALS_ILLICIT_DRUGS,
            CauseEnum.SPREAD_FROM_CONTROLLED_BURN,
            CauseEnum.STRUCTURE,
            CauseEnum.UNABLE_TO_BE_DETERMINED,
        };

        /// <summary>
        /// Determine whether a CauseEnum value is valid for an Outside Fire
        /// </summary>
        /// <param name="cause">CauseEnum value to test</param>
        /// <returns>True if valid for Outside Fire</returns>
        public bool IsOutsideCause(CauseEnum cause) => OutsideCauses.Contains(cause);

    }

    public enum LocationDetailType
    {
        STRUCTURE,
        OUTSIDE
    }

    public enum ArrivalConditionEnum
    {
        FIRE_OUT_UPON_ARRIVAL,
        FIRE_SPREAD_BEYOND_STRUCTURE,
        NO_SMOKE_FIRE_SHOWING,
        SMOKE_FIRE_SHOWING,
        SMOKE_SHOWING,
        STRUCTURE_INVOLVED,
    }

    public enum DamageTypeEnum
    {
        MAJOR_DAMAGE,
        MINOR_DAMAGE,
        MODERATE_DAMAGE,
        NO_DAMAGE,
    }

    public enum RoomOfOriginTypeEnum
    {
        ASSEMBLY,
        ATTIC,
        BALCONY_PORCH_DECK,
        BASEMENT,
        BATHROOM,
        BEDROOM,
        GARAGE,
        HALLWAY_FOYER,
        KITCHEN,
        LIVING_SPACE,
        OFFICE,
        OTHER,
        UNKNOWN,
        UTILITY_ROOM,
    }

    public enum CauseEnum
    {
        ACT_OF_NATURE,
        BATTERY_POWER_STORAGE,
        CHEMICAL,
        COOKING,
        ELECTRICAL,
        EXPLOSIVES_FIREWORKS,
        HEAT_FROM_ANOTHER_OBJECT,
        INCENDIARY,
        OPEN_FLAME,
        OPERATING_EQUIPMENT,
        OTHER_HEAT_SOURCE,
        SMOKING_MATERIALS_ILLICIT_DRUGS,
        UNABLE_TO_BE_DETERMINED,
        DEBRIS_OPEN_BURNING,
        EQUIPMENT_VEHICLE_USE,
        FIREARMS_EXPLOSIVES,
        FIREWORKS,
        NATURAL,
        POWER_GEN_TRANS_DIST,
        RAILROAD_OPS_MAINTENANCE,
        RECREATION_CEREMONY,
        SPREAD_FROM_CONTROLLED_BURN,
        STRUCTURE,
    }
}
