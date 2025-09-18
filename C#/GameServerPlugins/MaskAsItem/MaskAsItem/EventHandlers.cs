using System;
using LabApi.Events.CustomHandlers;
using UnityEngine;
using LabApi.Features.Wrappers;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp096Events;
using LabApi.Features.Extensions;
using MEC;
using PlayerRoles;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
namespace MaskAsItem
{
	public class EventHandlers : CustomEventsHandler
	{
        List<Item> masks = new List<Item>();

        Player scp096;

        bool isInMask;
        public SchematicObject schematicObject;

        public override void OnServerRoundStarted()
        {
            Timing.CallDelayed(0.1f, () =>
            {
                foreach(Player plr in Player.ReadyList)
                {
                    if(plr.Role == RoleTypeId.Scp096)
                    {
                        isInMask = false;
                        scp096 = plr;
                        break;
                    }
                }
            });
        }
        //public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        //{
        //    Timing.CallDelayed(0.05f, () =>
        //    {
        //        if (ev.Player.Role == RoleTypeId.NtfCaptain && ev.Player.CustomInfo == "E-11" && scp096 != null)  
        //        {
        //            //Item item = Item.
        //        }
        //    });
        //}
        public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
        {
            if(masks.Contains(ev.Item) && ev.Player.IsHuman)
            {
                ev.IsAllowed = false;
                Vector3 posOfScp = scp096.Position;
                float dist = Vector3.Distance(posOfScp, ev.Player.Position);
                if(dist < 4)
                {
                    ev.Player.RemoveItem(ev.Item);
                    masks.Remove(ev.Item);
                    isInMask = true;
                    posOfScp = new Vector3(scp096.Position.x, scp096.Position.y + 1.5f, scp096.Position.z);
                    schematicObject = ObjectSpawner.SpawnSchematic("paket", posOfScp, Vector3.zero, Vector3.one);
                    Timing.CallDelayed(0.05f, () =>
                    {
                        foreach (var k in schematicObject.AdminToyBases)
                        {
                            if (k != null)
                            {
                                k.IsStatic = false;

                            }
                        }

                    });
                    ev.Player.SendHint("Вы надели пакет на SCP-096", 3f);
                }
                else
                {
                    ev.Player.SendHint("Подойдите к SCP-096 ближе, чтобы надеть на него пакеи", 5f);
                }
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
        {
            if(ev.Player == scp096 && isInMask)
            {
                isInMask = false;
                schematicObject.Destroy();
            }
        }
        public override void OnScp096AddingTarget(Scp096AddingTargetEventArgs ev)
        {
            if(isInMask)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        public override void OnScp096ChangingState(Scp096ChangingStateEventArgs ev)
        {
            if (isInMask)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        public override void OnScp096Enraging(Scp096EnragingEventArgs ev)
        {
            if (isInMask)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        public override void OnScp096Charging(Scp096ChargingEventArgs ev)
        {
            if (isInMask)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        public override void OnScp096PryingGate(Scp096PryingGateEventArgs ev)
        {
            if (isInMask)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }
    }
}

