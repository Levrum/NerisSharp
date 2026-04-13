using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
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


            Console.WriteLine("Please enter the client ID: ");
            string sclientid = Console.ReadLine();
            if (string.IsNullOrEmpty(sclientid)) { return; }
            Console.WriteLine("Please enter the client secret: ");
            string sclientpass = Console.ReadLine();
            if (string.IsNullOrEmpty(sclientpass)) { return; }
            //Config clientCredentialsLogin = Config.CreateClientCredentialConfig("CLIENT ID HERE", "CLIENT SECRET HERE");
            Config clientCredentialsLogin = Config.CreateClientCredentialConfig(sclientid,sclientpass);
            if (null == clientCredentialsLogin) { return; }

            HttpClient client = new HttpClient(new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) });
            NerisBase nb = new NerisBase(clientCredentialsLogin, client, factory.CreateLogger<NerisBase>());

            string RWB_ENTITY_ID = "FD08117680";

            IncidentRequestModel requestModel = new IncidentRequestModel();
            requestModel.Call_Create_Start = DateTime.Now.AddDays(-90);
            requestModel.Call_Create_End = DateTime.Now.AddDays(-1);
            requestModel.Neris_Id_Entity = RWB_ENTITY_ID;
            requestModel.Status = IncidentStatusTypes.APPROVED;

            Stopwatch stopwatch = Stopwatch.StartNew();

            List<IncidentModel> incidents = await nb.GetAllIncidents(requestModel);
            double retrievalSec = stopwatch.Elapsed.TotalSeconds;
            double writeSec = 0;
            if (incidents.Count == 0)
            {
                Console.WriteLine("DIDN'T GET ANY DATA");
            }
            else
            {
                Console.WriteLine("SUCCESS, GOT {0} INCIDENTS", incidents.Count);
                int n = 0;
                foreach (IncidentModel incident in incidents)
                {
                    n++;
                    Console.WriteLine(n.ToString().PadLeft(4, ' ') + ": " + incident.Prettyprint());
                }
                writeSec = stopwatch.Elapsed.TotalSeconds - retrievalSec;
            }

            Console.WriteLine("Total incidents: " + incidents.Count);
            Console.WriteLine("  Retrieval: " + Math.Round(retrievalSec, 3).ToString().PadLeft(8, ' ') + " sec.");
            Console.WriteLine("  Output:    " + Math.Round(writeSec,3).ToString().PadLeft(8,' ') + " sec.");
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

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
