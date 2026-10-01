namespace NerisSharp.Models.ElementModels
{
    public class UnitModel
    {
        public int? Staffing { get; set; } = null;
        /// <summary>
        /// Whether the unit has dedicated staffing. The server defaults this to true. Left null here so a PATCH omits it
        /// unless the caller sets it explicitly.
        /// </summary>
        public bool? Dedicated_Staffing { get; set; } = null;
        public string Neris_Id { get; set; }
        public int? Version { get; set; } = null;
        public UnitTypes? Type { get; set; } = null;
        public string Cad_Designation_1 { get; set; }
        public string Cad_Designation_2 { get; set; }
        /// <summary>
        /// Whether the unit is in service. Set to false when a unit is taken offline for repair or
        /// moved seasonally so its NERIS id is preserved; not intended for real-time availability.
        /// The server defaults this to true. Left null here so a PATCH omits it unless the caller sets it explicitly.
        /// </summary>
        public bool? In_Service { get; set; } = null;
    }

    public enum UnitTypes
    {
        AIR_EMS,
        AIR_LIGHT,
        AIR_RECON,
        AIR_TANKER,
        ALS_AMB,
        ARFF,
        ATV_EMS,
        ATV_FIRE,
        BLS_AMB,
        BOAT,
        BOAT_LARGE,
        CHIEF_STAFF_COMMAND,
        CREW,
        CREW_TRANS,
        DECON,
        DOZER,
        EMS_NOTRANS,
        EMS_SUPV,
        ENGINE_STRUCT,
        ENGINE_WUI,
        FOAM,
        HAZMAT,
        HELO_FIRE,
        HELO_GENERAL,
        HELO_RESCUE,
        INVEST,
        LADDER_QUINT,
        LADDER_SMALL,
        LADDER_TALL,
        LADDER_TILLER,
        MAB,
        MOBILE_COMMS,
        MOBILE_ICP,
        OTHER_GROUND,
        PLATFORM,
        PLATFORM_QUINT,
        POV,
        QUINT_TALL,
        REHAB,
        RESCUE_HEAVY,
        RESCUE_LIGHT,
        RESCUE_MEDIUM,
        RESCUE_USAR,
        RESCUE_WATER,
        SCBA,
        TENDER,
        UAS_FIRE,
        UAS_RECON,
        UTIL
    }
}
