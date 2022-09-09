using Grpc.Core;
using Grpc.Net.Client;
using StabilityDiffusionConsole;
using static Grpc.Core.Metadata;

namespace StabilitySdkClient
{
    public class GeneratorClient
    {
        const string API_SERVER_ADDRESS = "https://grpc.stability.ai:443";

        public static Metadata CreateMetaDataWithApiKey(string apikey)
        {
           return new Metadata() { new Entry("authorization", $"Bearer {apikey}") };
        }

        public async Task Generate(Request request, Metadata metadata, Action<Answer> actionForAnswers)
        {
            var requestId = Guid.NewGuid().ToString();

            using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
            {
                var client = new GenerationService.GenerationServiceClient(channel);

                request.RequestId = requestId;
                request.RequestedType = ArtifactType.ArtifactImage; // needed? assumed? 

                var reply = client.Generate(request, metadata);
                var answers = reply.ResponseStream.ReadAllAsync();
                try
                {
                    await foreach (var answer in answers)
                    {
#if DEBUG
                        if (!answer.Artifacts.Any()) Console.WriteLine($"{DateTime.Now}: Keepalive received");
#endif
                        actionForAnswers(answer);
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