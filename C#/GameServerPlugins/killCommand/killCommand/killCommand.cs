using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerStatsSystem;
using SCPScape;
using System;
using UserSettings.ServerSpecific;
using ICommand = CommandSystem.ICommand;

namespace cageFor173
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class cageCommand : ICommand
    {
        public string Command => "kill";
        public string[] Aliases => new string[0];
        public string Description => "Убивает вас";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "Игрок не найден";
                return false;
            }
            if (!player.Role.IsAlive())
            {
                response = "Эту команду могут использовать только живые игроки";
                return false;
            }

            EventHandlers.kill(player);
            response = "Вы успешно ввели команду";
            return true;
        }
    }
}