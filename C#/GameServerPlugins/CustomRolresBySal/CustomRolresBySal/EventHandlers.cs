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
using PlayerRoles.Visibility;
using PlayerStatsSystem;
using RemoteAdmin.Communication;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MapGeneration;
using LiteNetLib;
using RoundRestarting;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace CustomRolesBySal
{
    public class EventHandlers : CustomEventsHandler
    {
        Player CIspy;
        Player GSB1;
        Player MC1;
        Player Cleaner1;
        Player SHC1;

        public override void OnServerRoundStarted()
        {
            Timing.CallDelayed(1.5f, () =>
            {
                int MaxPLayers = Player.List.Count;
                if (MaxPLayers >= 3)
                {
                    GSB();
                }
                if (MaxPLayers >= 4)
                {
                    Timing.CallDelayed(0.5f, () =>
                    {
                        Cleaner();
                    });

                }
                if (MaxPLayers >= 9)
                {
                    MC();
                }
                if (MaxPLayers >= 15)
                {
                    SHC();
                }
                if (MaxPLayers >= 16)
                {
                    Timing.CallDelayed(0.5f, () =>
                    {
                        CISpy();
                    });
                }
            });


        }

        public void GSB()
        {
            Player guard = Player.List.Where(p => p.Role == RoleTypeId.FacilityGuard).OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();
            guard.SendHint("Вы Глава СБ", 20f);
            guard.SetRole(RoleTypeId.FacilityGuard, flags: RoleSpawnFlags.AssignInventory);
            guard.AddItem(ItemType.KeycardMTFOperative);
            guard.RemoveItem(ItemType.KeycardGuard);
            guard.CustomInfo = "Глава СБ";
            GSB1 = guard;
        }

        public void SHC()
        {
            Player scientist = Player.List.Where(p => p.Role == RoleTypeId.Scientist && string.IsNullOrEmpty(p.CustomInfo) && p!=CIspy).OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();
            scientist.SendHint("Вы Старший Научный Сотрудник", 20f);
            scientist.SetRole(RoleTypeId.Scientist, flags: RoleSpawnFlags.AssignInventory);
            scientist.AddItem(ItemType.KeycardResearchCoordinator);
            scientist.AddItem(ItemType.Medkit);
            scientist.RemoveItem(ItemType.KeycardScientist);
            scientist.CustomInfo = "Старший НС";
            SHC1 = scientist;
        }

        public void MC()
        {
            Player manager = Player.List.Where(p => p.Role == RoleTypeId.Scientist).OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();
            manager.SendHint("Вы менеджер комплекса", 20f);
            manager.SetRole(RoleTypeId.Scientist);
            foreach (Room intercom in Room.List)
            {
                if (intercom.Name == RoomName.EzIntercom)
                {
                    manager.Position = intercom.Position + new UnityEngine.Vector3(0f, 1f, 0f);
                }
            }
            manager.ClearInventory();
            manager.AddItem(ItemType.KeycardFacilityManager);
            manager.AddItem(ItemType.Radio);
            manager.AddItem(ItemType.Medkit);
            manager.AddItem(ItemType.GunCOM18);
            manager.AddItem(ItemType.Ammo9x19);
            manager.AddItem(ItemType.Ammo9x19);
            manager.CustomInfo = "Менеджер Комплекса";
            int index = manager.DisplayName.IndexOf("Д-р");
            string result = manager.DisplayName.Substring(index + "Д-р".Length).Trim();
            manager.DisplayName = $"[{manager.PlayerId}] Менеджер Зоны {result}";
            MC1 = manager;
        }

        public void Cleaner()
        {
            Player Dclass = Player.List.Where(p => p.Role == RoleTypeId.ClassD && string.IsNullOrEmpty(p.CustomInfo)).OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();
            Dclass.SetRole(RoleTypeId.ClassD, flags: RoleSpawnFlags.AssignInventory | RoleSpawnFlags.UseSpawnpoint);
            Dclass.SendHint("Вы Уборщик", 20f);
            Dclass.AddItem(ItemType.KeycardJanitor);
            Dclass.CustomInfo = "Уборщик";
            Cleaner1 = Dclass;
        }

        public void CISpy()
        {
            Player spy = Player.List.Where(p => p.Role == RoleTypeId.ClassD && string.IsNullOrEmpty(p.CustomInfo) && p != SHC1).OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();
            spy.SendHint("Вы Шпион Хаоса", 20f);
            spy.SetRole(RoleTypeId.Scientist, flags: RoleSpawnFlags.AssignInventory | RoleSpawnFlags.UseSpawnpoint);
            CIspy = spy;
        }

        public override void OnPlayerDying(PlayerDyingEventArgs ev)
        {
            if (ev.Player == CIspy)
            {
                ev.IsAllowed = false;
                ev.Player.SetRole(RoleTypeId.ChaosRifleman);
                ev.IsAllowed = true;
                ev.Player.CustomInfo = null;
                CIspy = null;
            }
            if (ev.Player == GSB1)
            {
                ev.Player.CustomInfo = null;
            }
            if (ev.Player == Cleaner1)
            {
                ev.Player.CustomInfo = null;
            }
            if (ev.Player == SHC1)
            {
                ev.Player.CustomInfo = null;
            }
            if (ev.Player == MC1)
            {
                ev.Player.CustomInfo = null;
            }
        }


        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.Player == CIspy)
            {
                ev.Player.CustomInfo = null;
                CIspy = null;
            }
            if (ev.Player == GSB1)
            {
                ev.Player.CustomInfo = null;
            }
            if (ev.Player == Cleaner1)
            {
                ev.Player.CustomInfo = null;
            }
            if (ev.Player == SHC1)
            {
                ev.Player.CustomInfo = null;
            }
            if (ev.Player == MC1)
            {
                ev.Player.CustomInfo = null;
            }
        }
    }
}
