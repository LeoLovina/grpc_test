using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServer.Protos;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class PersonService : PersonProto.PersonProtoBase
    {
        private readonly ILogger<PersonService> _logger;

        public PersonService(ILogger<PersonService> logger)
        {
            this._logger = logger;
        }

        public override Task<PersonResponse> GetAll(PersonRequest request, ServerCallContext context)
        {
            return Task.FromResult(new PersonResponse()
                {
                    Id = request.Id,
                    FirstName = $"First {DateTime.Now.Minute}",
                    LastName = $"Last {DateTime.Now.Second}"
                }
            );
        }

        public override async Task StreamFromServer(PersonRequest request, IServerStreamWriter<PersonResponse> responseStream, ServerCallContext context)
        {
            for (var i = 0; i < 5; i++)
            {
                await responseStream.WriteAsync(new PersonResponse()
                {
                    Id = request.Id + i,
                    FirstName = $"First {DateTime.Now.Minute}",
                    LastName = $"Last {DateTime.Now.Second}"
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public override async Task<PersonResponse> StreamFromClient(IAsyncStreamReader<PersonRequest> requestStream,
            ServerCallContext context)
        {
            int count = 0;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (await requestStream.MoveNext())
            {
                var person = requestStream.Current;
                count += person.Id;
            }

            stopWatch.Stop();
            return new PersonResponse()
                {
                    Id = count,
                    FirstName = $"First {DateTime.Now.Minute}",
                    LastName = $"Last {DateTime.Now.Second}"
                }
            ;
        }

        public override async Task StreamBothWay(IAsyncStreamReader<PersonRequest> requestStream, IServerStreamWriter<PersonResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var person = requestStream.Current;
                await responseStream.WriteAsync(new PersonResponse()
                {
                    Id = person.Id,
                    FirstName = $"First {DateTime.Now.Minute}",
                    LastName = $"Last {DateTime.Now.Second}"
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
