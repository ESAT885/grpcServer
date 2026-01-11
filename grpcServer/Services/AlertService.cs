using Grpc.Core;
using grpcAlertServer;

namespace grpcServer.Services
{
    public class AlertService:Alert.AlertBase
    {
        private readonly ILogger<GreeterService> _logger;
        public AlertService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        public override async Task<AlertResponse> SendAlert(IAsyncStreamReader<AlertRequest> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                var alertRequest = requestStream.Current;
       
                Console.WriteLine($"Received Alert: Type={alertRequest.Alert}");

            }
            return new AlertResponse
            {
                Message = "All alerts processed."
            };
        }
        
    }
}
