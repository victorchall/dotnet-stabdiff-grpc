using Cli.Common;
using StabilitySdkClient.Generation;
using StabilitySdkClient;
using System.CommandLine;
using SixLabors.ImageSharp;
using Google.Protobuf;
using IS = SixLabors.ImageSharp;

var generateCommand = new RootCommand("command line args")
    {
        SdOptions.StepsOption,
        SdOptions.PromptOption,
        SdOptions.EngineOption,
        SdOptions.OutDirOption,
        SdOptions.SamplerOption,
        SdOptions.CountOption,
        SdOptions.InitImageOption,
        SdOptions.StrengthOption
    };

Func<Image,byte[]> getImgBytes = (i) =>
{
    using (var ms = new MemoryStream())
    {
        i.SaveAsPng(ms);
        return ms.ToArray();
    }
};

generateCommand.SetHandler(async (steps, prompt, engineId, outdir, sampler, cnt, initImgPath, str) =>
{
    using (var initImage = await Image.LoadAsync(initImgPath))
    {
        var request = new Request();

        var samplerEnum = SamplerFactory.GetDiffusionSampler(sampler);
        var requestId = Guid.NewGuid().ToString();

        request.EngineId = engineId;
        request.RequestedType = ArtifactType.ArtifactImage;
        request.RequestId = requestId;

        request.Prompt.Add(new Prompt() { Text = prompt });

        var imgBytes = getImgBytes(initImage);

        request.Image = new ImageParameters
        {
            Height = (uint)initImage.Height,
            Width = (uint)initImage.Width,
            Steps = steps,
            Transform = new TransformType { Diffusion = samplerEnum },
            Samples = cnt
        };

        request.Prompt.Add(new Prompt()
            {
                Artifact = new Artifact()
                {
                    Binary = ByteString.CopyFrom(imgBytes),
                    Type = ArtifactType.ArtifactImage
                },
                Parameters = new PromptParameters { Init = true }
            });

        request.Image.Parameters.Add(new StepParameter
            {
                Schedule = new ScheduleParameters { Start = str, End = 0.01F },
                Sampler = new SamplerParameters { CfgScale = 7.5F },
                ScaledStep = 0
            });

        if (!outdir.EndsWith("/")) outdir = $"{outdir}/";

        if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

        Console.WriteLine($"{DateTime.Now}: ****  Generated Img2Img Request id: {requestId} ****");
        Console.WriteLine($"* sampler: {samplerEnum}, steps: {steps}, samples: {cnt}, model: {engineId}, initImg:{initImgPath}");
        Console.WriteLine($"* H: {initImage.Height}, W: {initImage.Width}, prompt: {prompt}");

        var metadata = GeneratorClient.CreateMetaData(Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty);
        var generatorClient = new GeneratorClient(request, metadata);

        var requestTime = DateTime.Now;
        var handler = AnswerHandlerFactory.GetWriteToDiskHandler(true);

        await generatorClient.Generate((answer) => handler(answer, outdir));

        Console.WriteLine($"Elapsed total request {request.RequestId} process time: {(DateTime.Now - requestTime).TotalSeconds:F2} s");
    }
},
SdOptions.StepsOption,SdOptions.PromptOption,SdOptions.EngineOption,SdOptions.OutDirOption,SdOptions.SamplerOption,SdOptions.CountOption,SdOptions.InitImageOption,SdOptions.StrengthOption);

await generateCommand.InvokeAsync(args);
