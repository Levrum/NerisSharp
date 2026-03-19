using NerisLibrary.Models.ElementModels.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerisRunner
{
    internal static class SampleIncidentFactory
    {
        public static IncidentModel CreateSampleIncident()
        {
            IncidentTypeUtil.TryCreateIncidentTypeString(out string typeString, IncidentTypeEnum.MEDICAL,
                IncidentTypeEnum.INJURY, IncidentTypeEnum.FALL);

            IncidentType iType = new IncidentType()
            {
                Type = typeString,
                Primary = true
            };

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
            Console.WriteLine(locationModel.StreetAddress());

            IncidentBaseModel modelBase = new IncidentBaseModel()
            {
                People_Present = true,
                Outcome_Narrative = "SAMPLE Subject taken to hospital by EMS unit with minor injuries",
                Location = locationModel,
                Department_Neris_Id = "FD39023168",
                Incident_Number = "1234"
            };

            DispatchModel dispatch = new DispatchModel()
            {
                Center_Id = "1122",
                Incident_Number = "1234",
                Automatic_Alarm = false,
                Call_Arrival = DateTimeOffset.UtcNow.AddMinutes(-10),
                Call_Answered = DateTimeOffset.UtcNow.AddMinutes(-9),
                Call_Create = DateTimeOffset.UtcNow.AddMinutes(-4),
                Location = locationModel,
                Unit_Responses = new List<UnitResponse>()
            };



            IncidentModel model = new IncidentModel()
            {
                Incident_Types = new List<IncidentType>() { iType },
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
                //Unit_Neris_Id = unitId, //test if required
                //Reported_Unit_Id = "EMS1" //test if required
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
            unitResponse.Med_Responses.Add(medResonse);
            return null;
        }
    }
}
