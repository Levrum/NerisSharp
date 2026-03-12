using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NerisLibrary;
using NerisLibrary.Models;
using NerisLibrary.Models.ElementModels;
using NerisLibrary.Models.RequestModels;

namespace NerisRunner
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());

            Config clientCredentialsLogin = Config.CreateClientCredentialConfig("CLIENT ID HERE", "CLIENT SECRET HERE", UrlType.Test);
            HttpClient client = new HttpClient(new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) });
            NerisBase nb = new NerisBase(clientCredentialsLogin, client, factory.CreateLogger<NerisBase>());

            string levrumEntityId = "FD39023168";
            string stationId = "FD39023168S001";

            EntityModel result = await nb.GetEntity("FD39023168");

            Console.WriteLine(result.Stations.FirstOrDefault().Units.FirstOrDefault().Neris_Id);


        }

        static StationModel SampleLevrumStation = new StationModel()
        {
            Station_Id = "Test Station",
            Address_Line_1 = "1707 Commerce Road",
            City = "Springfield",
            State = "OH",
            Zip_Code = "44504",
        };

        static StationModel UpdateStation = new StationModel()
        {
            Neris_Id = "FD39023168S001",
            Staffing = 0
        };

        static EntityModel LevrumUpdate = new EntityModel()
        {
            Website = "https://levrum.com/"
        };

        static UnitModel SampleUnit = new UnitModel()
            {
            Staffing = 1,
            Dedicated_Staffing = true,
            Type = UnitTypes.ENGINE_STRUCT,
            Cad_Designation_1 = "ENG1",
            };

            var result = await nb.GetEntities(erm);
            Console.WriteLine(result.Entities[0].Name);

        }
    }
}
