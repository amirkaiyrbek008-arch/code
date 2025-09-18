using AdminStats;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using System;

namespace AdminStats
{
    public class AdminStats : Plugin
    {
        public override string Name => "AdminStats";
        public override string Description => "Админ статы";
        public override string Author => "SalReI";
        public override System.Version Version => new System.Version(1, 0, 0, 0);
        public override System.Version RequiredApiVersion => new System.Version(LabApiProperties.CompiledVersion);

        public static AdminStats Instance;


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