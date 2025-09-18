using GameCore;
using LabApi.Events.Arguments.PlayerEvents;
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCP343
{
    public class EventHandlers : CustomEventsHandler
    {
        float radius = 3f;
        int cd = 0;
        string godName = string.Empty;


        IEnumerator<float> CoolDown()
        {
            while (cd > 0)
            {
                cd--;
                yield return Timing.WaitForSeconds(1);
            }

            Player godPlayer = Player.List.FirstOrDefault(p => p.Nickname == godName);
            if (godPlayer != null)
            {
                godPlayer.SendHint("Вы снова можете возрождать игроков", 10f);
            }
        }

        IEnumerator<float> Teleport(Player plr)
        {
            var targets = Player.List.Where(x => x.IsAlive && x != plr).ToList();
            if (targets.Count > 0)
            {
                plr.Position = targets.RandomItem().Position;
            }
            yield return Timing.WaitForSeconds(3);
        }

        public override void OnPlayerShootingWeapon(PlayerShootingWeaponEventArgs ev)
        {
            if (ev.Player.Nickname == godName)
            {
                if (ev.FirearmItem.Type == ItemType.MicroHID || ev.FirearmItem.Type == ItemType.ParticleDisruptor || ev.FirearmItem.Type == ItemType.Jailbird)
                {
                    ev.IsAllowed = false;
                }

                ev.IsAllowed = false;

            }


            else { return; }
        }

        public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
        {
            if (ev.Attacker == null)
                return;

            if (ev.Attacker.Nickname == godName)
            {
                ev.IsAllowed = false;
            }
            else { return; }
        }

        public override void OnServerRoundStarted()
        {
            Timing.CallDelayed(0.5f, () =>
            {
                Player target = Player.List.FirstOrDefault(p => p.Role == RoleTypeId.ClassD);
                if (target != null)
                {
                    if (Player.List.Count > Config.RequiredPlayers && godName == string.Empty)
                    {
                        Spawn343(target);
                    }
                }
            });
 
        }
  
        public void Spawn343(Player player)
        {
            player.SetRole(RoleTypeId.Tutorial, flags: RoleSpawnFlags.AssignInventory);
            player.DisplayName = $"[SCP-343] {player.Nickname}";
            player.AddItem(ItemType.Coin);
            player.AddItem(ItemType.SCP500);
            player.SendBroadcast("Вы - <color=green>SCP-343</color>", 15); 
            player.IsGodModeEnabled = true;
            player.IsBypassEnabled = true;
            if(godName == string.Empty) 
            {
                godName = player.Nickname;
            } 

        }

        public override void OnPlayerDeath(PlayerDeathEventArgs ev)
        {
            if (ev.Player.Nickname == godName)
            {
                ev.Player.DisplayName = null;
                ev.Player.IsGodModeEnabled = false;
                ev.Player.IsBypassEnabled = false;
                godName = string.Empty;
 
            }

            else { return; }
        }

        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.Player.Nickname == godName)
            {
                ev.Player.DisplayName = null;
                ev.Player.IsGodModeEnabled = false;
                ev.Player.IsBypassEnabled = false;
                godName = string.Empty;
             

            }

            else { return; }
        }

        public override void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.SCP500 && ev.Player.Nickname == godName)
            {
 
                ev.IsAllowed = false;
                if (cd > 0)
                {
                    ev.Player.SendHint($"Перезарядка способности: {cd} секунд");
                    return;
                }

                foreach (Ragdoll ragdoll in Ragdoll.List)
                {
                    Player name = Player.List.FirstOrDefault(p => p.Nickname == ragdoll.Nickname);

                    if (!ragdoll.Role.IsScp() && ragdoll.Role.IsHuman() && !name.IsAlive)
                    {

                        float dist = Vector3.Distance(ev.Player.Position, ragdoll.Position);
                        if (dist <= radius)
                        {
                            Player plr = Player.List.FirstOrDefault(p => p.Nickname == ragdoll.Nickname);
                             if (plr != null)
                            {
                                PlayerRevive(ev.Player, plr, ragdoll.Role, ragdoll.Position);
                                break;
                            }
                        }
                    }
                }
            }

            else if (ev.Item.Type == ItemType.Coin && ev.Player.Nickname == godName) 
            {
                Timing.RunCoroutine(Teleport(ev.Player));
                ev.IsAllowed = false;
            }

            else { return; }

        }

        public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
        {
           if (ev.Player.Nickname == godName)
            {
                if (ev.UsableItem.Type == ItemType.SCP500 )
                {
                    ev.IsAllowed = false;
                }
 
            }

           else { return; }
        }

        public override void OnPlayerCuffing(PlayerCuffingEventArgs ev)
        {
            if (ev.Player.Nickname == godName || ev.Target.Nickname == godName)
            {
                ev.IsAllowed = false;
            }

            else { return; }
        }

        public override void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev)
        {
            if ( ev.Player.Nickname == godName)
            { 
                if (ev.Door.DoorName == DoorName.Hcz079FirstGate || ev.Door.DoorName == DoorName.Hcz079SecondGate || ev.Door.DoorName == DoorName.Hcz079Armory || ev.Door.IsLocked) 
                {
                    ev.IsAllowed = false;
                }
            }

            else { return; }
        }

        public override void OnPlayerInteractingLocker(PlayerInteractingLockerEventArgs ev)
        {
 

            if (ev.Player.Nickname == godName &&  ev.Locker is RifleRackLocker )
            {
                ev.IsAllowed = false;
            }

            else { return; }
        }

        public override void OnScp914ProcessingInventoryItem(Scp914ProcessingInventoryItemEventArgs ev)
        {
            if(ev.Player.Nickname == godName) 
            {
                if(ev.Item.Type ==  ItemType.Coin || ev.Item.Type == ItemType.SCP500)
                { 
                    ev.IsAllowed = false; 
                } 
            }

            else { return; }
        }

        public override void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
        {
            if(ev.Target.Nickname == godName)
            {
                ev.IsAllowed = false;  
            }

            else { return; }
        }

        public override void OnScp096AddingTarget(Scp096AddingTargetEventArgs ev)
        {
            if(ev.Target.Nickname == godName)
            {
                ev.IsAllowed = false;
            }

            else { return; }
        }

        public override void OnPlayerInteractingScp330(PlayerInteractingScp330EventArgs ev)
        {
            if(ev.Player.Nickname == godName)
            {
                ev.IsAllowed = false;
            }

            else { return; }
        }

        public void PlayerRevive(Player god, Player target, RoleTypeId role, Vector3 pos)
        {
            cd = 50;
            Timing.RunCoroutine(CoolDown());
            target.SetRole(role);
            target.ClearInventory();
            target.Position = god.Position;
            god.SendHint($"Вы успешно возродили игрока {target.Nickname}", 10f);
        }

        public override void OnPlayerFlippingCoin(PlayerFlippingCoinEventArgs ev)
        {
            if(ev.Player.Nickname == godName)
            {
                Timing.RunCoroutine(Teleport(ev.Player));
 
             }
            
            else { return; }
        }

        public override void OnPlayerThrowingProjectile(PlayerThrowingProjectileEventArgs ev)
        {
            if (ev.Player.Nickname == godName)
            {
                if(ev.ThrowableItem.Type == ItemType.GrenadeFlash || ev.ThrowableItem.Type == ItemType.GrenadeHE)
                {
                    ev.IsAllowed = false;
                }
            }

            else { return;  }

        }

 
    }
} 