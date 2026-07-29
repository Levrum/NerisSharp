using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Models.ElementModels.Incident.Modules;
using NerisLibrary.Models.ElementModels.Incident.PatchObjects;
using NerisLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerisRunner
{
    internal static class SampleIncidentFactory
    {
        private static string CreateIncidentNumber() 
        {
            var now = DateTime.Now;
            string number = String.Format("{0}{1}{2}-{3}", now.Month, now.Day, now.Hour, now.Millisecond);
            return number;
        }
        public static IncidentModelPayload CreateSampleIncident(IncidentType incidentType)
        {
            //IncidentType iType = new IncidentType(IncidentTypeEnum.MEDICAL,
            //    IncidentTypeEnum.INJURY, IncidentTypeEnum.FALL)
            //{
            //    Primary = true
            //};
            incidentType.Primary = true;

            string incidentNumber = CreateIncidentNumber();

            LocationModel locationModel = new LocationModel()
            {
                Place_Type = "HOTEL",
                State = "OH",
                Postal_Code = "45504",
                Complete_Number = "1870",
                Street_Prefix_Direction = "W",
                Street = "1st",
                Street_Postfix = "STREET",
                Additional_Info = "SAMPLE INCIDENT DID NOT OCCUR"
            };
            //Console.WriteLine(locationModel.GetStreetAddress());

            IncidentBase modelBase = new IncidentBase()
            {
                People_Present = true,
                Outcome_Narrative = "SAMPLE Subject taken to hospital by EMS unit with minor injuries",
                Location = locationModel,
                Department_Neris_Id = "FD39023168",
                Incident_Number = incidentNumber
            };

            DispatchModel dispatch = new DispatchModel()
            {
                Center_Id = "1122",
                Incident_Number = incidentNumber,
                Automatic_Alarm = false,
                Call_Arrival = DateTimeOffset.UtcNow.AddMinutes(-10),
                Call_Answered = DateTimeOffset.UtcNow.AddMinutes(-9),
                Call_Create = DateTimeOffset.UtcNow.AddMinutes(-4),
                Location = locationModel,
                Unit_Responses = new List<UnitResponse>()
            };



            IncidentModelPayload model = new IncidentModelPayload()
            {
                Incident_Types = new List<IncidentType>() { incidentType },
                Base = modelBase,
                Dispatch = dispatch,
            };

            return model;

        }

        public static UnitResponse CreateSampleResponse()
        {
            DateTimeOffset baseTime = DateTimeOffset.Parse("2026-03-18 18:48:59-04:00");
            string unitId = "FD39023168S001U001";
            UnitResponse unitResponse = new UnitResponse()
            {
                Unit_Neris_Id = unitId, //test if required
                Reported_Unit_Id = "EMS1", //test if required
                Dispatch = baseTime,
                Enroute_To_Scene = baseTime.AddMinutes(2),
                On_Scene = baseTime.AddMinutes(10),
                Unit_Clear = baseTime.AddMinutes(45),
                Response_Mode = ResponseMode.EMERGENT,
                Transport_Mode = ResponseMode.NON_EMERGENT
            };

            MedReponse medResonse = new MedReponse()
            {
                Hospital_Destination = "Springfield Hospital",
                At_Patient = unitResponse.On_Scene,
                Enroute_To_Hospital = baseTime.AddMinutes(25),
                Arrived_At_Hospital = baseTime.AddMinutes(40),
                Hospital_Cleared = baseTime.AddMinutes(45)
            };
            unitResponse.Med_Responses = new List<MedReponse> { medResonse };
            return unitResponse;
        }

        public static FireDetail CreateFireDetail()
        {
            FireDetail fd = new FireDetail();
            fd.Water_Supply = WaterSupplyEnum.TANK_WATER;
            fd.Suppresion_Appliances = new List<SuppressionAppliancesEnum>() { SuppressionAppliancesEnum.MEDIUM_DIAMETER_FIRE_HOSE, SuppressionAppliancesEnum.SMALL_DIAMETER_FIRE_HOSE };
            fd.Investigation_Needed = InvestigationNeededEnum.NO;
            fd.Investigation_Types = new List<InvestigationTypeEnum>() { InvestigationTypeEnum.INVESTIGATED_ON_SCENE_RESOURCE };
            LocationDetail locationDetail = new LocationDetail()
            {
                Type = LocationDetailType.STRUCTURE,
                Progression_Evident = false,
                Floor_Of_Origin = 1,
                Arrival_Condition = ArrivalConditionEnum.SMOKE_SHOWING,
                Room_Of_Origin_Type = RoomOfOriginTypeEnum.KITCHEN,
                Cause = CauseEnum.COOKING,
                Damage_Type = DamageTypeEnum.MINOR_DAMAGE
            };
            fd.Location_Detail = locationDetail;
            return fd;
        }

        public static IncidentAid CreateAid()
        {
            IncidentAid aid = new IncidentAid()
            {
                Aid_Direction = AidDirectionEnum.RECEIVED,
                Aid_Type = AidTypeEnum.SUPPORT_AID,
                Department_Neris_Id = "FD24027214"
            };
            return aid;
        }

        public static IncidentPatchPayload CreateSamplePatch(string incidentId, int incidentBaseNerisUid)
        {
            //create top level incident patch properties:
            IncidentPatchProperties patchProps = new IncidentPatchProperties();

            //setup outcome narrative patch
            IncidentBaseModelPatchProperties baseModelProps = new IncidentBaseModelPatchProperties();
            baseModelProps.Outcome_Narrative = PatchAction<string>.CreateSetAction("PATCHED OUTCOME: Subject taken to hospital with non-critical injures");
            
            //create patch object:
            PatchObject basePatch = new PatchObject() { Properties = baseModelProps, Neris_Uid = incidentBaseNerisUid};
            //assign to incident patchProps:
            patchProps.Base = basePatch;

            //set a new tactics timestamp
            DateTimeOffset baseTime = DateTimeOffset.Parse("2026-03-18 18:48:59-04:00");
            TacticsTimestamps timeStamps = new TacticsTimestamps()
            {
                Command_Established = baseTime.AddMinutes(11)
            };
            //create patch object
            SetObject tacs = new SetObject() { Value = timeStamps };
            //assign to incident patchProps:
            patchProps.Tactic_Timestamps = tacs;

            IncidentPatchPayload incidentPatchPayload = new IncidentPatchPayload(incidentId, patchProps);
            return incidentPatchPayload;
        }
    }
}
