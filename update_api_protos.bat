git submodule update --init --recursive

COPY "api-interfaces/src/proto/" "StabilitySdkClient/proto" /Y

ECHO option csharp_namespace = "StabilitySdkClient.Completion"; >> "StabilitySdkClient\proto\completion.proto"
ECHO option csharp_namespace = "StabilitySdkClient.Dashboard"; >> "StabilitySdkClient\proto\dashboard.proto"
ECHO option csharp_namespace = "StabilitySdkClient.Engines"; >> "StabilitySdkClient\proto\engines.proto"
ECHO option csharp_namespace = "StabilitySdkClient.Generation"; >> "StabilitySdkClient\proto\generation.proto"

dotnet build