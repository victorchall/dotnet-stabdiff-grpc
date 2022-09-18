using Google.Protobuf.WellKnownTypes;
using StabilitySdkClient;
using System;
using System.CommandLine;
using System.Text.Json;
using StabilitySdkClient.Generation;
using StabilitySdkClient;
using Cli.Common;

var generateCommand = new RootCommand("command line args")
    {
        SdOptions.HeightOption,
        SdOptions.WidthOption,
        SdOptions.StepsOption,
        SdOptions.PromptOption,
        SdOptions.EngineOption,
        SdOptions.OutDirOption,
        SdOptions.SamplerOption,
        SdOptions.CountOption
    };

generateCommand.SetHandler(async (h, w, steps, prompt, engineId, outdir, sampler, cnt) =>
{
    var request = new Request();

    var requestId = Guid.NewGuid().ToString();
    request.RequestId = requestId;

    var samplerEnum = SamplerFactory.GetDiffusionSampler(sampler);

    request.Prompt.Add(new Prompt() { Text = prompt });
    request.Image = new ImageParameters
    {
        Height = h,
        Width = w,
        Steps = steps,
        Transform = new TransformType { Diffusion = samplerEnum },
        Samples = cnt,
    };
    request.EngineId = engineId;
    request.RequestedType = ArtifactType.ArtifactImage;

    request.Image.Parameters.Add(new StepParameter
        {
            Sampler = new SamplerParameters { CfgScale = 7.5F },
            ScaledStep = 0
        });

    if (!outdir.EndsWith("/")) outdir = $"{outdir}/";

    if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

    Console.WriteLine($"{DateTime.Now}: **** Generated Txt2Img Request id: {requestId} ****");
    Console.WriteLine($"* sampler: {samplerEnum}, steps: {steps}, samples: {cnt}, model: {engineId}");
    Console.WriteLine($"* H: {h}, W: {w}, prompt: {prompt}");

    var metadata = MetadataFactory.CreateMetaData(Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty);
    var generatorClient = new GeneratorClient(request, metadata);

    var requestTime = DateTime.Now;
    var handler = AnswerHandlerFactory.GetWriteToDiskHandler(true);

    await generatorClient.Generate((answer) => handler(answer, outdir));

    Console.WriteLine($"Elapsed total request {requestId} process time: {(DateTime.Now - requestTime).TotalSeconds:F2} s");
},
SdOptions.HeightOption,SdOptions.WidthOption,SdOptions.StepsOption,SdOptions.PromptOption,SdOptions.EngineOption,SdOptions.OutDirOption,SdOptions.SamplerOption,SdOptions.CountOption);

await generateCommand.InvokeAsync(args);
