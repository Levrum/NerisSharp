using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NerisLibrary;
using NerisLibrary.Models;

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
            await nb.Login();

            EntityRequestModel erm = new EntityRequestModel()
            {
                Name = "portland",
                State = "or"
            };

            var result = await nb.GetEntities(erm);
            Console.WriteLine(result.Entities[0].Name);

        }
    }
}
