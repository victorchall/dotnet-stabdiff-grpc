git submodule update --init --recursive
cp -i -r 'api-interfaces/src/proto' 'StabilitySdkClient/protos'

read -p "option csharp_namespace = "StabilitySdkClient.Completion";" newtext
echo $newtext >> 'StabilitySdkClient\protos\completion.proto'

read -p "option csharp_namespace = "StabilitySdkClient.Completion";" newtext
echo $newtext >> 'StabilitySdkClient\protos\dashboard.proto'

read -p "option csharp_namespace = "StabilitySdkClient.Completion";" newtext
echo $newtext >> 'StabilitySdkClient\protos\engines.proto'

read -p "option csharp_namespace = "StabilitySdkClient.Completion";" newtext
echo $newtext >> 'StabilitySdkClient\protos\generation.proto'

dotnet build

