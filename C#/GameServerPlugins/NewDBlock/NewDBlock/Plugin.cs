using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using System;

namespace NoDBlock
{
    public class Plugin : LabApi.Loader.Features.Plugins.Plugin
    {
        public override string Name => "DblockClose";
        public override string Description => "Закрывает двери д блока после і30 секнуд";
        public override string Author => "SalReI";
        public override System.Version Version => new System.Version(1, 0, 0, 0);
        public override System.Version RequiredApiVersion => new System.Version(LabApiProperties.CompiledVersion);

        public static Plugin Instance;


        public EventHandlers Events { get; } = new EventHandlers();

        public override void Enable()
        {
            Instance = this;
            LoadConfigs();
            CustomHandlersManager.RegisterEventsHandler(Events);
        }

        public override void Disable()
        {
            CustomHandlersManager.UnregisterEventsHandler(Events);
            Instance = this;
        }

    }
}
