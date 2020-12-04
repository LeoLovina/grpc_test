# gRPC
* uses a contract-first approach to API development. 
* uses Protocal Buffers to serialize structured data
* Protocol buffers (protobuf) are used as the Interface Definition Language (IDL) by default. The *.proto file contains:
    * The definition of the gRPC service.
    * The messages sent between clients and servers.
    * The message contains a series of name-value pairs called fields
* use protocal buffer compiler to generate data access classes
# gRPC Server
* define a .proto file
  * Ususally the proto files are under folder Protos
  * example: person.proto
``` C#
syntax = "proto3";
option csharp_namespace = "GrpcServer.Protos";
package person;
service PersonProto{
	rpc GetAll(PersonRequest) returns (PersonResponse); 
}
message PersonRequest {
	int32 id =1;
}
message PersonResponse {
	int32 id =1;
	string FirstName = 2;
	string LastName = 3;
}
```
    * The package name must be the same on server and client 


* The *.proto file is included in a project by adding it to the <Protobuf> item group
  * Example 
``` json
  <ItemGroup>
    <Protobuf Include="Protos\person.proto" GrpcServices="Server" />
  </ItemGroup>
```
* compile the project	
  * By default, a <Protobuf> reference generates a concrete client and a service base class. 
  * Grpc.AspNetCore is required in gRPC Server
* Create a concrete service implementation that derives from this base type and implements the logic for the gRPC calls.
* Add endpoint to Startup
``` C#
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<PersonService>();
});
```

 
# gRPC Client
* The *.proto file is included in a project by adding it to the <Protobuf> item group
  * Example 
``` json
  <ItemGroup>
    <Protobuf Include="Protos\person.proto" GrpcServices="Client" />
  </ItemGroup>
```
*  compile the project

https://docs.microsoft.com/en-us/aspnet/core/grpc/services?view=aspnetcore-5.0
https://grpc.io/docs/what-is-grpc/introduction/

