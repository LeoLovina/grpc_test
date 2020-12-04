using System;
using Grpc.Net.Client;
using System.Net.Http;
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
                new HelloRequest { Name = "GreeterClient" });

            Console.WriteLine($"Greeting: {reply.Message}");

            var PersonClient = new PersonProto.PersonProtoClient(channel);
            var PersonResponse = await PersonClient.GetAllAsync(
                new PersonRequest {Id = 5});

            Console.WriteLine($"PersonResponse: {PersonResponse.Id} {PersonResponse.FirstName} {PersonResponse.LastName}");
        }
    }
}
