using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.Arguments.Scp096Events;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.Arguments.Scp914Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Enums;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace HackOfGates
{
    public class EventHandlers : CustomEventsHandler
    {
        CoroutineHandle handler;

        Door gateA = null;
        Door gateB = null;
        int ciValA = 0;
        int ntfValA = 0;
        int ntfValB = 0;
        int ciValB = 0;
        public override void OnServerRoundStarted()
        {
            Timing.CallDelayed(1.5f, () =>
            {
                foreach (Door dr in Door.List)
                {
                    if (dr.DoorName == DoorName.EzGateA)
                    {
                        gateA = dr;
                        dr.IsLocked = true;
                    }
                    else if (dr.DoorName == DoorName.EzGateB)
                    {
                        gateB = dr;
                        dr.IsLocked = true;
                    }
                }

                Timing.RunCoroutine(lockedGates());

            });
        }
            IEnumerator<float> lockedGates()
            {
                yield return Timing.WaitForSeconds(600);
                gateA.IsLocked = false;
                gateB.IsLocked = false;
            }
            IEnumerator<float> ntfGates(Player ntf, Door door)
            {
                float varr = 10.0f;
                while (Math.Round(varr,1) >= 0f)
                {
                    if(ntf.CurrentItem != null && ntf.CurrentItem.Type == ItemType.Radio)
                    {
                        if(Math.Round(varr,1) <= 0)
                        {
                            if(door.DoorName == DoorName.EzGateA)
                            {
                                ntfValA--;
                            }
                            else if(door.DoorName == DoorName.EzGateB)
                            {
                                ntfValB--;
                            }
                            ntf.SendHint("Гермоворота была успешно открыта", 5f);
                            door.IsOpened = true;
                            yield return Timing.WaitForSeconds(10f);
                            door.IsOpened = false;
                            break;

                        }
                        if (varr > 4f)
                        {
                            ntf.SendHint($"Гермоворота откроется через: {Math.Round(varr, 1, MidpointRounding.AwayFromZero)} секунд");

                        }
                        else if (varr <= 4f && varr > 1f)
                        {
                            ntf.SendHint($"Гермоворота откроется через: {Math.Round(varr, 1, MidpointRounding.AwayFromZero)} секунды");
                        }

                        else if (varr <= 1f)
                        {
                            ntf.SendHint($"Гермоворота откроется через:{Math.Round(varr, 1, MidpointRounding.AwayFromZero)} секунду");
                        }
                        varr -= 0.1f;
                    }
                    else
                    {
                        ntf.SendHint("Процесс открытия гермоворот был прерван", 5f);
                        varr = 10f;
                        if (door.DoorName == DoorName.EzGateA)
                        {
                            ntfValA--;
                        }
                        else if (door.DoorName == DoorName.EzGateB)
                        {
                            ntfValB--;
                        }
                    break;
                    }
                    yield return Timing.WaitForSeconds(0.1f);

                }

            }

        IEnumerator<float> ntfGatesclose(Player ntf, Door door)
        {
            float varr2 = 10.0f;
            while (Math.Round(varr2,1) >= 0f)
            {
                if(ntf.CurrentItem != null && ntf.CurrentItem.Type == ItemType.Radio)
                {
                    if(Math.Round(varr2,1) <= 0)
                    {
                        if (door.DoorName == DoorName.EzGateA)
                        {
                            ntfValA--;
                        }
                        else if (door.DoorName == DoorName.EzGateB)
                        {
                            ntfValB--;
                        }
                        ntf.SendHint("Гермоворота была закрыта", 5f);
                        door.IsOpened = false;
                        break;
                    }
                    if (varr2 > 4f)
                    {
                        ntf.SendHint($"Гермоворота закроется через: {Math.Round(varr2, 1, MidpointRounding.AwayFromZero)} секунд");

                    }
                    else if (varr2 <= 4f && varr2 > 1f)
                    {
                        ntf.SendHint($"Гермоворота закроется через: {Math.Round(varr2, 1, MidpointRounding.AwayFromZero)} секунды");
                    }

                    else if (varr2 <= 1f)
                    {
                        ntf.SendHint($"Гермоворота закроется через:{Math.Round(varr2, 1, MidpointRounding.AwayFromZero)} секунду");
                    }
                    varr2 -= 0.1f;

                }

                else
                {
                    ntf.SendHint("Процесс закрытия гермоворот был прерван", 5f);
                    varr2 = 10f;
                    if (door.DoorName == DoorName.EzGateA)
                    {
                        ntfValA--;
                    }
                    else if (door.DoorName == DoorName.EzGateB)
                    {
                        ntfValB--;
                    }
                    break;

                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }

        IEnumerator<float> ciGates(Player ci, Door door)
        {
            float varr1 = 25.0f;
            while (Math.Round(varr1,1) >= 0f)
            {
                if(ci.CurrentItem != null && ci.CurrentItem.Type == ItemType.KeycardChaosInsurgency)
                {
                    if(varr1 > 0)
                    {
                        ci.SendHint($"Взлом Гермоворот\nГермоворота откроется через {Math.Round(varr1, 1)} секунд");
                        varr1 -= 0.1f;
                    }
                    else
                    {
                        if(door.DoorName == DoorName.EzGateA)
                        {
                            ciValA--;
                        }

                        else if (door.DoorName == DoorName.EzGateB)
                        {
                            ciValB--;
                        }
                        door.IsOpened = true;
                        ci.SendHint("Гермоворота была октрыта", 5f);
                        break;
                    }

                }

                else
                {
                    ci.SendHint("Процесс открытия гермоворот был прерван", 5f);
                    if (door.DoorName == DoorName.EzGateA)
                    {
                        ciValA--;
                    }

                    else if (door.DoorName == DoorName.EzGateB)
                    {
                        ciValB--;
                    }
                    break;
                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }

        public override void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev)
        {
            if (ev.Door.DoorName == DoorName.EzGateA || ev.Door.DoorName == DoorName.EzGateB)
            {
                if(!ev.Door.IsOpened)
                {
                    if (ev.Player.Role == RoleTypeId.NtfSergeant || ev.Player.Role == RoleTypeId.NtfSpecialist || ev.Player.Role == RoleTypeId.NtfCaptain)
                    {
                        if (ev.Player.CurrentItem.Type == ItemType.Radio)
                        {
                            if(ev.Door.DoorName == DoorName.EzGateA)
                            {
                                if (ntfValA == 0)
                                {
                                    ntfValA++;
                                    ev.IsAllowed = false;
                                    Timing.RunCoroutine(ntfGates(ev.Player, ev.Door));
                                }
                                else
                                {
                                    ev.IsAllowed = false;
                                    ev.Player.SendHint("Уже кто-то другой занимается с открытием геромоворот");
                                }
                            }
                            else if(ev.Door.DoorName == DoorName.EzGateB)
                            {
                                if (ntfValB == 0)
                                {
                                    ntfValB++;
                                    ev.IsAllowed = false;
                                    Timing.RunCoroutine(ntfGates(ev.Player, ev.Door));
                                }
                                else
                                {
                                    ev.IsAllowed = false;
                                    ev.Player.SendHint("Уже кто-то другой занимается с открытием геромоворот");
                                }
                            }
                        }
                    }

                    if (ev.Player.Role.IsChaos())
                    {
                        if (ev.Player.CurrentItem.Type == ItemType.KeycardChaosInsurgency)
                        {
                            if(ev.Door.DoorName == DoorName.EzGateA)
                            {
                                if (ciValA == 0)
                                {
                                    ciValA++;
                                    ev.IsAllowed = false;
                                    Timing.RunCoroutine(ciGates(ev.Player, ev.Door));
                                }
                                else
                                {
                                    ev.IsAllowed = false;
                                    ev.Player.SendHint("Уже кто-то другой занимается с открытием геромоворот");
                                }

                            }

                            else if(ev.Door.DoorName == DoorName.EzGateB)
                            {
                                if (ciValB == 0)
                                {
                                    ciValB++;
                                    ev.IsAllowed = false;
                                    Timing.RunCoroutine(ciGates(ev.Player, ev.Door));
                                }
                                else
                                {
                                    ev.IsAllowed = false;
                                    ev.Player.SendHint("Уже кто-то другой занимается с открытием геромоворот");
                                }
                            }
                        }
                    }
                }

                else
                {
                    if(ev.Player.Role == RoleTypeId.NtfSergeant || ev.Player.Role == RoleTypeId.NtfSpecialist || ev.Player.Role == RoleTypeId.NtfCaptain)
                    {
                        if(ev.Player.CurrentItem.Type == ItemType.Radio)
                        {
                            if (ev.Door.DoorName == DoorName.EzGateA)
                            {
                                if (ntfValA == 0)
                                {
                                    ev.IsAllowed = false;
                                    ntfValA++;
                                    Timing.RunCoroutine(ntfGatesclose(ev.Player, ev.Door));
                                }
                                else
                                {
                                    ev.IsAllowed = false;
                                    ev.Player.SendHint("Уже кто-то другой занимается с открытием геромоворот");
                                }
                            }
                            else if (ev.Door.DoorName == DoorName.EzGateB)
                            {
                                if (ntfValB == 0)
                                {
                                    ev.IsAllowed = false;
                                    ntfValB++;
                                    Timing.RunCoroutine(ntfGatesclose(ev.Player, ev.Door));
                                }
                                else
                                {
                                    ev.IsAllowed = false;
                                    ev.Player.SendHint("Уже кто-то другой занимается с открытием геромоворот");
                                }
                            }

                        }
                    }
                    if(ev.Player.Role.IsChaos())
                    {
                        if(ev.Door.IsLocked)
                        {
                            ev.IsAllowed = false;
                        }
                    }
                }

            }
        }

    }
}
