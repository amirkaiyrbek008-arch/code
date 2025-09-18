using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using System.Linq;
using System.Numerics;
using UserSettings.ServerSpecific;
using ICommand = CommandSystem.ICommand;

namespace DoorNoAccessForSCP
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class wCommand : ICommand
    {
        public string Command => "w";
        public string[] Aliases => new string[0];
        public string Description => "Призыв определенного отряда";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (arguments.Count == 0)
            {
                response = "Вы ввели неправильно команду. Использование: w nu7/e11/e11_106/a1";
                return false;
            }

            string arg = arguments.ElementAt(0).ToLower();

            if (arg == "nu7")
            {
                EventHandlers.argument = 2;
                response = "Вы успешно ввели команду. Введите команду на спавн волны МОГ(wave spawn Ntf), чтобы заспавнился отряд";
                return true;
            }

            else if (arg == "e11")
            {
                EventHandlers.argument = 1;
                response = "Вы успешно ввели команду. Введите команду на спавн волны МОГ(wave spawn Ntf), чтобы заспавнился отряд";
                return true;
            }

            else if (arg == "e11_106")
            {
                EventHandlers.argument = 1;
                EventHandlers.e106 = true;
                response = "Вы успешно ввели команду. Введите команду на спавн волны МОГ(wave spawn Ntf), чтобы заспавнился отряд";
                return true;
            }

            else if(arg == "a1")
            {
                EventHandlers.argument = 3;
                response = "Вы успешно ввели команду. Введите команду на спавн волны МОГ(wave spawn Ntf), чтобы заспавнился отряд";
                return true;
            }

            else
            {
                response = "Вы ввели неправильно команду. Использование: w nu7/e11/e11_106/a1";
                return false;
            }
        }
    }
}