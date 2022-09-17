using StabilitySdkClient.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Common
{
    public static class SamplerFactory
    {
        public static DiffusionSampler GetDiffusionSampler(string samplerString) =>
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
    }
}
