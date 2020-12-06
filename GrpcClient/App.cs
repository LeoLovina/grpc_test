﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcClient.Protos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GrpcClient
{
    public class App
    {
        private readonly ILogger<App> _logger;
        public App(ILogger<App> logger)
        {
            this._logger = logger;
        }

        public async Task Run()
        {
            try
            {
                _logger.LogInformation("App Start");

                Console.WriteLine("Hello gRPC!");
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Greeter.GreeterClient(channel);
                var reply = await client.SayHelloAsync(
                    new HelloRequest { Name = "GreeterClient" });

                Console.WriteLine($"Greeting: {reply.Message}");

                var personClient = new PersonProto.PersonProtoClient(channel);
                var PersonResponse = await personClient.UnaryAsync(new PersonRequest { Id = 5 });
                Console.WriteLine($"PersonResponse = {JsonConvert.SerializeObject(PersonResponse)}");
                DateTime startDate = PersonResponse.StartDate.ToDateTime();
                DateTimeOffset startDateOffset = PersonResponse.StartDate.ToDateTimeOffset();
                var duration = PersonResponse.Duration?.ToTimeSpan();
                Console.WriteLine(
                    $"PersonResponse: {startDate} {PersonResponse.FirstName} {PersonResponse.LastName}");

                // Test StreamFromServer
                using (var call = personClient.StreamFromServer(new PersonRequest() { Id = 1 }))
                {
                    while (await call.ResponseStream.MoveNext(new CancellationToken(false)))
                    {
                        var person = call.ResponseStream.Current;
                        Console.WriteLine(
                            $"Received: {PersonResponse.Id} {PersonResponse.FirstName} {PersonResponse.LastName}");
                    }
                }

                // Test StreamFromClient
                using (var call = personClient.StreamFromClient())
                {
                    for (var i = 0; i < 5; i++)
                    {
                        await call.RequestStream.WriteAsync(new PersonRequest()
                        {
                            Id = i
                        });
                    }
                    await call.RequestStream.CompleteAsync();
                    PersonResponse summary = await call.ResponseAsync;
                }
                channel.ShutdownAsync().Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"App Exception");
                throw;
            }
        }
    }
}