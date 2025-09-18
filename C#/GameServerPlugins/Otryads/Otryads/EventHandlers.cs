using LabApi.Events.CustomHandlers;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;
using System.Numerics;

namespace DoorNoAccessForSCP
{
    public class EventHandlers : CustomEventsHandler
    {
        public static int argument = 0;
        public static bool e106 = false;
        private static readonly Random Rand = new Random();
        private string cassieLine;
        private string otryadName;
        private readonly List<Player> pendingPlayers = new List<Player>();
        bool proccesed = false;

        public void Check()
        {
            if (proccesed)
            {
                proccesed = false;
                Timing.CallDelayed(0.5f, () =>
                {
                    spawnOtryad();
                });
            }


        }

        public override void OnServerRoundStarted()
        {
            Timing.RunCoroutine(enumerator());
        }
        IEnumerator<float> enumerator()
        {
            while (true)
            {
                if (proccesed)
                {
                    proccesed = false;
                    spawnOtryad();
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
        public override void OnPlayerSpawned(PlayerSpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.NtfCaptain || ev.Player.Role == RoleTypeId.NtfSergeant || ev.Player.Role == RoleTypeId.NtfPrivate)
            {
                pendingPlayers.Add(ev.Player);
                proccesed = true;
            }
        }

        public void spawnOtryad()
        {
            if (argument == 1)
            {
                otryadName = "E-11";
                List<Player> players = pendingPlayers.Where(p => p.Role != RoleTypeId.NtfCaptain).ToList();

                int amountPlayers = players.Count;

                int spec = (int)Math.Round(amountPlayers * 0.1, MidpointRounding.AwayFromZero);
                int sergeant = (int)Math.Round(amountPlayers * 0.5, MidpointRounding.AwayFromZero);
                int privates = amountPlayers - spec - sergeant;

                players = players.OrderBy(x => Rand.Next()).ToList();

                for (int i = 0; i < players.Count; i++)
                {
                    if (i < spec)
                        players[i].Role = RoleTypeId.NtfSpecialist;
                    else if (i < spec + sergeant)
                        players[i].Role = RoleTypeId.NtfSergeant;
                    else
                        players[i].Role = RoleTypeId.NtfPrivate;
                    players[i].CustomInfo = otryadName;
                }
                foreach (Player plr in pendingPlayers)
                {
                    plr.CustomInfo = otryadName;
                }

                if (e106 == true)
                {
                    foreach (Player plr in pendingPlayers)
                    {
                        bool flash = false;
                        foreach (Item item in plr.Items)
                        {
                            if (item.Type == ItemType.GrenadeFlash)
                            {
                                flash = true;
                            }
                        }
                        if (!flash)
                        {
                            plr.AddItem(ItemType.GrenadeFlash);
                        }
                    }
                    List<Player> privateList = players.Where(p => p.Role == RoleTypeId.NtfPrivate).ToList();

                    if (privateList.Count > 0)
                    {
                        Player dclass = privateList[Rand.Next(privateList.Count)];
                        dclass.SetRole(RoleTypeId.ClassD, flags: RoleSpawnFlags.AssignInventory);
                        dclass.DisplayName = "106-B";
                        dclass.CustomInfo = null;
                    }

                    else
                    {

                        List<Player> sergeantList = players.Where(p => p.Role == RoleTypeId.NtfSergeant).ToList();

                        if (sergeantList.Count > 0)
                        {
                            Player dclass = sergeantList[Rand.Next(sergeantList.Count)];
                            dclass.SetRole(RoleTypeId.ClassD, flags: RoleSpawnFlags.AssignInventory);
                            dclass.DisplayName = "106-B";
                        }
                    }

                    e106 = false;
                }

                cassieLine =
                    "pitch_1 Bell_Start .g6<size=0> identification process pitch_2 .g1 .g1 .g1 .g1 " +
                    ".g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 . . pitch_0.8 successfully " +
                    ".<split><b><size=21>Процесс авторизации…</b><size=0>.........................................................................................................................................................." +
                    "</size><b>【✅・<color=#00ff49>Успешно</color>】<split><size=21><b><color=#ff1500>ВНИМАНИЕ</color> | Всему персоналу комплекса: <color=#00008b>Мобильная Оперативная Группа Эпсилон-11</color>, " +
                    "под кодовым именем <color=#00008B>\"Девятихвостая Лиса\"</color>, вошла в комплекс... Всем оставшимся в живых рекомендуется оставаться в <#14b542>эвакуационном убежище</color> или любом другом " +
                    "<#14b542>безопасном месте</color>. Пока подразделение не возьмет <color=#8a8a8a>объект</color> под свой контроль. Эвакуация начнется после повторного сдерживания <color=#ff1500>SCP-объектов</color>  " +
                    "<size=0> bell_start . pitch_0.90 attention to all personnel . mtfunit Epsilon 11 designated NINETAILEDFOX hasentered . Any survivors are advised to remain in an evacuation shelter or other . . " +
                    "Until the unit takes control of the facility . Evacuation will start for repeated containment of SCPSUBJECT  bell_end";

                Cassie.Message(cassieLine);
                proccesed = false;
                pendingPlayers.Clear();
                argument = 0;
                otryadName = null;

            }
            else if (argument == 2)
            {
                otryadName = "NU-7";
                foreach (Player plr in pendingPlayers)
                {
                    if (plr.Role == RoleTypeId.NtfCaptain)
                    {
                        plr.MaxArtificialHealth = 150;
                        plr.MaxHealth = 150;
                        plr.Health = 150;
                    }
                    else if (plr.Role == RoleTypeId.NtfSergeant)
                    {
                        plr.RemoveItem(ItemType.ArmorCombat);
                        plr.AddItem(ItemType.ArmorHeavy);
                        plr.AddItem(ItemType.Adrenaline);
                    }
                    plr.CustomInfo = otryadName;
                }

                cassieLine =
                    "pitch_1 Bell_Start .g6<size=0> identification process pitch_2 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 .g1 . . pitch_0.8 successfully " +
                    ".<split><b><size=21>Процесс авторизации…</b><size=0>...........................................................................................................................................................</size>" +
                    "<b>【✅・<color=#00ff49>Успешно</color>】<split><size=21><b><color=#ff1500>ВНИМАНИЕ</color> | Всему персоналу комплекса: <color=#00008b>Мобильная Оперативная Группа Ню-7</color>, " +
                    "под кодовым именем <color=#00008B>\"Удар Молота\"</color>, вошла в комплекс... для нейтрализации <color=#14b542>неавторизованного персонала</color>… Всем оставшимся в живых рекомендуется оставаться " +
                    "в <color=#14b542>эвакуационном убежище</color> или любом другом <color=#14b542>безопасном месте</color>, пока подразделение не возьмет объект под свой контроль. <size=0> pitch_1.5 bell_start pitch_1 " +
                    "attention to all personnel . mtfunit Nu 7 hasentered for Neutralize unauthorized personnel . . pitch_0.9 Any survivors are advised to a safe shelter or other. Until the unit will neutralize unauthorized personnel . g1 .g1";

                Cassie.Message(cassieLine);
                proccesed = false;
                pendingPlayers.Clear();
                argument = 0;
                otryadName = null;
            }

            else if (argument == 3)
            {
                otryadName = "A-1";
                foreach (Player plr in pendingPlayers)
                {
                    plr.SetRole(RoleTypeId.NtfCaptain, flags: RoleSpawnFlags.All);
                    plr.CustomInfo = otryadName;
                }

                cassieLine = "<split><size=21><b><color=#ff1500>ВНИМАНИЕ</color> | Всему персоналу комплекса: <color=#990026>Мобильная Оперативная Группа Альфа-1</color>, под кодовым именем <color=#990026>\"Багряная десница\"</color>, вошла на территорию комплекса...<size=0> bell_start . pitch_0.90 attention to all facility personnel . mtfunit Alpha 1 designated Red Right Hand hasentered . bell_end";
                proccesed = false;
                pendingPlayers.Clear();
                argument = 0;
                otryadName = null;
            }


            else if (argument == 4)
            {
                foreach (Player plr in pendingPlayers)
                {
                    continue;
                }
            }
        }

        public override void OnPlayerDeath(PlayerDeathEventArgs ev)
        {
            if (ev.Player.CustomInfo == "E-11" || ev.Player.CustomInfo == "NU-7" || ev.Player.CustomInfo == "A-1")
            {
                ev.Player.CustomInfo = null;
            }
        }
        public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
        {
            if (ev.Player.CustomInfo == "E-11" || ev.Player.CustomInfo == "NU-7" || ev.Player.CustomInfo == "A-1")
            {
                ev.Player.CustomInfo = null;
            }
        }
    }
}