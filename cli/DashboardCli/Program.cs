using StabilitySdkClient;
using StabilitySdkClient.Dashboard;

//Action<Charges> chargesHandler = (charges) =>
//{
//    foreach(var charge in charges.Charges_)
//    {
//        Console.WriteLine($"Credits: {charge.AmountCredits}, paid: {charge.Paid}, id: {charge.Id}, created at: {charge.CreatedAt}, receiptlink: {charge.ReceiptLink}, paymentlink: {charge.PaymentLink}");
//    }
//    var dumpFile = System.Text.Json.JsonSerializer.Serialize(charges);

//    File.WriteAllText("q:/projects/dotnet-stabdiff-grpc/charges.json", dumpFile);
//};

//var metadata = GeneratorClient.CreateMetaData(Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty);

//var client = new DashboardClient(metadata);

//await client.GetCharges(chargesHandler);
Console.WriteLine("Currently unsupported by API KEY");
