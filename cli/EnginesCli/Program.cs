using StabilitySdkClient;
using StabilitySdkClient.Engines;

Action<EngineInfo> handler = (e) =>
{
    Console.WriteLine($"Name: {e.Name}, id: {e.Id}, owner: {e.Owner}, type: {e.Type}, descr: {e.Description}, ready: {e.Ready}, tknzr: {e.Tokenizer}");
};

var metadata = MetadataFactory.CreateMetaData(Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty);

var enginesClient = new EnginesClient(metadata);

await enginesClient.GetEngines(handler);