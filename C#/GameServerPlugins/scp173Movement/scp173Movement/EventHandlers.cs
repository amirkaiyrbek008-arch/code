using System;
using LabApi.Events.CustomHandlers;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Interfaces;
using UnityEngine;
using GameCore;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using LabApi.Events.Arguments.PlayerEvents;

namespace move173   
{
	public class EventHandlers : CustomEventsHandler
	{
		public int amount_of_plrs = 0;
        Player scp173;
        Vector3 pos;
        IEnumerator<float> enumerator(Vector3 pos)
        {
            while(true)
            {
                if(scp173 == null)
                {
                    Cassie.Message("test1");
                    yield return Timing.WaitForSeconds(1f);
                    continue;
                }
                if (amount_of_plrs >= 3)
                {
                    Cassie.Message("test2");
                    scp173.Position = pos;
                }
                else
                {
                    Cassie.Message("test3");
                    pos = scp173.Position;
                }
                Cassie.Message(amount_of_plrs.ToString());
                yield return Timing.WaitForSeconds(0.1f);
            }

        }

        public override void OnServerRoundStarted()
        {
            Timing.CallDelayed(2f, () =>
            {
                Timing.RunCoroutine(enumerator(pos));
                foreach(Player plr in Player.List)
                {
                    if(plr.Role == RoleTypeId.Scp173)
                    {
                        scp173 = plr;
                        pos = scp173.Position;
                        break;
                    }
                }

            });
        }
        public override void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
        {
            amount_of_plrs++;
        }

        public override void OnScp173RemovingObserver(Scp173RemovingObserverEventArgs ev)
        {
            amount_of_plrs--;
        }
        public override void OnScp173CreatingTantrum(Scp173CreatingTantrumEventArgs ev)
        {
            if(amount_of_plrs >= 3)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        public override void OnScp173BreakneckSpeedChanging(Scp173BreakneckSpeedChangingEventArgs ev)
        {
            if (amount_of_plrs >= 3)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        public override void OnScp173PlayingSound(Scp173PlayingSoundEventArgs ev)
        {
            if (amount_of_plrs >= 3)
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
            if(ev.Attacker.Role == RoleTypeId.Scp173)
            {
                ev.IsAllowed = false;
            }
        }

        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if(ev.NewRole == RoleTypeId.Scp173 && scp173 == null)
            {
                scp173 = ev.Player;
            }
            else if(ev.OldRole.RoleTypeId == RoleTypeId.Scp173)
            {
                scp173 = null;
            }

        }
    }
}

