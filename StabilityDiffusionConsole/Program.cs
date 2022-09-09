using StabilityDiffusionConsole;
using StabilitySdkClient;
using System.CommandLine;

Action<Answer, string> writeAnswerPayloadToDiskAction = async (a, outdir) =>
{
    var saveMimeTypesWithExtension = new Dictionary<string, string>();
    saveMimeTypesWithExtension.Add("image/png", "png");
    saveMimeTypesWithExtension.Add("image/jpg", "jpg");

    int i = 0;

    if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

    foreach (var artifact in a.Artifacts)
    {
        if (saveMimeTypesWithExtension.ContainsKey(artifact.Mime))
        {
            var ext = saveMimeTypesWithExtension[artifact.Mime];

            string fileMask = $"{outdir}{i++:D5}.{ext}";
            while (File.Exists(fileMask))
            {
                fileMask = $"{outdir}{i++:D5}.{ext}";
            }
            Console.WriteLine($"{DateTime.Now}: writing image/{ext}: {fileMask}");

            // TODO: consider filename based on prompt, seed, outdir, etc. or arg based
            await File.WriteAllBytesAsync($"{fileMask}", artifact.Binary.ToByteArray());
        }
    }
};

var heightOption = new Option<ulong>(
    aliases: new string[] { "--h", "-h" },
    description: "Height of the image to be created in pixels.",
    getDefaultValue: () => 512);

var widthOption = new Option<ulong>(
    aliases: new string[] { "--w", "-w" },
    description: "Width of the image to be created in pixels.",
    getDefaultValue: () => 512);

var stepsOption = new Option<ulong>(
    aliases: new string[] { "--steps", "-steps", "--ddim_steps", "-ddim_steps" },
    description: "Steps",
    getDefaultValue: () => 30);

var promptOption = new Option<string>(
    aliases: new string[] { "--prompt", "-prompt" },
    description: "Text prompt.",
    getDefaultValue: () => "A gold plated cat horse on a table top");

var engineOption = new Option<string>(
    aliases: new string[] { "--engine", "-engine" },
    description: "engine",
    getDefaultValue: () => "stable-diffusion-v1-5"
    );

var apiKeyOption = new Option<string>(
    aliases: new string[] { "--apikey", "-apikey" },
    description: "Your API Key",
    getDefaultValue: () => string.Empty
    );

var outDirOption = new Option<string>(
    aliases: new string[] { "--outdir", "-outdir" },
    description: "Output directory",
    getDefaultValue: () => "output/"
    );

var generateCommand = new RootCommand("command line args")
    {
        heightOption,
        widthOption,
        promptOption,
        engineOption,
        stepsOption,
        outDirOption,
        apiKeyOption
    };

generateCommand.SetHandler(async (h, w, prompt, engineId, steps, apiKey, outdir) =>
{
    var request = new Request();

    request.Prompt.Add(new Prompt() { Text = prompt });
    request.Image = new ImageParameters
    {
        Height = h,
        Width = w,
        Steps = steps,
        Transform = new TransformType { Diffusion = DiffusionSampler.SamplerKEuler },
        Samples = 1
    };
    request.EngineId = engineId;

    if (!outdir.EndsWith("/")) outdir = $"{outdir}/";

    if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

    var generatorClient = new GeneratorClient();
    var metadata = GeneratorClient.CreateMetaDataWithApiKey(apiKey);

    await generatorClient.Generate(request, metadata, (answer) => writeAnswerPayloadToDiskAction(answer, outdir));
},
heightOption, widthOption, promptOption, engineOption, stepsOption, apiKeyOption, outDirOption);

await generateCommand.InvokeAsync(args);