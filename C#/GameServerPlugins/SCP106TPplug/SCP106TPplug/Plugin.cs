using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using scp106Teleport;

namespace SCP966
{
    public class SCP966 : Plugin
    {
        public override string Name => "Scp-966";
        public override string Description => "Бессонник";
        public override string Author => "SalReI";
        public override System.Version Version => new System.Version(1, 0, 0, 0);
        public override System.Version RequiredApiVersion => new System.Version(LabApiProperties.CompiledVersion);

        public static SCP966 Instance;


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
