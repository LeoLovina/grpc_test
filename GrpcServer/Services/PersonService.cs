using System;
using System.Collections.Generic;
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
    }
}
