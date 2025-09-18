using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features.Roles;
using UnityEngine;
using Exiled.API.Features;
namespace MainRPCorePlugin.Features
{
    public class CustomNicknames
    {
        public Config config = Plugin.Instance.Config;
        public void Init()
        {
            Log.Info("Inniting Custom Nicknames Module!");
            Exiled.Events.Handlers.Player.Spawned += ChangingNickName;
        }
        public void DeInit()
        {
            Log.Info("DeInniting Custom Nicknames Module!");
            Exiled.Events.Handlers.Player.Spawned -= ChangingNickName;
        }
        public void ChangingNickName(SpawnedEventArgs ev)
        {
            ev.Player.DisplayNickname = $"[{ev.Player.Id}] "+ev.Player.GetPlayersRoleName();
        }
    }
    public static class CustomNicknamesExt
    {
        public static string GetPlayersRoleName(this Player player)
        {

            Role role = player.Role;
            if (role.Team == Team.SCPs)
            {
                return "SCP ■■■";
            }
            else
            {
                switch (role.Type)
                {
                    case RoleTypeId.ClassD:
                        int num = UnityEngine.Random.Range(0001, 9999);
                        return "D-" + num;
                    case RoleTypeId.Scientist:
                        return "Д-р " + Plugin.Instance.Config.DoctorName.RandomItem();
                    case RoleTypeId.FacilityGuard:
                        return "" + Plugin.Instance.Config.PozivNieSB.RandomItem();
                    case RoleTypeId.NtfCaptain:
                        return "" + Plugin.Instance.Config.PozivNieMTF.RandomItem();
                    case RoleTypeId.NtfSergeant:
                        return "" + Plugin.Instance.Config.PozivNieMTF.RandomItem();
                    case RoleTypeId.NtfPrivate:
                        return "" + Plugin.Instance.Config.PozivNieMTF.RandomItem();
                    case RoleTypeId.NtfSpecialist:
                        return "" + Plugin.Instance.Config.PozivNieMTF.RandomItem();
                    case RoleTypeId.ChaosConscript:
                        return "" + Plugin.Instance.Config.PozivNieHAOS.RandomItem();
                    case RoleTypeId.ChaosMarauder:
                        return "" + Plugin.Instance.Config.PozivNieHAOS.RandomItem();
                    case RoleTypeId.ChaosRifleman:
                        return "" + Plugin.Instance.Config.PozivNieHAOS.RandomItem();
                    case RoleTypeId.ChaosRepressor:
                        return "" + Plugin.Instance.Config.PozivNieHAOS.RandomItem();
                    default:
                        return player.Nickname;
                }
            }
        }
    }
}
