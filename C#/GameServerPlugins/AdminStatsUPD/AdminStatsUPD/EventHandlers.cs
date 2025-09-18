
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdminStats
{
    public class EventHandlers : CustomEventsHandler
    {
        public static Dictionary<string, int> playTime = new Dictionary<string, int>();
        public static Dictionary<string, string> names = new Dictionary<string, string>();
        private CoroutineHandle handler;

        public EventHandlers()
        {
            playTime = PlayerTimeStorage.Load();
            names = PlayerTimeStorage.LoadNames();
            handler = Timing.RunCoroutine(PerMinute());
        }

        private IEnumerator<float> PerMinute()
        {
            while (true)
            {
                foreach (var key in playTime.Keys.ToList())
                {
                    var player = Player.List.FirstOrDefault(p => p.UserId == key);
                    if (player != null)
                    {
                        playTime[key] += 1;
                    }
                }

                PlayerTimeStorage.Save(playTime);
                PlayerTimeStorage.SaveNames(names);
                yield return Timing.WaitForSeconds(60f);
            }
        }

        public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
        {
            string id = ev.Player.UserId;

            if (!playTime.ContainsKey(id))
            {
                if (ev.Player.ReferenceHub.serverRoles.RemoteAdmin)
                {
                    playTime.Add(id, 0);
                }
            }

            if (!names.ContainsKey(id))
            {
                if (ev.Player.ReferenceHub.serverRoles.RemoteAdmin)
                {
                    names.Add(id, ev.Player.Nickname);
                }
            }
        }

        public override void OnServerRoundRestarted()
        {
            Timing.KillCoroutines(handler);
        }
    }
}
