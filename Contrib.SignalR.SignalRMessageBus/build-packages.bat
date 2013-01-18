rmdir release /S /Q

mkdir release

.nuget\nuget.exe pack "Contrib.SignalR.SignalRMessageBus\Contrib.SignalR.SignalRMessageBus.csproj" -Prop Configuration=Release -OutputDirectory release

.nuget\nuget.exe pack "Contrib.SignalR.SignalRMessageBus.Backend\Contrib.SignalR.SignalRMessageBus.Backend.csproj" -Prop Configuration=Release -OutputDirectory release