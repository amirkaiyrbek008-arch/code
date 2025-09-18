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
using UnityEngine;

namespace SCPScape
{
    public class EventHandlers : CustomEventsHandler
    {
     
        public static void kill(Player player)
        {
            player.Kill("Команда .kill");
        }

    }
}
