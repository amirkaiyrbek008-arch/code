using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using UnityEngine;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System.Collections.Generic;
using MEC;
using LabApi.Events.Arguments.Scp173Events;
namespace evacutation
{
    public class EventHandlers : CustomEventsHandler
    {
        bool isAccessed = false;
        bool isinUse = false;
        bool isActivated = false;
        string cassieLineTrue = "";
        string cassieLineFalse = "";
        Vector3 centerOfCall = new Vector3(0f, 0f, 0f);
        Vector3 centerOfHelicopter = new Vector3(0f, 0f, 0f);
        float radius = 3f;
        CoroutineHandle handler;

        IEnumerator<float> inUse(Player plr)
        {
            isinUse = true;
            if (plr.Role == RoleTypeId.NtfCaptain || plr.Role == RoleTypeId.NtfSpecialist)
            {
                isAccessed = true;
            }

            else if(plr.Role == RoleTypeId.Scientist && plr.CustomInfo == "Менеджер Зоны")
            {
                isAccessed = true;
            }

            else
            {
                isAccessed = false;
            }
            while (plr.IsUsingRadio)
            {
                yield return Timing.WaitForSeconds(0.5f);
            }
            if(isAccessed)
            {
                Cassie.Message(cassieLineTrue);
                isinUse = false;
                isActivated = true;
            }
            else
            {
                Cassie.Message(cassieLineFalse);
                isinUse = false;
            }
;
        }
        public override void OnPlayerUsingRadio(PlayerUsingRadioEventArgs ev)
        {
            if(ev.Radio.RangeLevel == InventorySystem.Items.Radio.RadioMessages.RadioRangeLevel.HighRange)
            {
                float dist = Vector3.Distance(ev.Player.Position, centerOfCall);
                if (dist < radius)
                {
                    if (!isinUse && !isActivated)
                    {
                        Timing.RunCoroutine(inUse(ev.Player));
                    }
                    else
                    {
                        ev.Player.SendHint("Эвакуационный вертолет уже в пути", 5f);
                    }
                }
                else { return; }
            }

            else { return; }
        }

        public override void OnServerWaveRespawning(WaveRespawningEventArgs ev)
        {
            ev.IsAllowed = false;
            
        }
    }
}
