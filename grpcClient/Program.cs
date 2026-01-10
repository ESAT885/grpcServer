using Grpc.Net.Client;
using grpcClient;


var channel = GrpcChannel.ForAddress("http://localhost:5127");
var greetingClient = new Greeter.GreeterClient(channel);
 HelloReply result =await greetingClient.SayHelloAsync(
     new HelloRequest 
     { Name = " World" 
     });

Console.WriteLine(result.Message);
Console.ReadLine();