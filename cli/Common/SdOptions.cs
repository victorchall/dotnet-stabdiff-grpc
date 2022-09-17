using System;
using System.CommandLine;
using System.Runtime.InteropServices;

namespace Cli.Common
{
    public sealed class SdOptions
    {
        private static readonly Option<float> strengthOption =
           new Option<float>(
               aliases: new string[] { "--h", "-h" },
               description: "Denoising strength to weigh initial image and prompt text for img2img.",
               getDefaultValue: () => 512);

        public static Option<float> StrengthOption => strengthOption;

        private static readonly Option<string> initImageOption =
           new Option<string>(
               aliases: new string[] { "--h", "-h" },
               description: "Initial image for img2img",
               getDefaultValue: () => string.Empty);

        public static Option<string> InitImageOption => initImageOption;

        private static readonly Option<ulong> heightOption = 
            new Option<ulong>(
                aliases: new string[] { "--h", "-h" },
                description: "Height of the image to be created in pixels.",
                getDefaultValue: () => 512);

        public static Option<ulong> HeightOption => heightOption;

        private static readonly Option<ulong> widthOption = 
            new Option<ulong>(
                aliases: new string[] { "--w", "-w" },
                description: "Width of the image to be created in pixels.",
                getDefaultValue: () => 512);

        public static Option<ulong> WidthOption => widthOption;

        private static readonly Option<ulong> stepsOption =
             new Option<ulong>(
                aliases: new string[] { "--steps", "-steps", "--ddim_steps", "-ddim_steps" },
                description: "Steps",
                getDefaultValue: () => 30);

        public static Option<ulong> StepsOption => stepsOption;
        
        private static readonly Option<string> promptsOption = 
            new Option<string>(
                aliases: new string[] { "--prompt", "-prompt" },
                description: "Text prompt.",
                getDefaultValue: () => "a golden cat on a wood table");

        public static Option<string> PromptOption => promptsOption;

        private static readonly Option<string> engineOption =
            new Option<string>(
                aliases: new string[] { "--engine", "-engine" },
                description: "engine",
                getDefaultValue: () => "stable-diffusion-v1-5");

        public static Option<string> EngineOption => engineOption;

        private static readonly Option<string> outDirOption =
            new Option<string>(
                aliases: new string[] { "--outdir", "-outdir" },
                description: "Output directory",
                getDefaultValue: () => "output/");

        public static Option<string> OutDirOption => outDirOption;

        private static readonly Option<string> samplerOption =
            new Option<string>(
                aliases: new string[] { "--sampler", "-sampler" },
                description: "Sampler option: [ddim, plms, k_euler, k_euler_acenstral, k_dpm_2, klms, k_heun]",
                getDefaultValue: () => string.Empty);

        public static Option<string> SamplerOption => samplerOption;

        private static readonly Option<uint> cntOption =
            new Option<uint>(
                aliases: new string[] { "--n_samples", "-n_samples", "--n_iter", "-n_iter" },
                description: "Sampler option: [ddim, plms, k_euler, k_euler_acenstral, k_dpm_2, klms, k_heun]",
                getDefaultValue: () => 1);

        public static Option<uint> CountOption => cntOption;
    }
}