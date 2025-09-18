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
        private readonly Vector3 escapeCenter = new Vector3(123.84f, 289.08f, 16.09f);
        private readonly Vector3 Center = new Vector3(-40.89f, 291.88f, -36.132f); 
        private const float Radius = 0.3f;
        private const float escapeRadius = 2.0f;
        CoroutineHandle handler;
        CoroutineHandle handlerSpec;
        public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
        {

            if (ev.Player.Role == RoleTypeId.ClassD || ev.Player.Role == RoleTypeId.Scientist)
            {

                ev.Player.SetRole(RoleTypeId.Spectator);
                return;
            }

            if (!ev.Player.IsSCP)
            {
                return; 
            }

            EscapeSCP(ev.Player);
        }

        IEnumerator<float> EscapeGateA()
        {
            while (true)
            {
                foreach (Player player in Player.List)
                {
                    gateAEscaping(player);
                }

                yield return Timing.WaitForSeconds(1);
            }
        }

        IEnumerator<float> forceSpectator()
        {
            while (true)
            {
                foreach (Player player in Player.List)
                {
                    escapeForceSpec(player);
                }

                yield return Timing.WaitForSeconds(1);
            }
        }

        public void gateAEscaping(Player player)
        {
            float dist = Vector3.Distance(player.Position, Center);

            if (dist <= Radius)
            {
                if (player.Role == RoleTypeId.ClassD || player.Role == RoleTypeId.Scientist)
                {
                    player.SetRole(RoleTypeId.Spectator);
                }
                if (player.Role.IsScp())
                {
                    EscapeSCP(player);
                }
                else
                {
                    return;
                }
            }
        }

        public void escapeForceSpec(Player player)
        {
            float dist = Vector3.Distance(player.Position, escapeCenter);
            if (dist <= escapeRadius)
            {
                if (player.Role == RoleTypeId.ClassD || player.Role == RoleTypeId.Scientist)
                {
                    player.SetRole(RoleTypeId.Spectator);
                }

                else
                {
                    return;
                }
            }
        }


        public override void OnServerRoundStarted()
        {
            handler = Timing.RunCoroutine(EscapeGateA());
            handlerSpec = Timing.RunCoroutine(forceSpectator());
        }

        public override void OnServerRoundRestarted()
        {
            Timing.KillCoroutines(handler);
            Timing.KillCoroutines(handlerSpec);
        }

        private void EscapeSCP(Player player)
        {
            string cassieLine = "";

            if (player.Role == RoleTypeId.Scp173) 
            {
                cassieLine = "pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 <color=red>Внимание.</color> <color=orange>SСP-173</color> сбежал из комплекса. <color=blue>МОГ эпсилон 11</color> отправилась на операцию. <color=#ffffff00> Attention . SCP 1 7 3 has escaped the facility . MTFunit Epsilon 11 arrival to operation . pitch_0.8 .g4 pitch_0.85 .g4  pitch_0.8 .g3 </color>";
            }

            if (player.Role == RoleTypeId.Scp096) 
            {
                cassieLine = "<color=red>Внимание.</color> <color=orange>SСP-096</color> сбежал из комплекса. <color=blue>МОГ Эта-10 </color> отправилась на операцию.  <color=#ffffff00>  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 Attention . SCP 0 9 6 has escaped the facility . Mtfunit Eta 10 arrival to operation  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 </color>";
            }
            if (player.Role == RoleTypeId.Scp939)
            {
                cassieLine = "<color=red>Внимание.</color> <color=red>SСP-939</color> сбежал из комплекса. <color=blue>МОГ НЮ-7 </color> отправилась на операцию.  <color=#ffffff00>  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 Attention . SCP 9 3 9 has escaped the facility . Mtfunit Nu 7 arrival to operation  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 </color>";
            }

            if (player.Role == RoleTypeId.Scp106)
            {
                cassieLine = "<color=red>Внимание.</color> <color=red>SСP-106</color> сбежал из комплекса. <color=blue>МОГ Дельта-5</color> отправилась на операцию.  <color=#ffffff00>  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 Attention . SCP 1 0 6 has escaped the facility . Mtfunit nato_d 5 arrival to operation  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 </color>";
            }

            if (player.Role == RoleTypeId.Scp049)
            {
                cassieLine = "<color=red>Внимание.</color> <color=orange>SСP-049</color> сбежал из комплекса. <color=blue>МОГ Дельта-5</color> отправилась на операцию.  <color=#ffffff00>  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 Attention . SCP 0 4 9 has escaped the facility . Mtfunit nato_d 5 arrival to operation  pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 </color>";
            }
            if (player.Role == RoleTypeId.Scp3114)
            {
                cassieLine = "pitch_0.2 .g3 pitch_0.19 .g3 pitch_1 <color=red>Внимание.</color> <color=orange>SСP-3114</color> сбежал из комплекса. <color=blue>МОГ эпсилон 11</color> отправилась на операцию. <color=#ffffff00> Attention . SCP 3 1 1 4 has escaped the facility . MTFunit Epsilon 11 arrival to operation . pitch_0.8 .g4 pitch_0.85 .g4  pitch_0.8 .g3 </color>\r\n";
            }

            string name = player.Nickname;
            player.SetRole(RoleTypeId.Spectator);
            Cassie.Message(cassieLine);
        }


    }
}
