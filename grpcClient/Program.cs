using Grpc.Net.Client;
using grpcAlertClient;
using grpcClient;
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

//Server Streaming
var messageClient = new Message.MessageClient(channel);
var messageResult = messageClient.SendMessage(
    new MessageRequest
    {
        Message = "Merhaba",
        Name = "Esat"
    });
CancellationTokenSource cts = new CancellationTokenSource();
while (await messageResult.ResponseStream.MoveNext(cts.Token))
{
    var currentMessage = messageResult.ResponseStream.Current;
    Console.WriteLine(currentMessage.Message);
}
//Client Streaming
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
