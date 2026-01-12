using System.IO;
using Grpc.Core;
using Grpc.Net.Client;
using grpcAlertClient;
using grpcClient;
using grpcErrorClient;
using grpcMessageClient;

///Unary
var channel = GrpcChannel.ForAddress("http://localhost:5127");
var greetingClient = new Greeter.GreeterClient(channel);
HelloReply result = await greetingClient.SayHelloAsync(
    new HelloRequest
    {
        Name = " World"
    });

Console.WriteLine(result.Message);

// =====================
// SERVER STREAMING (arka planda)
// =====================
var messageClient = new Message.MessageClient(channel);

_ = Task.Run(async () =>
{
    var stream = messageClient.SendMessage(new MessageRequest
    {
        Message = "Merhaba",
        Name = "Esat"
    });

    await foreach (var msg in stream.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine("ServerStream: " + msg.Message);
    }
});


// =====================
// Client STREAMING (arka planda)
// =====================
var alertresponseClient = new Alert.AlertClient(channel);
var requestStream = alertresponseClient.SendAlert();
for (int i = 0; i < 10; i++)
{
    await Task.Delay(1000);
    await requestStream.RequestStream.WriteAsync(
          new AlertRequest
          {
              Alert = $"Alert {i + 1}"
          });
}
await requestStream.RequestStream.CompleteAsync();
var response = await requestStream.ResponseAsync;
Console.WriteLine("Server says: " + response.Message);


// =====================
// Bidirectional STREAMING (arka planda)
// =====================
var errorClient = new Error.ErrorClient(channel);
var errorStream = errorClient.SendError();
Random rnd = new Random();
_ = Task.Run(async () =>
{
    await foreach (var msg in errorStream.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine("FROM SERVER: " + msg.Error);
    }
});
for (int i = 0; i < 20; i++)
{
    int value = rnd.Next(0, 100);

    await errorStream.RequestStream.WriteAsync(new ErrorRequest
    {
        Error="Client error Value: " + value
    });

    Console.WriteLine("RANDOM TO SERVER => " + value);

    await Task.Delay(1000);
}

await errorStream.RequestStream.CompleteAsync();
Console.ReadLine();