using StabilitySdkClient;
using StabilitySdkClient.Engines;

Action<EngineInfo> handler = (e) =>
{
    Console.WriteLine($"Name: {e.Name}, id: {e.Id}, owner: {e.Owner}, type: {e.Type}, descr: {e.Description}, ready: {e.Ready}, tknzr: {e.Tokenizer}");
};

var metadata = MetadataFactory.CreateMetaData(Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty);

var enginesClient = new EnginesClient(metadata);

await enginesClient.GetEngines(handler);

/* 2022-09-18
Name: Prompt Checker v1, id: prompt-checker-v1, owner: stabilityai, type: Text, descr: A model for checking prompts, ready: True, tknzr: Gpt2
Name: Stable Diffusion v1.4, id: stable-diffusion-v1, owner: stabilityai, type: Picture, descr: Stability-AI Stable Diffusion v1.4, ready: True, tknzr: Gpt2
Name: Stable Diffusion v1.5, id: stable-diffusion-v1-5, owner: stabilityai, type: Picture, descr: Stability-AI Stable Diffusion v1.5, ready: True, tknzr: Gpt2
Name: ViT-L-14, id: vit-l-14, owner: stabilityai, type: Text, descr: CLIP ViT L 14, ready: True, tknzr: Gpt2
*/