using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainRPCorePlugin.CustomItems;
using Respawning;
using Respawning.NamingRules;
using Utils.Networking;

namespace MainRPCorePlugin.Features
{
    public abstract class Otrad
    {
        public Player player;
        public abstract List<Player> GetTeamates();
        public abstract string GetName();
    }
    public class Etta10: Otrad
    {
        public override List<Player> GetTeamates()
        {
            return CustomOtrads.players.Keys.Where(x => CustomOtrads.players[x] is Etta10 && x!=this.player).ToList();
        }
        public override string GetName()
        {
            return "Эта-10 Не Вижу Зла";
        }
    }
    public class Alpha1: Otrad
    {
        public override List<Player> GetTeamates()
        {
            return CustomOtrads.players.Keys.Where(x => CustomOtrads.players[x] is Alpha1 && x!=this.player).ToList();
        }
        public override string GetName()
        {
            return "Альфа-1 Багрянная Десница";
        }
    }
    public class CustomOtrads
    {
        public static Dictionary<Player, Otrad> players = new Dictionary<Player, Otrad>();

        public void Init()
        {
            Exiled.Events.Handlers.Server.RespawningTeam += OnTeamRespawn;
            Exiled.Events.Handlers.Player.ChangingRole += OnRoleChange;
        }
        public void OnTeamRespawn(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == PlayerRoles.Faction.FoundationStaff)
            {
                SpawnAlpha1(ev.Players);
                ev.IsAllowed = false;
            }
        }
        public void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (players.ContainsKey(ev.Player))
            {
                players.Remove(ev.Player);
            }
        }
        public void SpawnEta10(List<Player> playerstospawn)
        {
            bool spawnedkaptain = false;
            foreach (Player player in playerstospawn)
            {
                if (!spawnedkaptain)
                {
                    player.Role.Set(PlayerRoles.RoleTypeId.NtfCaptain);
                    spawnedkaptain = true;
                    players.Add(player, new Etta10());
                    Shifrator shifrator = Shifrator.Create();
                    player.AddItem(shifrator.item);
                    shifrator.owner = player;
                    continue;

                }
                player.Role.Set(PlayerRoles.RoleTypeId.NtfSergeant);
                players.Add(player, new Etta10());
            }
        }
        public void SpawnAlpha1(List<Player> playerstospawn)
        {
            bool spawnedkaptain = false;
            foreach (Player player in playerstospawn)
            {
                if (!spawnedkaptain)
                {
                    player.Role.Set(PlayerRoles.RoleTypeId.NtfCaptain);
                    spawnedkaptain = true;
                    //HackToolItem hacktool = HackToolItem.Create();
                    //player.AddItem(hacktool.item);
                    //hacktool.owner = player;
                    players.Add(player, new Alpha1());
                    continue;

                }//
                player.Role.Set(PlayerRoles.RoleTypeId.NtfSergeant);
                players.Add(player, new Alpha1());
            }
        }
    }
    public static class OtradExtensions
    {
        public static string GetPlayerOtradName(this Player player)
        {
            if (CustomOtrads.players.ContainsKey(player))
            {
                return CustomOtrads.players[player].GetName();
            }
            else
            {
                return "NULL";
            }
            
        }
    }
}
