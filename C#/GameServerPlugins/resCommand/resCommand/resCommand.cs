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
        public string Command => "res";
        public string[] Aliases => new string[0];
        public string Description => "Зареспавниться за Д-Класс до прибытия мог";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "Игрок не найден";
                return false;
            }
            if (player.Role != RoleTypeId.Spectator)
            {
                response = "Эту команду могут использовать только наблюдатели";
                return false;
            }

            if (EventHandlers.hasSpawned)
            {
                response = "Вы не можете больше использовать эту команду, ведь уже прошло 3 минуты с начала раунда";
                return false;
            }

            EventHandlers.spawnDClass(player);
            response = "Вы успешно ввели команду";
            return true;
        }
    }
}