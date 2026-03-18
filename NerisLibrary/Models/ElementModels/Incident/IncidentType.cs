using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class IncidentType
    {
        public int? Neris_Uid { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWriting)]
        public DateTimeOffset Last_Modified { get; set; }
        public bool? Primary { get; set; } = null;
        public string Type { get; set; }
        [JsonIgnore]
        public List<IncidentTypeEnum> TypeList
        {
            get
            {
                return IncidentTypeUtil.GetTypesFromString(this.Type);
            }
        }
    }
    public static class IncidentTypeUtil
    {
        public static bool TryCreateIncidentTypeString(out string typeString, IncidentTypeEnum type1, IncidentTypeEnum? type2 = null, IncidentTypeEnum? type3 = null)
        {
            typeString = string.Empty;
            if (type2 == null && type3 != null) { throw new ArgumentException("Cannot supply Type 3 without Type 2"); }

            if (!IncidentType1.Contains(type1))
            {
                return false;
            }
            StringBuilder toOutput = new StringBuilder();
            toOutput.Append(type1.ToString());

            if (type2 != null)
            {
                IncidentTypeEnum _type2 = (IncidentTypeEnum)type2;
                if (!IncidentType2.Contains(_type2))
                {
                    return false;
                }
                toOutput.AppendFormat("||{0}", _type2.ToString());
            }
            if (type3 != null)
            {
                IncidentTypeEnum _type3 = (IncidentTypeEnum)type3;
                if (!IncidentType3.Contains(_type3))
                {
                    return false;
                }
                toOutput.AppendFormat("||{0}", _type3.ToString());
            }

            typeString = toOutput.ToString();
            return true;
        }

        public static List<IncidentTypeEnum> GetTypesFromString(string incidentTypeString)
        {
            List<IncidentTypeEnum> toReturn = new List<IncidentTypeEnum>();
            if (incidentTypeString == null) { return toReturn; }
            string[] substrings = incidentTypeString.Split("||");
            foreach (string typeString in substrings)
            {
                if (Enum.TryParse<IncidentTypeEnum>(typeString, out IncidentTypeEnum type)){
                    toReturn.Add(type);
                }
            }
            return toReturn;
        }

        public static readonly HashSet<IncidentTypeEnum> IncidentType1 = new HashSet<IncidentTypeEnum>
        {
            IncidentTypeEnum.FIRE,
            IncidentTypeEnum.HAZSIT,
            IncidentTypeEnum.MEDICAL,
            IncidentTypeEnum.NOEMERG,
            IncidentTypeEnum.PUBSERV,
            IncidentTypeEnum.RESCUE,
            IncidentTypeEnum.LAWENFORCE,
        };

        public static readonly HashSet<IncidentTypeEnum> IncidentType2 = new HashSet<IncidentTypeEnum>
        {
            IncidentTypeEnum.OUTSIDE_FIRE,
            IncidentTypeEnum.SPECIAL_FIRE,
            IncidentTypeEnum.STRUCTURE_FIRE,
            IncidentTypeEnum.TRANSPORTATION_FIRE,
            IncidentTypeEnum.HAZARD_NONCHEM,
            IncidentTypeEnum.HAZARDOUS_MATERIALS,
            IncidentTypeEnum.OVERPRESSURE,
            IncidentTypeEnum.INVESTIGATION,
            IncidentTypeEnum.ILLNESS,
            IncidentTypeEnum.INJURY,
            IncidentTypeEnum.OTHER,
            IncidentTypeEnum.CITIZEN_ASSIST,
            IncidentTypeEnum.ALARMS_NONMED,
            IncidentTypeEnum.DISASTER_WEATHER,
            IncidentTypeEnum.OUTSIDE,
            IncidentTypeEnum.STRUCTURE,
            IncidentTypeEnum.TRANSPORTATION,
            IncidentTypeEnum.WATER,
            IncidentTypeEnum.FALSE_ALARM,
            IncidentTypeEnum.GOOD_INTENT,
            IncidentTypeEnum.CANCELLED
        };

        public static readonly HashSet<IncidentTypeEnum> IncidentType3 = new HashSet<IncidentTypeEnum>
        {
            IncidentTypeEnum.CONSTRUCTION_WASTE, IncidentTypeEnum.OTHER_OUTSIDE_FIRE, IncidentTypeEnum.OUTSIDE_TANK_FIRE, IncidentTypeEnum.TRASH_RUBBISH_FIRE, IncidentTypeEnum.VEGETATION_GRASS_FIRE, IncidentTypeEnum.WILDFIRE_WILDLAND, IncidentTypeEnum.WILDFIRE_URBAN_INTERFACE,
            IncidentTypeEnum.UTILITY_INFRASTRUCTURE_FIRE, IncidentTypeEnum.DUMPSTER_OUTDOOR_CONTAINER_FIRE, IncidentTypeEnum.ESS_FIRE, IncidentTypeEnum.EXPLOSION, IncidentTypeEnum.INFRASTRUCTURE_FIRE, IncidentTypeEnum.STRUCTURAL_INVOLVEMENT_FIRE, IncidentTypeEnum.ROOM_AND_CONTENTS_FIRE,
            IncidentTypeEnum.CONFINED_COOKING_APPLIANCE_FIRE, IncidentTypeEnum.CHIMNEY_FIRE, IncidentTypeEnum.AIRCRAFT_FIRE, IncidentTypeEnum.VEHICLE_FIRE_PASSENGER, IncidentTypeEnum.VEHICLE_FIRE_COMMERCIAL, IncidentTypeEnum.VEHICLE_FIRE_RV, IncidentTypeEnum.VEHICLE_FIRE_FOOD_TRUCK,
            IncidentTypeEnum.BOAT_PERSONAL_WATERCRAFT_BARGE_FIRE, IncidentTypeEnum.POWERED_MOBILITY_DEVICE_FIRE, IncidentTypeEnum.TRAIN_RAIL_FIRE, IncidentTypeEnum.BOMB_THREAT_RESPONSE_SUSPICIOUS_PACKAGE, IncidentTypeEnum.ELEC_POWER_LINE_DOWN_ARCHING_MALFUNC,
            IncidentTypeEnum.ELEC_HAZARD_SHORT_CIRCUIT, IncidentTypeEnum.MOTOR_VEHICLE_COLLISION, IncidentTypeEnum.FUEL_SPILL_ODOR, IncidentTypeEnum.GAS_LEAK_ODOR, IncidentTypeEnum.CARBON_MONOXIDE_RELEASE, IncidentTypeEnum.BIOLOGICAL_RELEASE_INCIDENT,
            IncidentTypeEnum.RADIOACTIVE_RELEASE_INCIDENT, IncidentTypeEnum.HAZMAT_RELEASE_TRANSPORT, IncidentTypeEnum.HAZMAT_RELEASE_FACILITY, IncidentTypeEnum.RUPTURE_WITHOUT_FIRE, IncidentTypeEnum.NO_RUPTURE, IncidentTypeEnum.ODOR, IncidentTypeEnum.SMOKE_INVESTIGATION,
            IncidentTypeEnum.ABDOMINAL_PAIN, IncidentTypeEnum.ALLERGIC_REACTION_STINGS, IncidentTypeEnum.BACK_PAIN_NON_TRAUMA, IncidentTypeEnum.BREATHING_PROBLEMS, IncidentTypeEnum.CARDIAC_ARREST, IncidentTypeEnum.CHEST_PAIN_NON_TRAUMA, IncidentTypeEnum.CONVULSIONS_SEIZURES,
            IncidentTypeEnum.DIABETIC_PROBLEMS, IncidentTypeEnum.HEADACHE, IncidentTypeEnum.HEART_PROBLEMS, IncidentTypeEnum.OVERDOSE, IncidentTypeEnum.PANDEMIC_EPIDEMIC_OUTBREAK, IncidentTypeEnum.PREGNANCY_CHILDBIRTH, IncidentTypeEnum.PSYCHOLOGICAL_BEHAVIOR_ISSUES,
            IncidentTypeEnum.SICK_CASE, IncidentTypeEnum.STROKE_CVA, IncidentTypeEnum.UNCONSCIOUS_VICTIM, IncidentTypeEnum.WELL_PERSON_CHECK, IncidentTypeEnum.ALTERED_MENTAL_STATUS, IncidentTypeEnum.NAUSEA_VOMITING, IncidentTypeEnum.UNKNOWN_PROBLEM,
            IncidentTypeEnum.NO_APPROPRIATE_CHOICE, IncidentTypeEnum.ANIMAL_BITES, IncidentTypeEnum.ASSAULT, IncidentTypeEnum.BURNS_EXPLOSION, IncidentTypeEnum.CARBON_MONOXIDE_OTHER_INHALATION_INJURY, IncidentTypeEnum.CHOKING, IncidentTypeEnum.DROWNING_DIVING_SCUBA_ACCIDENT,
            IncidentTypeEnum.ELECTROCUTION, IncidentTypeEnum.EYE_TRAUMA, IncidentTypeEnum.FALL, IncidentTypeEnum.HEAT_COLD_EXPOSURE, IncidentTypeEnum.INDUSTRIAL_INACCESSIBLE_ENTRAPMENT, IncidentTypeEnum.POISONING, IncidentTypeEnum.GUNSHOT_WOUND,
            IncidentTypeEnum.HEMORRHAGE_LACERATION, IncidentTypeEnum.STAB_PENETRATING_TRAUMA, IncidentTypeEnum.OTHER_TRAUMATIC_INJURY, IncidentTypeEnum.HEALTHCARE_PROFESSIONAL_ADMISSION, IncidentTypeEnum.MEDICAL_ALARM, IncidentTypeEnum.STANDBY_REQUEST,
            IncidentTypeEnum.TRANSFER_INTERFACILITY, IncidentTypeEnum.AIRMEDICAL_TRANSPORT, IncidentTypeEnum.INTERCEPT_OTHER_UNIT, IncidentTypeEnum.COMMUNITY_PUBLIC_HEALTH, IncidentTypeEnum.LOST_PERSON, IncidentTypeEnum.PERSON_IN_DISTRESS,
            IncidentTypeEnum.CITIZEN_ASSIST_SERVICE_CALL, IncidentTypeEnum.LIFT_ASSIST, IncidentTypeEnum.FIRE_ALARM, IncidentTypeEnum.GAS_ALARM, IncidentTypeEnum.CO_ALARM, IncidentTypeEnum.OTHER_ALARM, IncidentTypeEnum.DAMAGE_ASSESSMENT,
            IncidentTypeEnum.WEATHER_RESPONSE, IncidentTypeEnum.MOVE_UP, IncidentTypeEnum.STANDBY, IncidentTypeEnum.DAMAGED_HYDRANT, IncidentTypeEnum.BACKCOUNTRY_RESCUE, IncidentTypeEnum.CONFINED_SPACE_RESCUE, IncidentTypeEnum.TRENCH,
            IncidentTypeEnum.EXTRICATION_ENTRAPPED, IncidentTypeEnum.HIGH_ANGLE_RESCUE, IncidentTypeEnum.LOW_ANGLE_RESCUE, IncidentTypeEnum.STEEP_ANGLE_RESCUE, IncidentTypeEnum.LIMITED_NO_ACCESS, IncidentTypeEnum.BUILDING_STRUCTURE_COLLAPSE,
            IncidentTypeEnum.ELEVATOR_ESCALATOR_RESCUE, IncidentTypeEnum.MOTOR_VEHICLE_EXTRICATION_ENTRAPPED, IncidentTypeEnum.TRAIN_RAIL_COLLISION_DERAILMENT, IncidentTypeEnum.AVIATION_COLLISION_CRASH, IncidentTypeEnum.AVIATION_STANDBY,
            IncidentTypeEnum.PERSON_IN_WATER_STANDING, IncidentTypeEnum.PERSON_IN_WATER_SWIFTWATER, IncidentTypeEnum.WATERCRAFT_IN_DISTRESS, IncidentTypeEnum.INTENTIONAL_FALSE_ALARM, IncidentTypeEnum.MALFUNCTIONING_ALARM,
            IncidentTypeEnum.ACCIDENTAL_ALARM, IncidentTypeEnum.OTHER_FALSE_CALL, IncidentTypeEnum.BOMB_SCARE, IncidentTypeEnum.NO_INCIDENT_FOUND_LOCATION_ERROR, IncidentTypeEnum.CONTROLLED_BURNING_AUTHORIZED, IncidentTypeEnum.SMOKE_FROM_NONHOSTILE_SOURCE, IncidentTypeEnum.INVESTIGATE_HAZARDOUS_RELEASE,
        };

    }
    public enum IncidentTypeEnum
    {
        FIRE,
        OUTSIDE_FIRE,
        CONSTRUCTION_WASTE,
        DUMPSTER_OUTDOOR_CONTAINER_FIRE,
        OTHER_OUTSIDE_FIRE,
        OUTSIDE_TANK_FIRE,
        TRASH_RUBBISH_FIRE,
        UTILITY_INFRASTRUCTURE_FIRE,
        VEGETATION_GRASS_FIRE,
        WILDFIRE_URBAN_INTERFACE,
        WILDFIRE_WILDLAND,
        SPECIAL_FIRE,
        ESS_FIRE,
        EXPLOSION,
        INFRASTRUCTURE_FIRE,
        STRUCTURE_FIRE,
        CHIMNEY_FIRE,
        CONFINED_COOKING_APPLIANCE_FIRE,
        ROOM_AND_CONTENTS_FIRE,
        STRUCTURAL_INVOLVEMENT_FIRE,
        TRANSPORTATION_FIRE,
        AIRCRAFT_FIRE,
        BOAT_PERSONAL_WATERCRAFT_BARGE_FIRE,
        POWERED_MOBILITY_DEVICE_FIRE,
        TRAIN_RAIL_FIRE,
        VEHICLE_FIRE_COMMERCIAL,
        VEHICLE_FIRE_FOOD_TRUCK,
        VEHICLE_FIRE_PASSENGER,
        VEHICLE_FIRE_RV,
        HAZSIT,
        HAZARDOUS_MATERIALS,
        BIOLOGICAL_RELEASE_INCIDENT,
        CARBON_MONOXIDE_RELEASE,
        FUEL_SPILL_ODOR,
        GAS_LEAK_ODOR,
        HAZMAT_RELEASE_FACILITY,
        HAZMAT_RELEASE_TRANSPORT,
        RADIOACTIVE_RELEASE_INCIDENT,
        HAZARD_NONCHEM,
        BOMB_THREAT_RESPONSE_SUSPICIOUS_PACKAGE,
        ELEC_HAZARD_SHORT_CIRCUIT,
        ELEC_POWER_LINE_DOWN_ARCHING_MALFUNC,
        MOTOR_VEHICLE_COLLISION,
        INVESTIGATION,
        ODOR,
        SMOKE_INVESTIGATION,
        OVERPRESSURE,
        NO_RUPTURE,
        RUPTURE_WITHOUT_FIRE,
        MEDICAL,
        ILLNESS,
        ABDOMINAL_PAIN,
        ALLERGIC_REACTION_STINGS,
        ALTERED_MENTAL_STATUS,
        BACK_PAIN_NON_TRAUMA,
        BREATHING_PROBLEMS,
        CARDIAC_ARREST,
        CHEST_PAIN_NON_TRAUMA,
        CONVULSIONS_SEIZURES,
        DIABETIC_PROBLEMS,
        HEADACHE,
        HEART_PROBLEMS,
        NAUSEA_VOMITING,
        NO_APPROPRIATE_CHOICE,
        OVERDOSE,
        PANDEMIC_EPIDEMIC_OUTBREAK,
        PREGNANCY_CHILDBIRTH,
        PSYCHOLOGICAL_BEHAVIOR_ISSUES,
        SICK_CASE,
        STROKE_CVA,
        UNCONSCIOUS_VICTIM,
        UNKNOWN_PROBLEM,
        WELL_PERSON_CHECK,
        INJURY,
        ANIMAL_BITES,
        ASSAULT,
        BURNS_EXPLOSION,
        CARBON_MONOXIDE_OTHER_INHALATION_INJURY,
        CHOKING,
        DROWNING_DIVING_SCUBA_ACCIDENT,
        ELECTROCUTION,
        EYE_TRAUMA,
        FALL,
        GUNSHOT_WOUND,
        HEAT_COLD_EXPOSURE,
        HEMORRHAGE_LACERATION,
        INDUSTRIAL_INACCESSIBLE_ENTRAPMENT,
        OTHER_TRAUMATIC_INJURY,
        POISONING,
        STAB_PENETRATING_TRAUMA,
        OTHER,
        AIRMEDICAL_TRANSPORT,
        COMMUNITY_PUBLIC_HEALTH,
        HEALTHCARE_PROFESSIONAL_ADMISSION,
        INTERCEPT_OTHER_UNIT,
        MEDICAL_ALARM,
        STANDBY_REQUEST,
        TRANSFER_INTERFACILITY,
        NOEMERG,
        CANCELLED,
        FALSE_ALARM,
        ACCIDENTAL_ALARM,
        BOMB_SCARE,
        INTENTIONAL_FALSE_ALARM,
        MALFUNCTIONING_ALARM,
        OTHER_FALSE_CALL,
        GOOD_INTENT,
        CONTROLLED_BURNING_AUTHORIZED,
        INVESTIGATE_HAZARDOUS_RELEASE,
        NO_INCIDENT_FOUND_LOCATION_ERROR,
        SMOKE_FROM_NONHOSTILE_SOURCE,
        PUBSERV,
        ALARMS_NONMED,
        CO_ALARM,
        FIRE_ALARM,
        GAS_ALARM,
        OTHER_ALARM,
        CITIZEN_ASSIST,
        CITIZEN_ASSIST_SERVICE_CALL,
        LIFT_ASSIST,
        LOST_PERSON,
        PERSON_IN_DISTRESS,
        DISASTER_WEATHER,
        DAMAGE_ASSESSMENT,
        WEATHER_RESPONSE,
        DAMAGED_HYDRANT,
        MOVE_UP,
        STANDBY,
        RESCUE,
        OUTSIDE,
        BACKCOUNTRY_RESCUE,
        CONFINED_SPACE_RESCUE,
        EXTRICATION_ENTRAPPED,
        HIGH_ANGLE_RESCUE,
        LIMITED_NO_ACCESS,
        LOW_ANGLE_RESCUE,
        STEEP_ANGLE_RESCUE,
        TRENCH,
        STRUCTURE,
        BUILDING_STRUCTURE_COLLAPSE,
        ELEVATOR_ESCALATOR_RESCUE,
        TRANSPORTATION,
        AVIATION_COLLISION_CRASH,
        AVIATION_STANDBY,
        MOTOR_VEHICLE_EXTRICATION_ENTRAPPED,
        TRAIN_RAIL_COLLISION_DERAILMENT,
        WATER,
        PERSON_IN_WATER_STANDING,
        PERSON_IN_WATER_SWIFTWATER,
        WATERCRAFT_IN_DISTRESS,
        UNDETERMINED,
        LAWENFORCE
    }


}
