using Grpc.Core;
using Grpc.Net.Client;
using StabilitySdkClient.Dashboard;
using StabilitySdkClient.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabilitySdkClient
{
    public sealed class DashboardClient : AbstractClient
    {
        public DashboardClient(Metadata metadata) : base(metadata)
        {
        }

        private EmptyRequest _emptyRequest { get; } = new EmptyRequest();

        // CURRENTLY UNSUPPORTED BY API KEY

        //public async Task GetClientSettingsAsync(Action<ClientSettings> handler)
        //{
        //    using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
        //    {
        //        var client = new DashboardService.DashboardServiceClient(channel);

        //        try 
        //        { 
        //            var reply = await client.GetClientSettingsAsync(_emptyRequest, Metadata);

        //            handler(reply);
        //        }
        //        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unauthenticated)
        //        {
        //            Console.WriteLine($"{DateTime.Now}: Authorization failed.  Check API key.  Aborting.");
        //        }                
        //    }
        //}

        //public async Task GetUser(Action<User> handler)
        //{
        //    using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
        //    {
        //        var client = new DashboardService.DashboardServiceClient(channel);

        //        var emptyRequest = new EmptyRequest();

        //        try
        //        {
        //            var user = await client.GetMeAsync(_emptyRequest, Metadata);

        //            handler(user);
        //        }
        //        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unauthenticated)
        //        {
        //            Console.WriteLine($"{DateTime.Now}: Authorization failed.  Check API key.  Aborting.");
        //        }
        //    }
        //}

        //public async Task GetCharges(Action<Charges> handler)
        //{
        //    using var channel = GrpcChannel.ForAddress(API_SERVER_ADDRESS);
        //    {
        //        var client = new DashboardService.DashboardServiceClient(channel);

        //        var emptyRequest = new GetChargesRequest() { OrganizationId = "org-TnkZDbyUy6RfbSBwIrofKUbT", RangeFrom = 0, RangeTo = 99 };

        //        try
        //        {
        //            var charges = await client.GetChargesAsync(emptyRequest, Metadata);

        //            handler(charges);
        //        }
        //        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unauthenticated)
        //        {
        //            Console.WriteLine($"{DateTime.Now}: Authorization failed.  Check API key.  Aborting.");
        //        }
        //    }
        //}

    }
}
