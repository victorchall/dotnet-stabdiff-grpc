using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StabilitySdkClient.Engines;

namespace StabilitySdkClient
{
    public sealed class EnginesClient : AbstractClient
    {
        public EnginesClient(Metadata metadata) : base(metadata)
        {
        }

        private ListEnginesRequest _listEnginesRequest = new();

        public async Task GetEngines(Action<EngineInfo> handler)
        {
            using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
            {
                var client = new EnginesService.EnginesServiceClient(channel);

                var engines = await client.ListEnginesAsync(_listEnginesRequest, _metadata);

                foreach (var engine in engines.Engine)
                {
                    handler(engine);
                }
            }
        }
    }
}
