using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NerisSharp.Models.ElementModels.Incident.Modules
{
    public class FireDetail
    {
        public LocationDetail? Location_Detail { get; set; }
        public WaterSupplyEnum Water_Supply { get; set; }
        public InvestigationNeededEnum Investigation_Needed { get; set; }
        public List<InvestigationTypeEnum> Investigation_Types { get; set; }
        public List<SuppressionAppliancesEnum> Suppression_Appliances { get; set; }
    }

    public enum WaterSupplyEnum
    {
        DRAFT_FROM_STATIC_SOURCE,
        FOAM_ADDITIVE,
        HYDRANT_GREATER_500,
        HYDRANT_LESS_500,
        NONE,
        NURSE_OTHER_APPARATUS,
        SUPPLY_FROM_FIRE_BOAT,
        TANK_WATER,
        WATER_TENDER_SHUTTLE
    }

    public enum InvestigationNeededEnum
    {
        NO,
        NOT_APPLICABLE,
        NOT_EVALUATED,
        NO_CAUSE_OBVIOUS,
        OTHER,
        YES,
    }

    public enum InvestigationTypeEnum
    {
        [Obsolete("Deprecated in NERIS 1.5; use INVESTIGATED_BY_FIRE_AND_EXPLOSION_INVESTIGATOR.")]
        INVESTIGATED_BY_ARSON_FIRE_INVESTIGATOR,
        INVESTIGATED_BY_FIRE_AND_EXPLOSION_INVESTIGATOR,
        INVESTIGATED_BY_INSURANCE,
        INVESTIGATED_BY_NONFIRE_LAW_ENFORCEMENT,
        INVESTIGATED_BY_OTHER,
        INVESTIGATED_BY_OUTSIDE_AGENCY,
        INVESTIGATED_BY_STATE_FIRE_MARSHAL,
        INVESTIGATED_ON_SCENE_RESOURCE,
        NONE,
    }

    public enum SuppressionAppliancesEnum
    {
        AERIAL_MASTER_STREAM,
        AIRATTACK_HELITACK,
        BOOSTER_FIRE_HOSE,
        BUILDING_FDC,
        BUILDING_STANDPIPE,
        ELEVATED_MASTER_STREAM_STANDPIPE,
        FIRE_EXTINGUISHER,
        GROUND_MONITOR,
        MASTER_STREAM,
        MEDIUM_DIAMETER_FIRE_HOSE,
        NONE,
        OTHER,
        SMALL_DIAMETER_FIRE_HOSE,
    }
}
