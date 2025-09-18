using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RadioCensore
{
    public class EventHandlers : CustomEventsHandler
    {
        string oldNickname;
        public override void OnPlayerUsingIntercom(PlayerUsingIntercomEventArgs ev)
        {
            oldNickname = ev.Player.DisplayName.ToString();
            ev.Player.DisplayName = "???";
            if(ev.State.ToString() != "InUse")
            {
                int index = oldNickname.ToLower().IndexOf("color");
                if (index >= 0)
                {
                    oldNickname = oldNickname.Substring(0, index);
                }

                ev.Player.DisplayName = oldNickname;
                oldNickname = null;
            }
           
        }


    }
}
