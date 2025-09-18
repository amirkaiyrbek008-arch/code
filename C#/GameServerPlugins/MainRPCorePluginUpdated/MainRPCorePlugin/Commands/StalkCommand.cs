using CommandSystem;
using MEC;
using System;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRPCorePlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class StalkCommand : ICommand
    {
        public static List<Player> Cooldown = new List<Player>();
        public string Command { get; } = "stalk";

        public string[] Aliases { get; } = { "sta" };

        public string Description { get; } = "Сталкнуться К Игроку!";

        public bool SanitizeResponse { get; }
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player pl = Player.Get(sender);
            if (pl.Role.Type == PlayerRoles.RoleTypeId.Scp106)
            {
                if (Cooldown.Contains(pl))
                {
                    response = "<color=red>Кулдаун Не Прошел!</color>";
                    return false;
                }
                pl.Role.As<Scp106Role>().UsePortal(Player.List.Where(x=>!x.IsScp && !x.IsDead).ToList().RandomItem().Position);
                response = "<color=green>Успешно сталкнулся.</color>";
                Cooldown.Add(pl);
                Timing.CallDelayed(30f, () =>
                {
                    Cooldown.Remove(pl);
                });
                return true;
            }
            response = "<color=red>Только SCP-106 Можент Использовать Данную Комманду!</color>";
            return false;
        }
    }
}
