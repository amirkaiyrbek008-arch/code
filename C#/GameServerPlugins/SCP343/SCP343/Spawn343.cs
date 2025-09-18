using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;

namespace SCP343
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn343 : ICommand
    {
        public string Command { get; set; } = "spawn343";

        public string[] Aliases { get; set; } = new string[] { "s343", "343" };

        public string Description { get; set; } = "Спавнит вас или друго-го игрока за SCP-343.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender snd, out string response)
        {
            Player sender = Player.Get(snd);
            if (!sender.HasPermissions("scp343.spawn"))
            {
                response = "У вас нету прав на использование этой комманды.";
                return false;
            }
            if (arguments.Count == 0)
            {
                SCP343.Instance.Events.Spawn343(sender);
                response = "Вы успешно заспавнили себя за 343!";
                return true;
            }
            if (arguments.Count > 0)
            {
                int id;
                if (int.TryParse(arguments.ElementAt(0), out id))
                {
                    Player target = Player.Get(id);
                    SCP343.Instance.Events.Spawn343(target);
                    response = $"Вы успешно заспавнили {target.Nickname} за SCP-343!";
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
