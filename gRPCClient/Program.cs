
using Grpc.Core;
using Grpc.Net.Client;
using GrpcExample;

namespace gRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7235");
            var client = new Greeter.GreeterClient(channel);

            using var call = client.SayHelloStream();

            _ = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine("Greeting: " + response.Message);
                }
            });

            while (true ){
                var result = Console.ReadLine();
                if (string.IsNullOrEmpty(result))
                {
                    break;
                }
                await call.RequestStream.WriteAsync(new HelloRequest { Name = result });
            }

            await call.RequestStream.CompleteAsync();

            //Console.WriteLine("Greeting: " + reply.Message);
            Console.ReadKey();

        }

    }
}