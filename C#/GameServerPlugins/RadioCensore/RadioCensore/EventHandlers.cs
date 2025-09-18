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
        private readonly Dictionary<Player, string> oldNicknames = new Dictionary<Player, string>();

        public override void OnPlayerUsingRadio(PlayerUsingRadioEventArgs ev)
        {
            if (ev.Player.IsHuman && ev.Player.IsUsingRadio)
            {
                if (!oldNicknames.ContainsKey(ev.Player))
                {
                    oldNicknames[ev.Player] = ev.Player.DisplayName;
                    ev.Player.DisplayName = "???";
                    Timing.RunCoroutine(RadioCheck(ev.Player));
                }
            }
        }

        private IEnumerator<float> RadioCheck(Player plr)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.1f);

                if (!plr.IsUsingRadio)
                {
                    if (oldNicknames.TryGetValue(plr, out string originalName))
                    {
                        int index = originalName.ToLower().IndexOf("color");
                        if (index >= 0) 
                        {
                            originalName = originalName.Substring(0, index);
                        }

                        plr.DisplayName = originalName;
                        oldNicknames.Remove(plr);
                    }
                    yield break;
                }
            }
        }
    }
}
