using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace StabilitySdkClient
{
    public abstract class AbstractClient
    {
        protected const string API_SERVER_ADDRESS = "https://grpc.stability.ai:443";

        protected Metadata _metadata { get; }

        public AbstractClient(Metadata metadata)
        {
            _metadata = metadata;
        }

    }
}
