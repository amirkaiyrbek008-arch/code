using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabApi.Events.Arguments.ServerEvents;
using PlayerRoles;
using LabApi.Features.Wrappers;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using GameCore;
using MEC;
namespace CassieCIRemove
{
    public class EventHandlers : CustomEventsHandler
    {
        public override void OnServerWaveRespawning(WaveRespawningEventArgs ev)
        {
            foreach (Player player in ev.SpawningPlayers)
            {
                if (player.Role == RoleTypeId.ChaosRepressor || player.Role == RoleTypeId.ChaosMarauder || player.Role == RoleTypeId.ChaosRifleman || player.Role == RoleTypeId.ChaosConscript)
                    {
                    Timing.CallDelayed(1.5f, () => Cassie.Clear());
                    return;
                    }   
                }

        }
    }
}
