using Grpc.Core;
using grpcMessageServer;

namespace grpcServer.Services
{
    public class MessageService : Message.MessageBase
    {
        private readonly ILogger<GreeterService> _logger;
        public MessageService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
        public override async Task SendMessage(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
        
            System.Console.WriteLine($"Gelen Mesaj: {request.Message} - From: {request.Name}");
            for (int i = 0; i < 10; i++)
            {
               await Task.Delay(200);
               await responseStream.WriteAsync(new MessageResponse
                {
                    Message = $"Merhaba {request.Name} - {i}",

                });
            }
            await Task.Delay(20220);
            await responseStream.WriteAsync(new MessageResponse
            {
                Message = $"Merhaba {request.Name} - p",

            });
        }
    
    }
}
