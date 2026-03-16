using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.ElementModels.Incident
{
    public class IncidentTypeUtil
    {
        public bool TryCreateIncidentTypeString(out string typeString, IncidentType type1, IncidentType? type2 = null, IncidentType? type3 = null)
        {
            typeString = string.Empty;
            if (type2 == null && type3 != null) { throw new ArgumentException("Cannot supply Type 3 without Type 2"); }

            if (!IncidentType1.Contains(type1))
            {
                return false;
            }
            StringBuilder toOutput = new StringBuilder();
            toOutput.Append(IncidentType1.ToString());

            if (type2 != null)
            {
                IncidentType _type2 = (IncidentType)type2;
                if (!IncidentType2.Contains(_type2))
                {
                    return false;
                }
                toOutput.AppendJoin("||", _type2.ToString());
            }
            if (type3 != null)
            {
                IncidentType _type3 = (IncidentType)type3;
                if (!IncidentType2.Contains(_type3))
                {
                    return false;
                }
                toOutput.AppendJoin("||", _type3.ToString());
            }

            typeString = toOutput.ToString();
            return true;
        }

        public List<IncidentType> GetTypesFromString(string incidentTypeString)
        {
            List<IncidentType> toReturn = new List<IncidentType>();
            string[] substrings = incidentTypeString.Split("||");
            foreach (string typeString in substrings)
            {
                if (Enum.TryParse<IncidentType>(typeString, out IncidentType type)){
                    toReturn.Add(type);
                }
            }
            return toReturn;
        }

        public readonly HashSet<IncidentType> IncidentType1 = new HashSet<IncidentType>
        {
            IncidentType.FIRE,
            IncidentType.HAZSIT,
            IncidentType.MEDICAL,
            IncidentType.NOEMERG,
            IncidentType.PUBSERV,
            IncidentType.RESCUE,
            IncidentType.LAWENFORCE,
        };

        public readonly HashSet<IncidentType> IncidentType2 = new HashSet<IncidentType>
        {
            IncidentType.OUTSIDE_FIRE,
            IncidentType.SPECIAL_FIRE,
            IncidentType.STRUCTURE_FIRE,
            IncidentType.TRANSPORTATION_FIRE,
            IncidentType.HAZARD_NONCHEM,
            IncidentType.HAZARDOUS_MATERIALS,
            IncidentType.OVERPRESSURE,
            IncidentType.INVESTIGATION,
            IncidentType.ILLNESS,
            IncidentType.INJURY,
            IncidentType.OTHER,
            IncidentType.CITIZEN_ASSIST,
            IncidentType.ALARMS_NONMED,
            IncidentType.DISASTER_WEATHER,
            IncidentType.OUTSIDE,
            IncidentType.STRUCTURE,
            IncidentType.TRANSPORTATION,
            IncidentType.WATER,
            IncidentType.FALSE_ALARM,
            IncidentType.GOOD_INTENT,
            IncidentType.CANCELLED
        };

        public readonly HashSet<IncidentType> IncidentType3 = new HashSet<IncidentType>
        {
            IncidentType.CONSTRUCTION_WASTE, IncidentType.OTHER_OUTSIDE_FIRE, IncidentType.OUTSIDE_TANK_FIRE, IncidentType.TRASH_RUBBISH_FIRE, IncidentType.VEGETATION_GRASS_FIRE, IncidentType.WILDFIRE_WILDLAND, IncidentType.WILDFIRE_URBAN_INTERFACE,
            IncidentType.UTILITY_INFRASTRUCTURE_FIRE, IncidentType.DUMPSTER_OUTDOOR_CONTAINER_FIRE, IncidentType.ESS_FIRE, IncidentType.EXPLOSION, IncidentType.INFRASTRUCTURE_FIRE, IncidentType.STRUCTURAL_INVOLVEMENT_FIRE, IncidentType.ROOM_AND_CONTENTS_FIRE,
            IncidentType.CONFINED_COOKING_APPLIANCE_FIRE, IncidentType.CHIMNEY_FIRE, IncidentType.AIRCRAFT_FIRE, IncidentType.VEHICLE_FIRE_PASSENGER, IncidentType.VEHICLE_FIRE_COMMERCIAL, IncidentType.VEHICLE_FIRE_RV, IncidentType.VEHICLE_FIRE_FOOD_TRUCK,
            IncidentType.BOAT_PERSONAL_WATERCRAFT_BARGE_FIRE, IncidentType.POWERED_MOBILITY_DEVICE_FIRE, IncidentType.TRAIN_RAIL_FIRE, IncidentType.BOMB_THREAT_RESPONSE_SUSPICIOUS_PACKAGE, IncidentType.ELEC_POWER_LINE_DOWN_ARCHING_MALFUNC,
            IncidentType.ELEC_HAZARD_SHORT_CIRCUIT, IncidentType.MOTOR_VEHICLE_COLLISION, IncidentType.FUEL_SPILL_ODOR, IncidentType.GAS_LEAK_ODOR, IncidentType.CARBON_MONOXIDE_RELEASE, IncidentType.BIOLOGICAL_RELEASE_INCIDENT,
            IncidentType.RADIOACTIVE_RELEASE_INCIDENT, IncidentType.HAZMAT_RELEASE_TRANSPORT, IncidentType.HAZMAT_RELEASE_FACILITY, IncidentType.RUPTURE_WITHOUT_FIRE, IncidentType.NO_RUPTURE, IncidentType.ODOR, IncidentType.SMOKE_INVESTIGATION,
            IncidentType.ABDOMINAL_PAIN, IncidentType.ALLERGIC_REACTION_STINGS, IncidentType.BACK_PAIN_NON_TRAUMA, IncidentType.BREATHING_PROBLEMS, IncidentType.CARDIAC_ARREST, IncidentType.CHEST_PAIN_NON_TRAUMA, IncidentType.CONVULSIONS_SEIZURES,
            IncidentType.DIABETIC_PROBLEMS, IncidentType.HEADACHE, IncidentType.HEART_PROBLEMS, IncidentType.OVERDOSE, IncidentType.PANDEMIC_EPIDEMIC_OUTBREAK, IncidentType.PREGNANCY_CHILDBIRTH, IncidentType.PSYCHOLOGICAL_BEHAVIOR_ISSUES,
            IncidentType.SICK_CASE, IncidentType.STROKE_CVA, IncidentType.UNCONSCIOUS_VICTIM, IncidentType.WELL_PERSON_CHECK, IncidentType.ALTERED_MENTAL_STATUS, IncidentType.NAUSEA_VOMITING, IncidentType.UNKNOWN_PROBLEM,
            IncidentType.NO_APPROPRIATE_CHOICE, IncidentType.ANIMAL_BITES, IncidentType.ASSAULT, IncidentType.BURNS_EXPLOSION, IncidentType.CARBON_MONOXIDE_OTHER_INHALATION_INJURY, IncidentType.CHOKING, IncidentType.DROWNING_DIVING_SCUBA_ACCIDENT,
            IncidentType.ELECTROCUTION, IncidentType.EYE_TRAUMA, IncidentType.FALL, IncidentType.HEAT_COLD_EXPOSURE, IncidentType.INDUSTRIAL_INACCESSIBLE_ENTRAPMENT, IncidentType.POISONING, IncidentType.GUNSHOT_WOUND,
            IncidentType.HEMORRHAGE_LACERATION, IncidentType.STAB_PENETRATING_TRAUMA, IncidentType.OTHER_TRAUMATIC_INJURY, IncidentType.HEALTHCARE_PROFESSIONAL_ADMISSION, IncidentType.MEDICAL_ALARM, IncidentType.STANDBY_REQUEST,
            IncidentType.TRANSFER_INTERFACILITY, IncidentType.AIRMEDICAL_TRANSPORT, IncidentType.INTERCEPT_OTHER_UNIT, IncidentType.COMMUNITY_PUBLIC_HEALTH, IncidentType.LOST_PERSON, IncidentType.PERSON_IN_DISTRESS,
            IncidentType.CITIZEN_ASSIST_SERVICE_CALL, IncidentType.LIFT_ASSIST, IncidentType.FIRE_ALARM, IncidentType.GAS_ALARM, IncidentType.CO_ALARM, IncidentType.OTHER_ALARM, IncidentType.DAMAGE_ASSESSMENT,
            IncidentType.WEATHER_RESPONSE, IncidentType.MOVE_UP, IncidentType.STANDBY, IncidentType.DAMAGED_HYDRANT, IncidentType.BACKCOUNTRY_RESCUE, IncidentType.CONFINED_SPACE_RESCUE, IncidentType.TRENCH,
            IncidentType.EXTRICATION_ENTRAPPED, IncidentType.HIGH_ANGLE_RESCUE, IncidentType.LOW_ANGLE_RESCUE, IncidentType.STEEP_ANGLE_RESCUE, IncidentType.LIMITED_NO_ACCESS, IncidentType.BUILDING_STRUCTURE_COLLAPSE,
            IncidentType.ELEVATOR_ESCALATOR_RESCUE, IncidentType.MOTOR_VEHICLE_EXTRICATION_ENTRAPPED, IncidentType.TRAIN_RAIL_COLLISION_DERAILMENT, IncidentType.AVIATION_COLLISION_CRASH, IncidentType.AVIATION_STANDBY,
            IncidentType.PERSON_IN_WATER_STANDING, IncidentType.PERSON_IN_WATER_SWIFTWATER, IncidentType.WATERCRAFT_IN_DISTRESS, IncidentType.INTENTIONAL_FALSE_ALARM, IncidentType.MALFUNCTIONING_ALARM,
            IncidentType.ACCIDENTAL_ALARM, IncidentType.OTHER_FALSE_CALL, IncidentType.BOMB_SCARE, IncidentType.NO_INCIDENT_FOUND_LOCATION_ERROR, IncidentType.CONTROLLED_BURNING_AUTHORIZED, IncidentType.SMOKE_FROM_NONHOSTILE_SOURCE, IncidentType.INVESTIGATE_HAZARDOUS_RELEASE,
        };

    }
    public enum IncidentType
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
