using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace StabilitySdkClient
{
    public static class MetadataFactory
    {
        public static Metadata CreateMetaData(string apiKey)
        {
            return new Metadata() { new Entry("authorization", $"bearer {apiKey}") };
        }
    }
}

