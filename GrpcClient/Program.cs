using System;
using Grpc.Net.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GrpcClient.Protos;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using Serilog;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start App
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            // Create service collection
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            await serviceProvider.GetService<App>().Run().ConfigureAwait(false);

        }


        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            // Add logging
            serviceCollection.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                 .CreateLogger();

            // Add app
            serviceCollection.AddTransient<App>();
        }
    }
}
