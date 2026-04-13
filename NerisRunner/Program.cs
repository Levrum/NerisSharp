using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NerisLibrary;
using NerisLibrary.Models;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.ElementModels.Incident;
using NerisLibrary.Models.RequestModels;
using NerisLibrary.Utils;

namespace NerisRunner
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());

            Config clientCredentialsLogin = Config.CreateClientCredentialConfig("CLIENT ID HERE", "CLIENT SECRET HERE");
            HttpClient client = new HttpClient(new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) });
            NerisBase nb = new NerisBase(clientCredentialsLogin, client, factory.CreateLogger<NerisBase>());

            string RWB_ENTITY_ID = "FD08117680";

            IncidentRequestModel requestModel = new IncidentRequestModel();
            requestModel.Call_Create_Start = DateTime.Now.AddDays(-30);
            requestModel.Call_Create_End = DateTime.Now.AddDays(-1);
            requestModel.Neris_Id_Entity = RWB_ENTITY_ID;
            requestModel.Status = IncidentStatusTypes.APPROVED;

            List<IncidentModel> incidents = await nb.GetAllIncidents(requestModel);
            if (incidents.Count == 0)
            {
                Console.WriteLine("DIDN'T GET ANY DATA");
            }
            else
            {
                Console.WriteLine("SUCCESS, GOT {0} INCIDENTS", incidents.Count);
            }

        }

        //static StationModel SampleLevrumStation = new StationModel()
        //{
        //    Station_Id = "Test Station",
        //    Address_Line_1 = "1707 Commerce Road",
        //    City = "Springfield",
        //    State = "OH",
        //    Zip_Code = "44504",
        //};

        //static StationModel UpdateStation = new StationModel()
        //{
        //    Neris_Id = "FD39023168S001",
        //    Staffing = 0
        //};

        //static EntityModel LevrumUpdate = new EntityModel()
        //{
        //    Website = "https://levrum.com/"
        //};

        //static UnitModel SampleUnit = new UnitModel()
        //{
        //    Staffing = 1,
        //    Dedicated_Staffing = true,
        //    Type = UnitTypes.ENGINE_STRUCT,
        //    Cad_Designation_1 = "ENG1",
        //};


    }
}
