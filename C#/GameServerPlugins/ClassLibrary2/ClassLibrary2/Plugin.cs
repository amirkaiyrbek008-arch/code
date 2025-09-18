using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace Containment106
{
    public class Containment106 : Plugin
    {
        public override string Name => "Containment106";
        public override string Description => "Контейнмент 106";
        public override string Author => "SalReI";
        public override System.Version Version => new System.Version(1, 0, 0, 0);
        public override System.Version RequiredApiVersion => new System.Version(LabApiProperties.CompiledVersion);

        public static Containment106 Instance;


        public EventHandlers Events { get; } = new EventHandlers();

        public override void Enable()
        {
            Instance = this;
            LoadConfigs();
            CustomHandlersManager.RegisterEventsHandler(Events);
            AudioClipStorage.LoadClip("/root/scpsl/mandarin_scream.ogg", "scream");
        }

        public override void Disable()
        {
            CustomHandlersManager.UnregisterEventsHandler(Events);
            Instance = this;
        }
    }
}
