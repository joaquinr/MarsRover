using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MarsRover.Api.Client;
using MarsRover.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MarsRover.Api
{
    public class Program
    {
        private static IHost host;
        public static void Main(string[] args)
        {
            host = CreateHostBuilder(args).Build();
            var task = host.RunAsync();

            if (args != null && args.Count() == 2 && !string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]))
            {
                RunCLIClient(args);
            }

            task.Wait();
        }

        private static void RunCLIClient(string[] args)
        {
            var inputFile = args[0];
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Input file does not exist, aborting execution");
                host.StopAsync().Wait();
            }
            else
            {
                RunApiCallAndSaveResults(args, inputFile);
            }
        }

        private static void RunApiCallAndSaveResults(string[] args, string inputFile)
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new StringEnumConverter());
            jsonSettings.Formatting = Formatting.Indented;

            var input = JsonConvert.DeserializeObject<RoverInput>(File.ReadAllText(inputFile), jsonSettings);
            var apiClient = new RoverClient("https://localhost:5001", new HttpClient());

            var result = apiClient.SimulateRunAsync(input).Result;
            File.WriteAllText(args[1], JsonConvert.SerializeObject(result, jsonSettings));
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
