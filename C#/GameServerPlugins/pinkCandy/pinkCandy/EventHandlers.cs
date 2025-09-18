using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using System;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using UnityEngine;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System.Collections.Generic;
using InventorySystem.Items.Usables.Scp330;
using LabApi.Features.Extensions;
using CandyUtilities;
namespace pinkCandy
{
    public class EventHandlers : CustomEventsHandler
    {
        Dictionary<Player, int> candies = new Dictionary<Player, int>();

        public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
        {
            candies[ev.Player] = 0;
        }

        public override void OnPlayerInteractingScp330(PlayerInteractingScp330EventArgs ev)
        {
            candies.TryGetValue(ev.Player, out int value);

            if (value >= 3)
            {
                ev.Player.EnableEffect<CustomPlayerEffects.SeveredHands>(1);
                return;
            }

            if (UnityEngine.Random.Range(0, 100) < 10)
            {
                ev.IsAllowed = false;

                Scp330Bag.TryAddCandy()

                candies[ev.Player] = value + 1;
            }
            else
            {
                candies[ev.Player] = value + 1;
            }
        }
    }
}