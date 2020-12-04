using System;
using Grpc.Net.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GrpcClient.Protos;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello gRPC!");
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                new HelloRequest {Name = "GreeterClient"});

            Console.WriteLine($"Greeting: {reply.Message}");

            var personClient = new PersonProto.PersonProtoClient(channel);
            var PersonResponse = await personClient.GetAllAsync(new PersonRequest {Id = 5});
            Console.WriteLine(
                $"PersonResponse: {PersonResponse.Id} {PersonResponse.FirstName} {PersonResponse.LastName}");

            // Test StreamFromServer
            using (var call = personClient.StreamFromServer(new PersonRequest() {Id = 1}))
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


    }
}
