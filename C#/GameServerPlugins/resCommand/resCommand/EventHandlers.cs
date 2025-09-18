using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using Microsoft.SqlServer.Server;
using PlayerRoles;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace SCPScape
{
    public class EventHandlers : CustomEventsHandler
    {
        public static bool hasSpawned = false;
        public override void OnServerRoundStarted()
        {
            Timing.RunCoroutine(Cor());
            hasSpawned = false;
        }
        IEnumerator<float> Cor()
        {
            yield return Timing.WaitForSeconds(180f);
            hasSpawned = true;
        }
        public static void spawnDClass(Player player)
        {
            player.SetRole(RoleTypeId.ClassD, flags: RoleSpawnFlags.All);
        }

    }
}
