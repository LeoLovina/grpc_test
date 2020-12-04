# gRPC
* gRPC uses a contract-first approach to API development. 
* Protocol buffers (protobuf) are used as the Interface Definition Language (IDL) by default. The *.proto file contains:
    * The definition of the gRPC service.
    * The messages sent between clients and servers.
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
*  compile the project
   *  