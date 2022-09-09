using Grpc.Core;
using Grpc.Net.Client;
using StabilityDiffusionConsole;
using static Grpc.Core.Metadata;

namespace StabilitySdkClient
{
    public class GeneratorClient
    {
        const string API_SERVER_ADDRESS = "https://grpc.stability.ai:443";

        public GeneratorClient(Request request, Metadata metadata)
        {
            Request = request;
            Metadata = metadata;
        }

        public Request Request { get; }
        public Metadata Metadata { get; }

        public static Metadata CreateMetaData(string apikey)
        {
           return new Metadata() { new Entry("authorization", $"Bearer {apikey}") };
        }

        public async Task Generate(Action<Answer> answerHandler)
        {
            using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
            {
                var client = new GenerationService.GenerationServiceClient(channel);

                Request.RequestedType = ArtifactType.ArtifactImage; // needed? assumed? 

                var reply = client.Generate(Request, Metadata);
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