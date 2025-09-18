using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using Microsoft.SqlServer.Server;
using PlayerRoles;
using RemoteAdmin.Communication;
using System.Collections.Generic;
using UnityEngine;

namespace CustomTowerTP
{
    public class EventHandlers : CustomEventsHandler
    {
        private readonly Vector3 Center = new Vector3(40f, 314.08f, -32.6f);
        private readonly Vector3 CustomTower = new Vector3(161.909f, 319.5153f, -13.06434f);
        private const float Radius = 2f;
        public override void OnPlayerSpawning(PlayerSpawningEventArgs ev)
        {
            Timing.RunCoroutine(coroutin(ev.Player));
        }

        IEnumerator<float> coroutin(Player player)
        {
            yield return Timing.WaitForSeconds(0.1f);
 
            float dist = Vector3.Distance(player.Position, Center);
 
            if (dist <= Radius)
            {
 
                player.Position = CustomTower;

            }
 
 
        }

    }
}
