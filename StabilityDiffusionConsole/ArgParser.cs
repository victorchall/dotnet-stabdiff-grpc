
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection.Metadata.Ecma335;
using Grpc.Core;
using static Grpc.Core.Metadata;

namespace StabilityDiffusionConsole
{
    internal static class ArgParser
    {
        public static (Request, LocalAppArgs) ParseRequest(params string[] args)
        {
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
                    getDefaultValue: () => String.Empty
                );

            var outDirOption = new Option<string>(
                aliases: new string[] { "--outdir", "-outdir" },
                description: "Output directory",
                  getDefaultValue: () => "./"
                );

            var generateCommand = new RootCommand("Request args")
            {
                heightOption,
                widthOption,
                promptOption,
                engineOption,
                stepsOption,
                outDirOption,
                apiKeyOption
            };

            var request = new Request();
            var localAppArgs = new LocalAppArgs();

            generateCommand.SetHandler((h, w, p, e, s, a, o) =>
            {
                request.Prompt.Add(new Prompt() { Text = p });
                request.Image = new ImageParameters
                {
                    Height = h,
                    Width = w,
                    Steps = s,
                    Transform = new TransformType { Diffusion = DiffusionSampler.SamplerKEuler },
                    Samples = 1
                };
                request.EngineId = e;

                localAppArgs.ApiKey = a;
                localAppArgs.OutDir = o;
            },
            heightOption, widthOption, promptOption, engineOption, stepsOption, apiKeyOption, outDirOption);

            generateCommand.Invoke(args);

            return (request, localAppArgs);
        }

        public static LocalAppArgs ParseLocalAppArgs(params string[] args)
        {
            var apiKeyOption = new Option<string>(
                aliases: new string[] { "--apikey", "-apikey" },
                description: "Your API Key",
                  getDefaultValue: () => String.Empty
                );

            var outDirOption = new Option<string>(
                aliases: new string[] { "--outdir", "-outdir" },
                description: "Output directory",
                  getDefaultValue: () => "./"
                );

            var localAppArgsCommand = new Command("localAppArgs")
            {
                apiKeyOption
            };

            var localAppArgs = new LocalAppArgs();

            localAppArgsCommand.SetHandler((a) => {
                localAppArgs.ApiKey = a;
            },
            apiKeyOption);

            return localAppArgs;
        }
    }
}
