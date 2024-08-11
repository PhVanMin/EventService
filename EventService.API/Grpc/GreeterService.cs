using Grpc.Core;
using EventService.API.Protos;

namespace EventService.API.Grpc {
    public class GreeterService : Greeter.GreeterBase {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            return Task.FromResult(new HelloReply {
                Message = "Hello " + request.Name
            });
        }
    }
}
