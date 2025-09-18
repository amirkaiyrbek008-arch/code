using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;

namespace SCP966
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn966 : CommandSystem.ICommand
    {
        public string Command { get; set; } = "spawn of 966";

        public string[] Aliases { get; set; } = new string[] { "s966", "966" };

        public string Description { get; set; } = "Спавнит вас или друго-го игрока за SCP-966.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender snd, out string response)
        {
            Player sender = Player.Get(snd);
            if (!sender.HasPermissions("scp966.spawn"))
            {
                response = "У вас нету прав на использование этой комманды.";
                return false;
            }
            if (arguments.Count == 0)
            {
                SCP966.Instance.Events.Spawn966(sender);
                response = "Вы успешно заспавнили себя за 343!";
                return true;
            }
            if (arguments.Count > 0)
            {
                int id;
                if (int.TryParse(arguments.ElementAt(0), out id))
                {
                    Player target = Player.Get(id);
                    SCP966.Instance.Events.Spawn966(target);
                    response = $"Вы успешно заспавнили {target.Nickname} за SCP-966!";
                    return true;
                }
                else
                {
                    response = "Произошла ошибка! Вы уверены, что указали аргументы комманды верно? Использование комманды: spawn343 id";
                    return false;
                }
            }
            response = "Произошла ошибка! Вы уверены, что указали аргументы комманды верно? Использование комманды: spawn343 id";
            return false;

        }


    }

}
