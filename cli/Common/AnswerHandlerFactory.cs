using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StabilitySdkClient.Generation;

namespace Cli.Common
{
    public static class AnswerHandlerFactory
    {
        private static readonly Action<Artifact, string> DumptoJson = async (artifact, fileName) =>
        {
            Console.WriteLine($"Dumping artifact: {fileName}");
            var answertxt = JsonSerializer.Serialize(artifact);
            await File.WriteAllTextAsync(fileName, answertxt);
        };

        public static Action<Answer,string> GetWriteToDiskHandler(bool dumpFilteredToJson = false)
        {
            return async (a, outdir) =>
            {
                var saveMimeTypesWithExtension = new Dictionary<string, string>
                {
                    { "image/png", "png" },
                    { "image/jpg", "jpg" }
                };

                int i = 0;

                if (!Directory.Exists(outdir)) Directory.CreateDirectory(outdir);

                Console.WriteLine($"{DateTime.Now}: Received Answer, request id: {a.RequestId}");

                foreach (var artifact in a.Artifacts)
                {
                    if (artifact.FinishReason == FinishReason.Filter)
                    {
                        Console.Write($"Content filtered, request id: {a.RequestId}, reasons: ");
                        foreach (var category in artifact.Classifier.Categories) Console.Write($"{category.Name}");
                        Console.WriteLine();

                        if (dumpFilteredToJson) DumptoJson(artifact, $"filtered_{a.RequestId}_{artifact.Index}.json");

                        continue;
                    };

                    if (saveMimeTypesWithExtension.ContainsKey(artifact.Mime))
                    {
                        var ext = saveMimeTypesWithExtension[artifact.Mime];

                        string fileMask = $"{outdir}{i++:D5}.{ext}";
                        while (File.Exists(fileMask))
                        {
                            fileMask = $"{outdir}{i++:D5}.{ext}";
                        }
                        Console.WriteLine($"{DateTime.Now}: Received image. Seed: {artifact.Seed}");
                        Console.WriteLine($"{DateTime.Now}: Writing: {fileMask}");

                        await File.WriteAllBytesAsync($"{fileMask}", artifact.Binary.ToByteArray());
                    }
                }
            };
        }
    }
}
