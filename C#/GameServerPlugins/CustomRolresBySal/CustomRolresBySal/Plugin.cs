using CustomRolesBySal;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using System;

namespace CustomRolesBySal
{
    public class cageFor173 : Plugin
    {
        public override string Name => "CustomRoles";
        public override string Description => "Кастомные роли";
        public override string Author => "SalReI";
        public override System.Version Version => new System.Version(1, 0, 0, 0);
        public override System.Version RequiredApiVersion => new System.Version(LabApiProperties.CompiledVersion);

        public static cageFor173 Instance;
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
