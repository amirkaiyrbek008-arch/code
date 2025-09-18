using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using System.Numerics;
using UnityEngine;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System.Collections;
using MEC;
using LabApi.Events.Arguments.Scp173Events;
using ProjectMER;
using ProjectMER.Features.Objects;
using ProjectMER.Features;
using PlayerStatsSystem;
using ProjectMER.Commands.Modifying.Position;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;

namespace cage173
{
    public class EventHandlers : CustomEventsHandler
    {
        public static Player scp173 = null;
        public static Player mainPlayer = null;
        public static bool inCage = false;
        public static bool took = false;
        public static bool explosion = false;
        public static bool inCorouitne = false;
        public static int amount_of_players = 0;
        public static int amount_of_hits = 0;

        CoroutineHandle handler;
        public static SchematicObject spawnedSchematic;
        public static Vector3 posOf173;
        public static IEnumerator<float> scp173InCage(SchematicObject sch, Player plr)
        {
            inCage = true;
            inCorouitne = true;

            while (inCage)
            {
                if (took)
                {
                    if (scp173?.IsSCP == true)
                    {
                        if (mainPlayer?.IsAlive == true)
                        {
                            if (amount_of_hits >= 30 || explosion)
                            {
                                sch.Destroy();
                                ResetCageState();
                                yield break;
                            }

                            mainPlayer.StaminaRemaining = 0;
                            Vector3 forward = mainPlayer.GameObject.transform.forward;
                            Vector3 behindPos = mainPlayer.Position + forward * 2f;
                            scp173.Position = behindPos;
                            sch.Position = scp173.Position;
                            sch.Position = new Vector3(sch.Position.x, sch.Position.y - 1f, sch.Position.z);
                            sch.Rotation = UnityEngine.Quaternion.LookRotation(forward, Vector3.up);
                        }
                        else
                        {
                            mainPlayer = null;
                            took = false;
                        }
                    }
                    else
                    {
                        sch.Destroy();
                        ResetCageState();
                        scp173 = null;
                        yield break;
                    }

                    yield return Timing.WaitForSeconds(0.1f);
                }
                else
                {
                    scp173.Position = new Vector3(sch.Position.x, sch.Position.y + 1, sch.Position.z);
                    if (scp173 == null || !scp173.IsAlive || !scp173.IsSCP || scp173.Role == RoleTypeId.Spectator)
                    {
                        sch.Destroy();
                        ResetCageState();
                        scp173 = null;
                        yield break;
                    }
                    if (amount_of_hits >= 30 || explosion)
                    {
                        sch.Destroy();
                        ResetCageState();
                        yield break;
                    }

                    yield return Timing.WaitForSeconds(0.1f);
                }
            }
            ResetCageState();
        }
        public static void ResetCageState()
        {
            inCage = false;
            took = false;
            explosion = false;
            amount_of_hits = 0;
            inCorouitne = false;
            mainPlayer = null;
        }

        public override void OnServerRoundStarted()
        {
            Timing.CallDelayed(1f, () =>
            {
                foreach (Player player in Player.List)
                {
                    if (player.Role == RoleTypeId.Scp173 && scp173 == null)
                    {
                        scp173 = player;
                        break;
                    }

                }
            });
        }

        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.Player == mainPlayer)
            {
                mainPlayer = null;
                took = false;
            }
            if (ev.NewRole == RoleTypeId.Scp173 && scp173 == null)
            {
                scp173 = ev.Player;
                ResetCageState();
            }
            else if (ev.OldRole.RoleTypeId == RoleTypeId.Scp173)
            {
                scp173 = null;
                Timing.KillCoroutines(Timing.RunCoroutine(scp173InCage(spawnedSchematic, mainPlayer)));
                spawnedSchematic.Destroy();
                ResetCageState();
            }
        }

        public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
        {
            if(ev.Attacker != null)
            {
                if (ev.Player.Role == RoleTypeId.Scp173)
                {
                    if (inCorouitne)
                    {
                        if (ev.DamageHandler is ExplosionDamageHandler exp)
                        {
                            explosion = true;
                        }
                        else
                        {
                            amount_of_hits++;
                        }
                    }
                    else
                    {
                        explosion = false;
                        amount_of_hits = 0;
                    }
                }
                else if (ev.Attacker.Role == RoleTypeId.Scp173)
                {
                    if (inCage || took)
                    {
                        ev.IsAllowed = false;
                        ev.Attacker.Position = new Vector3(mainPlayer.Position.x - 2f, mainPlayer.Position.y, mainPlayer.Position.z);
                    }
                }
            }
        }
        public void SpawnSchematic(Player player)
        {
            posOf173 = scp173.Position;
            posOf173 = new Vector3(posOf173.x, posOf173.y - 1, posOf173.z);
            spawnedSchematic = ObjectSpawner.SpawnSchematic("173Cage", posOf173, Vector3.zero, Vector3.one);
            Timing.CallDelayed(0.05f, () =>
            {
                foreach (var k in spawnedSchematic.AdminToyBases)
                {
                    if (k != null)
                    {
                        k.IsStatic = false;

                    }
                }

            });

            inCage = true;
            Timing.RunCoroutine(scp173InCage(spawnedSchematic, player));
        }
        public override void OnScp173RemovingObserver(Scp173RemovingObserverEventArgs ev)
        {
            if (ev.Target.IsHuman && !ev.Target.IsSCP)
            {
                amount_of_players--;
            }
        }
        public override void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
        {
            if (ev.Target.IsHuman && !ev.Target.IsSCP)
            {
                amount_of_players++;
            }
        }

        public override void OnPlayerDeath(PlayerDeathEventArgs ev)
        {
            if (ev.Player == mainPlayer)
            {
                mainPlayer = null;
                took = false;
            }

            if (ev.Player.Role == RoleTypeId.Scp173)
            {
                scp173 = null;
                Timing.KillCoroutines(Timing.RunCoroutine(scp173InCage(spawnedSchematic, mainPlayer)));
            }
        }
    }
}
