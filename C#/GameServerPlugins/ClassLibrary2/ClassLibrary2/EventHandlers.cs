using LabApi.Events.CustomHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabApi.Features.Wrappers;
using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles;
using UnityEngine;
using MEC;
using LabApi.Features.Extensions;
using UnityEngine.Windows;
using System.Runtime.InteropServices;
using System.Security.Policy;


namespace Containment106
{
    public class EventHandlers : CustomEventsHandler
    {
        private readonly Vector3 CenterOfVictim = new Vector3(-7.06f, -99.02f, 131.14f);
        private const float RadiusOfVictim = 0.7f;

        private readonly Vector3 CenterOfPresser = new Vector3(-4.14f, -99.04f, 122.03f);
        private const float RadiusOfPresser = 0.3f;
        CoroutineHandle handler;
        string cassieLine = "<size=0>aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa<split></b>ОУС <color=red><b>SCP-106</b> [Класс: Кетер]</color> <color=green>успешно восстановлены.\r\n<size=0> pitch_0.19 .g3 pitch_0.18 .g3 pitch_0.95 .  Keter Class SCP 1 0 6 has been successfully recontained pitch_0.80 .g5 .g5 pitch_0.50 .g3";
        IEnumerator<float> checkBut()
        {
            while (true)
            {
                foreach (Player player in Player.List)
                {
                    if (!player.IsSCP)
                    {
                        buttonPress(player);
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
        }

        IEnumerator<float> checkCont()
        {
            foreach (Player player in Player.List)
            {
                if (!player.IsSCP)
                {
                    playerContainment106(player);
                }
            }
            yield return Timing.WaitForSeconds(1);
        }

        IEnumerator<float> checkSCP106(Player victim)
        {
            foreach (Player player in Player.List)
            {
                if(player.Role == RoleTypeId.Scp106)
                {
                    player.Kill("Условия содержания восстановлены");
                    player.SetRole(RoleTypeId.Spectator);
                    victim.SetRole(RoleTypeId.Spectator);
                    AudioPlayer audioPlayer = AudioPlayer.CreateOrGet("AnnouncerOfScream");
                    Speaker speaker = audioPlayer.GetOrAddSpeaker("AnnouncerOfScream", 1f, false, 5f, 5000f);
                    AudioClipPlayback audioClipPlayback = audioPlayer.AddClip("scream", 1f, false, true);
                    Timing.CallDelayed(25f, () =>
                    {
                        Cassie.Message(cassieLine);
                    });
                }
            }
            yield return Timing.WaitForSeconds(1);
        }
        public void buttonPress(Player player)
        {
            float dist = Vector3.Distance(player.Position, CenterOfPresser);
            if (dist <= RadiusOfPresser)
            {
                Timing.RunCoroutine(checkCont());
            }
            else
            {
                return;
            }

        }

        public void playerContainment106(Player player)
        {
            float dist = Vector3.Distance(player.Position, CenterOfVictim);

            if (dist <= RadiusOfVictim)
            {
                if (player.Role.IsHuman())
                {
                    Timing.RunCoroutine(checkSCP106(player));
                    
                }

                else
                {
                    return;
                }
            }
        }

        public override void OnServerRoundStarted()
        {
            handler = Timing.RunCoroutine(checkBut());
        }

        public override void OnServerRoundRestarted()
        {
            Timing.KillCoroutines(handler);
        }
    }
}
