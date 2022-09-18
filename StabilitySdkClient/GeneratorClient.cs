using Grpc.Core;
using Grpc.Net.Client;
using static Grpc.Core.Metadata;
using StabilitySdkClient.Generation;

namespace StabilitySdkClient
{
    public sealed class GeneratorClient : AbstractClient
    {
        private Request _request { get; }

        public GeneratorClient(Request request, Metadata metadata) : base(metadata)
        {
            _request = request;
        }

        public async Task Generate(Action<Answer> answerHandler)
        {
            using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
            {
                var client = new GenerationService.GenerationServiceClient(channel);
                var reply = client.Generate(_request, _metadata);
                var answers = reply.ResponseStream.ReadAllAsync();

                try
                {
                    await foreach (var answer in answers)
                    {
                        if (!answer.Artifacts.Any()) Console.WriteLine($"{DateTime.Now}: Keepalive received");

                        answerHandler(answer);
                    }
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unauthenticated)
                {
                    Console.WriteLine($"{DateTime.Now}: Authorization failed.  Check API key.  Aborting.");
                }
            }
        }
    }
}