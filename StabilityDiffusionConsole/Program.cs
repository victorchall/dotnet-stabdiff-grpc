using StabilityDiffusionConsole;
using StabilitySdkClient;
using System.CommandLine;

Action<Answer, string> answerHandler = async (a, outdir) =>
{
    var saveMimeTypesWithExtension = new Dictionary<string, string>();
    saveMimeTypesWithExtension.Add("image/png", "png");

    int i = 0;

    if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

    Console.WriteLine($"{DateTime.Now}: Received Answer, request id: {a.RequestId}, node: {a.Meta?.NodeId}, gpu id: {a.Meta?.GpuId}, engine: {a.Meta?.EngineId}");

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
            Console.WriteLine($"{DateTime.Now}: Received image. Seed: {artifact.Seed}, id: {artifact.Id}, finish reason: {artifact.FinishReason}");
            Console.WriteLine($"{DateTime.Now}: Writing: {fileMask}");

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

var samplerOption = new Option<string>(
    aliases: new string[] { "--sampler", "-sampler" },
    description: "Sampler option: [ddim, plms, k_euler, k_euler_acenstral, k_dpm_2, klms, k_heun]",
    getDefaultValue: () => string.Empty
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

static DiffusionSampler CreateDiffusionSampler(string samplerString) =>
    samplerString switch
    {
        /*from beta.dreamstudio.ai, with more lenient options added
        * a=[{value:Pe.DiffusionSampler.SAMPLER_DDIM,label:"ddim"},
        * {value:Pe.DiffusionSampler.SAMPLER_DDPM,label:"plms"},
        * {value:Pe.DiffusionSampler.SAMPLER_K_EULER,label:"k_euler"},
        * {value:Pe.DiffusionSampler.SAMPLER_K_EULER_ANCESTRAL,label:"k_euler_ancestral"},
        * {value:Pe.DiffusionSampler.SAMPLER_K_HEUN,label:"k_heun"},
        * {value:Pe.DiffusionSampler.SAMPLER_K_DPM_2,label:"k_dpm_2"},
        * {value:Pe.DiffusionSampler.SAMPLER_K_DPM_2_ANCESTRAL,label:"k_dpm_2_ancestral"},
        * {value:Pe.DiffusionSampler.SAMPLER_K_LMS,:"klms"
        * */
        "ddim" => DiffusionSampler.SamplerDdim,

        "plms" => DiffusionSampler.SamplerDdpm,

        "k_euler" => DiffusionSampler.SamplerKEuler,

        "k_euler_a" => DiffusionSampler.SamplerKEulerAncestral,
        "k_euler_ancestral" => DiffusionSampler.SamplerKEulerAncestral,
        "keulera" => DiffusionSampler.SamplerKEulerAncestral,
        "keulerancestral" => DiffusionSampler.SamplerKEulerAncestral,

        "k_heun" => DiffusionSampler.SamplerKHeun,
        "kheun" => DiffusionSampler.SamplerKHeun,

        "kdpm2a" => DiffusionSampler.SamplerKDpm2Ancestral,
        "k_dpm_2a" => DiffusionSampler.SamplerKDpm2Ancestral,
        "k_dpm_2ancentral" => DiffusionSampler.SamplerKDpm2Ancestral,

        "klms" => DiffusionSampler.SamplerKLms,
        "k_lms" => DiffusionSampler.SamplerKLms,
        _ => DiffusionSampler.SamplerDdpm
    };

generateCommand.SetHandler(async (h, w, prompt, engineId, steps, apiKey, outdir, sampler) =>
{
    var request = new Request();

    var requestId = Guid.NewGuid().ToString();
    request.RequestId = requestId;

    var samplerEnum = CreateDiffusionSampler(sampler);

    request.Prompt.Add(new Prompt() { Text = prompt });
    request.Image = new ImageParameters
    {
        Height = h,
        Width = w,
        Steps = steps,
        Transform = new TransformType { Diffusion = samplerEnum },
        Samples = 1
    };
    request.EngineId = engineId;

    if (!outdir.EndsWith("/")) outdir = $"{outdir}/";

    if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

    Console.WriteLine($"**** Generated RequestRequest id: {requestId} ****");
    Console.WriteLine($"* Sampler: {samplerEnum}, steps: {steps}, samples: {1}, model: {engineId}");
    Console.WriteLine($"* h: {h}, w: {w}, prompt:{prompt}");

    var metadata = GeneratorClient.CreateMetaData(apiKey);
    var generatorClient = new GeneratorClient(request, metadata);    

    await generatorClient.Generate((answer) => answerHandler(answer, outdir));
},
heightOption, widthOption, promptOption, engineOption, stepsOption, apiKeyOption, outDirOption, samplerOption);

await generateCommand.InvokeAsync(args);
