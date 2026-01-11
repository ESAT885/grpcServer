using Grpc.Net.Client;
using grpcClient;
using grpcMessageClient;

///Unary
var channel = GrpcChannel.ForAddress("http://localhost:5127");
var greetingClient = new Greeter.GreeterClient(channel);
 HelloReply result =await greetingClient.SayHelloAsync(
     new HelloRequest 
     { Name = " World" 
     });

Console.WriteLine(result.Message);

//Server Streaming
var messageClient = new Message.MessageClient(channel);
var messageResult =  messageClient.SendMessage(
    new MessageRequest
    {
        Message="Merhaba",
        Name="Esat"
    });
CancellationTokenSource cts = new CancellationTokenSource();
while (await messageResult.ResponseStream.MoveNext(cts.Token))
{
    var currentMessage = messageResult.ResponseStream.Current;
    Console.WriteLine(currentMessage.Message);
}
//Client Streaming

Console.ReadLine();