git submodule update --init --recursive

cp -avr api-interfaces/src/proto StabilitySdkClient

echo $'\noption csharp_namespace = "StabilitySdkClient.Completion";' >> 'StabilitySdkClient\proto\completion.proto'
echo $'\noption csharp_namespace = "StabilitySdkClient.Dashboard";' >> 'StabilitySdkClient\proto\dashboard.proto'
echo $'\noption csharp_namespace = "StabilitySdkClient.Engines";' >> 'StabilitySdkClient\proto\engines.proto'
echo $'\noption csharp_namespace = "StabilitySdkClient.Generation";' >> 'StabilitySdkClient\proto\generation.proto'

dotnet build
