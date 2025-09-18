using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabApi.Events.CustomHandlers;
using HintServiceMeow;
using GameCore;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;
using HintServiceMeow.Core.Utilities;
using HintServiceMeow.Core.Models.Hints;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using MEC;
using PlayerRoles;
using System.Data;
using HintServiceMeow.Core.Interface;
using System.Numerics;
using Hints;

namespace HintService
{
    public class EventHandlers : CustomEventsHandler
    {
        IEnumerator<float> enumerator()
        {
            while (true)
            {
                foreach (Player plr in Player.List)
                {
                    PlayerDisplay display = PlayerDisplay.Get(plr);
                    if (!plr.IsAlive)
                    {
                        string text1 = $"\n<color={plr.RoleBase.RoleColor.ToHex()}>⏰Время Раунда:</color> {Round.Duration.Minutes} Минут {Round.Duration.Seconds % 60} Секунд";


                        Hint hint1 = new Hint
                        {
                            Text = text1,
                            Id = "display"
                        };
                        hint1.YCoordinate = 1035f;
                        hint1.XCoordinate = 60f;
                        hint1.Alignment = 0;

                        if (display.GetHint("display") == null)
                        {
                            display.AddHint(hint1);
                        }
                        else
                        {
                            AbstractHint hintupd = display.GetHint("display");
                            hintupd.Text = hint1.Text;
                            hintupd = null;
                        }

                        continue;
                    }
                    string text = "";
                    if (plr.IsAlive)
                    {
                        if (plr.IsSCP)
                        {
                            text += $"<color={plr.RoleBase.RoleColor.ToHex()}>🙎Имя Объекта:</color> {plr.DisplayName}";
                        }
                        else
                        {
                            text += $"<color={plr.RoleBase.RoleColor.ToHex()}>🙎Ваше Имя:</color> {plr.DisplayName}";
                        }

                        if (plr.IsNTF)
                        {
                            if(plr.CustomInfo != null)
                            {
                                text += $"<color={plr.RoleBase.RoleColor.ToHex()}>👮‍♂️Название Отряда:</color> {plr.CustomInfo}";
                            }
                            string rolee = "";
                            if (plr.Role == RoleTypeId.NtfCaptain)
                            {
                                rolee = "Капитан";
                            }
                            else if (plr.Role == RoleTypeId.NtfPrivate)
                            {
                                rolee = "Рядовой";
                            }
                            else if (plr.Role == RoleTypeId.NtfSergeant)
                            {
                                rolee = "Сержант";
                            }
                            else if (plr.Role == RoleTypeId.NtfSpecialist)
                            {
                                rolee = "Специалист";
                            }
                            text += $"<color={plr.RoleBase.RoleColor.ToHex()}>👮‍♂️Роль:</color> {rolee}";
                        }

                        else if (plr.IsChaos)
                        {
                            string rolee = "";
                            if (plr.Role == RoleTypeId.ChaosConscript)
                            {
                                rolee = "Новобранец";
                            }
                            else if (plr.Role == RoleTypeId.ChaosRepressor)
                            {
                                rolee = "Усмиритель";
                            }
                            else if (plr.Role == RoleTypeId.ChaosRifleman)
                            {
                                rolee = "Стрелок";
                            }
                            else if (plr.Role == RoleTypeId.ChaosMarauder)
                            {
                                rolee = "Мародер";
                            }
                            text += $"<color={plr.RoleBase.RoleColor.ToHex()}>🥷Роль:</color> {rolee}";
                        }
                        else
                        {
                            if (plr.CustomInfo != null)
                            {
                                text += $"<color={plr.RoleBase.RoleColor.ToHex()}>📋Роль:</color> {plr.CustomInfo}";
                            }
                        }
                    }
                    text += $"\n<color={plr.RoleBase.RoleColor.ToHex()}>⏰Время Раунда:</color> {Round.Duration.Minutes} Минут {Round.Duration.Seconds % 60} Секунд";

                    Hint hint = new Hint
                    {
                        Text = text,
                        Id = "display"
                    };
                    hint.YCoordinate = 1035f;
                    hint.XCoordinate = 60f;
                    hint.Alignment = 0;

                    if (display.GetHint("display") == null)
                    {
                        display.AddHint(hint);
                        Cassie.Message("tess1");
                    }
                    else
                    {
                        AbstractHint hintupd = display.GetHint("display");
                        hintupd.Text = hint.Text;
                        hintupd = null;
                        Cassie.Message("tess2");

                    }
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public override void OnServerRoundStarted()
        {
            Timing.RunCoroutine(enumerator());
        }
        public override void OnServerRoundRestarted()
        {
            Timing.KillCoroutines();

            foreach (Player plr in Player.List)
            {
                PlayerDisplay display = PlayerDisplay.Get(plr);
                if (display.GetHint("display") != null)
                    display.RemoveHint(display.GetHint("display"));
            }
        }
    }
}




//using HarmonyLib;
//using Hints;
//using Exiled.API.Features;
//using MEC;


//namespace MainRPCorePlugin.Features
//{
//    public class HintSystem
//    {
//        public void Init()
//        {
//            Plugin.Coroutines.Add(Timing.RunCoroutine(CustomCourotineGui()));

//        }
//        public IEnumerator<float> CustomCourotineGui()
//        {
//            while (true)
//            {
//                foreach (Player player in Player.List)
//                {

//                    PlayerDisplay playerDisplay = PlayerDisplay.Get(player);
//                    if (player.Role.IsDead)
//                    {
//                        if (playerDisplay.GetHint("display") != null)
//                        {
//                            playerDisplay.RemoveHint(playerDisplay.GetHint("display"));
//                        }
//                        continue;

//                    }
//                    string text = "";
//                    if (!player.IsScp)
//                    {
//                        text += $"<color={player.Role.Color.ToHex()}>🙎Ваше Имя:</color> {player.DisplayNickname}";
//                    }
//                    else
//                    {
//                        text += $"<color={player.Role.Color.ToHex()}>🙎Имя Объекта:</color> {player.DisplayNickname}";
//                    }

//                    try
//                    {

//                        if (player.Role.Team == Team.FoundationForces)
//                        {
//                            if (CustomOtrads.players.ContainsKey(player) && CustomOtrads.players[player] is Alpha1)
//                            {
//                                string tasks = "";

//                                for (int i = 1; i <= Plugin.tasksys.Tasks.Count; i++)
//                                {
//                                    tasks += $"\n[{i}] {Plugin.tasksys.Tasks[i]}";

//                                }
//                                text += $"\n<color={player.Role.Color.ToHex()}>\ud83d\udccbВаши Задания:</color>{tasks}";
//                            }
//                        }

//                        text += $"\n<color={player.Role.Color.ToHex()}>⏰Время Раунда:</color> {Round.ElapsedTime.Minutes} Мин {Round.ElapsedTime.Seconds % 60} Сек";

//                        Hint hint = new Hint
//                        {
//                            Text = text,
//                            Id = "display"
//                        };
//                        hint.YCoordinate = 1035f;
//                        hint.XCoordinate = 60f;
//                        hint.Alignment = 0;

//                        if (playerDisplay.GetHint("display") == null)
//                        {
//                            playerDisplay.AddHint(hint);
//                        }
//                        else
//                        {
//                            AbstractHint hintupd = playerDisplay.GetHint("display");
//                            hintupd.Text = hint.Text;
//                            hintupd = null;
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        Log.Info(e);
//                    }

//                }
//                yield return Timing.WaitForSeconds(0.5f);
//            }

//        }
//    }
//}
