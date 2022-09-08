using Grpc.Core;
using Grpc.Net.Client;
using StabilityDiffusionConsole;
using System.Text;
using static Grpc.Core.Metadata;

const string API_SERVER_ADDRESS = "https://grpc.stability.ai:443";

var (request, localAppArgs) = ArgParser.ParseRequest(args);
var requestId = Guid.NewGuid().ToString();

var metadata = new Metadata() { new Entry("authorization", $"Bearer {localAppArgs.ApiKey}")};

Console.WriteLine($"{DateTime.Now}: Generated request. Request id: {requestId}, prompt: '{request.Prompt.FirstOrDefault()?.Text}', h:{request.Image.Height}, w:{request.Image.Width}");

var answersExt = new List<Answer>();

using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
{
    var client = new GenerationService.GenerationServiceClient(channel);
    
    request.RequestId = requestId;
    request.RequestedType = ArtifactType.ArtifactImage; // needed? assumed? 

    var reply = client.Generate(request, metadata);
    var answers = reply.ResponseStream.ReadAllAsync();
    try 
    {
        await foreach(var answer in answers)
        {
#if DEBUG
            if (!answer.Artifacts.Any()) Console.WriteLine($"{DateTime.Now}: Keepalive received");
#endif

            // TODO: write files as we go? easier to debug just setting them aside for now
            answersExt.Add(answer);
        }
    }
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Unauthenticated)
    {
        Console.WriteLine($"{DateTime.Now}: Authorization failed.  Check API key.  Aborting.");
    }
}

int i = 0;

var writeFileTasks = new List<Task>();

var saveMimeTypesWithExtension = new Dictionary<string, string>();
saveMimeTypesWithExtension.Add("image/png", "png");
saveMimeTypesWithExtension.Add("image/jpg", "jpg");

foreach (var answer in answersExt)
{
    foreach (var artifact in answer.Artifacts)
    {
        if (saveMimeTypesWithExtension.ContainsKey(artifact.Mime))
        {
            var ext = saveMimeTypesWithExtension[artifact.Mime];

            string fileMask = $"{i++:D5}.{ext}";
            while (File.Exists(fileMask))
            {
                fileMask = $"{i++:D5}.{ext}";
            }
            Console.WriteLine($"{DateTime.Now}: writing image/{ext}: {fileMask}");

            // TODO: consider filename based on prompt, seed, etc. or arg based
            writeFileTasks.Add(File.WriteAllBytesAsync(fileMask, artifact.Binary.ToByteArray()));
        }
    }
}

// TODO: probably wont do this async?
Task.WaitAll(writeFileTasks.ToArray());
