git submodule update --init --recursive
copy "api-interfaces/src/proto/" "StabilitySdkClient/protos" /Y
ECHO option csharp_namespace = "StabilitySdkClient.Completion"; >> "StabilitySdkClient\protos\completion.proto"
ECHO option csharp_namespace = "StabilitySdkClient.Dashboard"; >> "StabilitySdkClient\protos\dashboard.proto"
ECHO option csharp_namespace = "StabilitySdkClient.Engines"; >> "StabilitySdkClient\protos\engines.proto"
ECHO option csharp_namespace = "StabilitySdkClient.Generation"; >> "StabilitySdkClient\protos\generation.proto"
dotnet build