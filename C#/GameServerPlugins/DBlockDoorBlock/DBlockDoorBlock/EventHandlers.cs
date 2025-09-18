using GameCore;
using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Enums;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using Microsoft.SqlServer.Server;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Class1
{
    public class EventHandlers : CustomEventsHandler
    {

        private IEnumerator<float> BlockDDoors()
        {
            Room dClassRoom = Room.Get(RoomName.LczClassDSpawn).FirstOrDefault();

            foreach (var door in dClassRoom.Doors)
            {
                door.Lock(DoorLockReason.AdminCommand, true );
            }
            yield return Timing.WaitForSeconds(20);
 
            foreach (var door in dClassRoom.Doors)
            {
                door.IsLocked = false;
                if (!door.IsOpened)
                {
                    door.IsOpened = true;

                }

            }

        }

        public override void OnServerRoundStarted()
        {
            Timing.RunCoroutine(BlockDDoors());
        }

        public override void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev)
        {
            Room dclassRoom = Room.Get(RoomName.LczClassDSpawn).FirstOrDefault();

            foreach(Door door in dclassRoom.Doors)
            {
                if (door == ev.Door)
                {
                    ev.IsAllowed = false;
                }
            }
        }
    }
}
