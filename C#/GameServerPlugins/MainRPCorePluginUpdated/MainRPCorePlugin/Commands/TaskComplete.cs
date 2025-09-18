using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MainRPCorePlugin;
using MainRPCorePlugin.Features;

namespace MainRPCorePlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CompleteTask : ICommand
    {
        public static List<Player> Cooldown  = new List<Player>();
        public string Command { get; } = "completetask";

        public string[] Aliases { get; } = { "tc", "taskcomplete"};

        public string Description { get; } = "Отметить Задачу Как Выполеную";

        public bool SanitizeResponse { get; }
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player pl = Player.Get(sender);
            if (CustomOtrads.players.ContainsKey(pl))
            {
                if (CustomOtrads.players[pl] is Alpha1)
                {
                    if (arguments.Count >= 1)
                    {
                        if (Cooldown.Contains(pl))
                        {
                            response = "<color=red>Кулдаун Не Прошел!</color>";
                            return false;
                        }
                        int id = int.Parse(arguments.At(0));
                        Plugin.tasksys.Tasks.Remove(id);

                        response = "<color=green>Задание Отмечено Как Выполненное!</color>";
                        Cooldown.Add(pl);
                        Timing.CallDelayed(30f, () =>
                        {
                            Cooldown.Remove(pl);
                        });
                        return true;
                    }
                    
                }
                response = "<color=red>Только Отряд ALPHA-1 Может Использовать Эту Команду.</color>";
                return false;
            }
            
            response = "<color=red>Только Отряд ALPHA-1 Может Использовать Эту Команду.</color>";
            return false;
        }
    }
}