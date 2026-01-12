using Grpc.Core;
using grpcErrorServer;

namespace grpcServer.Services
{
    public class ErrorService : Error.ErrorBase
    {
        private readonly ILogger<GreeterService> _logger;
        public ErrorService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        public override async Task SendError(IAsyncStreamReader<ErrorRequest> requestStream, IServerStreamWriter<ErrorResponse> responseStream, ServerCallContext context)
        {
            var rnd = new Random();
            await foreach (var v in requestStream.ReadAllAsync())
            {

                Console.WriteLine("CLIENT error -> " + v.Error);

                var serverValue = rnd.Next(0, 1000);

                await responseStream.WriteAsync(new ErrorResponse
                {
                    Error = "Server error Value: " + serverValue
                });

            

            }
        }
    }
}
