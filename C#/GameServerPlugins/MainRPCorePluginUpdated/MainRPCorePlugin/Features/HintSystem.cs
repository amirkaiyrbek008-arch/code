using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Hints;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using PlayerRoles;
using MainRPCorePlugin.CustomItems;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;
using HintServiceMeow.Core.Utilities;
using HintServiceMeow.Core.Models.Hints;
namespace MainRPCorePlugin.Features
{
    public class HintSystem
    {
        public void Init()
        {
            Plugin.Coroutines.Add(Timing.RunCoroutine(CustomCourotineGui()));
            
        }
        public IEnumerator<float> CustomCourotineGui()
        {
            while (true)
            {
                foreach (Player player in Player.List)
                {
                    
                    PlayerDisplay playerDisplay = PlayerDisplay.Get(player);
                    if (player.Role.IsDead)
                    {
                        if (playerDisplay.GetHint("display") != null)
                        {
                            playerDisplay.RemoveHint(playerDisplay.GetHint("display"));
                        }
                        continue;

                    }
                    string text = "";
                    if (!player.IsScp)
                    {
                        text += $"<color={player.Role.Color.ToHex()}>🙎Ваше Имя:</color> {player.DisplayNickname}";
                    }
                    else
                    {
                        text += $"<color={player.Role.Color.ToHex()}>🙎Имя Объекта:</color> {player.DisplayNickname}";
                    }

                    try
                    {
                        
                        if (player.Role.Team == Team.FoundationForces)
                        {
                            if (CustomOtrads.players.ContainsKey(player) && CustomOtrads.players[player] is Alpha1)
                            {
                                string tasks = "";

                                for(int i = 1; i<= Plugin.tasksys.Tasks.Count; i++)
                                {
                                    tasks += $"\n[{i}] {Plugin.tasksys.Tasks[i]}";

                                }
                                text += $"\n<color={player.Role.Color.ToHex()}>\ud83d\udccbВаши Задания:</color>{tasks}";
                            }
                        }
                        
                        text += $"\n<color={player.Role.Color.ToHex()}>⏰Время Раунда:</color> {Round.ElapsedTime.Minutes} Мин {Round.ElapsedTime.Seconds%60} Сек";

                        Hint hint = new Hint
                        {
                            Text = text,
                            Id = "display"
                        };
                        hint.YCoordinate = 1035f;
                        hint.XCoordinate = 60f;
                        hint.Alignment = 0;

                        if (playerDisplay.GetHint("display") == null)
                        {
                            playerDisplay.AddHint(hint);
                        }
                        else
                        {
                            AbstractHint hintupd = playerDisplay.GetHint("display");
                            hintupd.Text = hint.Text;
                            hintupd = null;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Info(e);
                    }

                }
                yield return Timing.WaitForSeconds(0.5f);
            }

        }
    }
}
