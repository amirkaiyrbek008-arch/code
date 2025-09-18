using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using Microsoft.SqlServer.Server;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace scp173
{
    public class EventHandlers : CustomEventsHandler
    {
        Player scp173;
        CoroutineHandle handlerl;
        Vector3 posofscp173;
        int amount_of_people = 0;
        IEnumerator<float> forceSpectator()
        {
            Cassie.Message("test1");
            while (true)
            {
                posofscp173 = scp173.Position;
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public override void OnServerRoundStarted()
        {
            foreach(Player player in Player.List)
            {
                if (player.Role == RoleTypeId.Scp173)
                {
                    scp173 = player;
                }
            }
            handlerl = Timing.RunCoroutine(forceSpectator());
        }

        public override void OnServerRoundRestarted()
        {
            Timing.KillCoroutines();
        }


        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp173)
            {
                scp173 = null;

            }
        }

        public override void OnPlayerDeath(PlayerDeathEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp173)
            {
                scp173 = null;
            }
        }

        public override void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
        {
            amount_of_people += 1;
        }

        public override void OnScp173RemovingObserver(Scp173RemovingObserverEventArgs ev)
        {
            amount_of_people -= 1;
        }

        public override void OnScp173BreakneckSpeedChanging(Scp173BreakneckSpeedChangingEventArgs ev)
        {
            if (amount_of_people >= 3)
            {
                ev.IsAllowed = false;
            }

            else
            {
                ev.IsAllowed = true;
            }
        }
        public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
        {
            if (ev.Attacker.Role == RoleTypeId.Scp173 && amount_of_people >= 3) 
            {
                ev.IsAllowed = false;
                ev.Attacker.Position = posofscp173;
            }

            else
            {
                ev.IsAllowed = true;
            }
        }
    }
}
