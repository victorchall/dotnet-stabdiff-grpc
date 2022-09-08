using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabilityDiffusionConsole
{
    internal class LocalAppArgs
    {
        public string ApiKey { get; set; } = string.Empty;
        public string OutDir { get; set; } = "./";
    }
}
