using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using UnityEngine;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System.Collections;
using MEC;
using LabApi.Events.Arguments.Scp173Events;
namespace CIClear
{
    public class EventHandlers : CustomEventsHandler
    {
        public override void OnServerWaveRespawned(WaveRespawnedEventArgs ev)
        {
            Timing.CallDelayed(1.5f, () =>
            {
                Cassie.Clear();
            });
        }   
    }
}
