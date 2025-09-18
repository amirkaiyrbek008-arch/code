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

namespace MainRPCorePlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CreateTask : ICommand
    {
        public static List<Player> Cooldown = new List<Player>();
        public string Command { get; } = "taskcreate";

        public string[] Aliases { get; } = { "ct" };

        public string Description { get; } = "Создать Задание Для Отряда ALPHA-1";

        public bool SanitizeResponse { get; }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player pl = Player.Get(sender);
            if (arguments.Count >= 1)
            {
                if (Cooldown.Contains(pl))
                {
                    response = "<color=red>Кулдаун Не Прошел!</color>";
                    return false;
                }
                int id = 0;
                if (Plugin.tasksys.Tasks.Count >= 1)
                {
                    id = Plugin.tasksys.Tasks.Keys.Max();
                }
                
                Plugin.tasksys.Tasks.Add(id + 1, arguments.At(0));

                response = "<color=green>Успешно Создано Задания Для ALPHA-1.</color>";
                Cooldown.Add(pl);
                Timing.CallDelayed(30f, () => { Cooldown.Remove(pl); });
                return true;
            }

            response = "<color=red>Неправильное Использование Комманды!</color>";
            return false;

        }
    }
}