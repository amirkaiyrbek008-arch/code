using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Arguments.Scp106Events;
using PlayerRoles;
using LabApi.Features.Wrappers;
using LabApi.Features.Enums;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;
using MEC;
using LabApi.Events.CustomHandlers;
namespace scp106Teleport
{
    public class EventHandlers : CustomEventsHandler
    {
        Player scp106 = null;
        Vector3 oldPos;
        List<Player> playerList = new List<Player>();
        int varr = 0;

        IEnumerator<float> scp106Checklist()
        {
            while (playerList.Count > 0)
            {
                yield return Timing.WaitForSeconds(1);
            }

            scp106.Position = oldPos;
        }
        public override void OnServerRoundStarted()
        {
            Timing.CallDelayed(1.5f, () =>
            {
                foreach (Player plr in Player.List)
                {
                    if (plr.Role == RoleTypeId.Scp106)
                    {
                        scp106 = plr;
                    }
                }
            });
        }
        public override void OnPlayerEnteringPocketDimension(PlayerEnteringPocketDimensionEventArgs ev)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                oldPos = scp106.Position;
                scp106.Position = ev.Player.Position + new Vector3(0f, 0f, 0f);
                playerList.Add(ev.Player);
                varr++;
                if (!Timing.RunCoroutine(scp106Checklist()).IsRunning)
                {
                    Timing.RunCoroutine(scp106Checklist());
                }
            });
        }

        public override void OnPlayerLeavingPocketDimension(PlayerLeavingPocketDimensionEventArgs ev)
        {
            playerList.Remove(ev.Player);
            varr--;
        }

        public override void OnPlayerDeath(PlayerDeathEventArgs ev)
        {   
            if(Timing.RunCoroutine(scp106Checklist()).IsRunning)
            {
                foreach (Player plr in playerList)
                {
                    if (plr == ev.Player)
                    {
                        playerList.Remove(ev.Player);
                        varr--;
                    }
                }
            }
        }

        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleTypeId.Scp106)
            {
                scp106 = ev.Player;
            }

        }

        public override void OnPlayerLeft(PlayerLeftEventArgs ev)
        {
            if(ev.Player == scp106)
            {
                scp106 = null;
            }
        }
    }
}