using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using UnityEngine;
using UserSettings.ServerSpecific;
using Utf8Json.Internal.DoubleConversion;
using ICommand = CommandSystem.ICommand;

namespace cage173
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class cageCommand : ICommand
    {
        public string Command => "cage";
        public string[] Aliases => new string[0];
        public string Description => "Клетка для 173";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "Игрок не найден";
                return false;
            }
            if (player.Role != RoleTypeId.NtfCaptain && player.Role != RoleTypeId.NtfSpecialist)
            {
                response = "Эту команду может использовать только Капитан МОГ либо Специалист МОГ";
                return false;
            }

            if (EventHandlers.inCage)
            {
                response = "Клетка уже висит на SCP-173";
                return false;
            }

            Vector3 center = EventHandlers.scp173.Position;
            float dist = Vector3.Distance(center, player.Position);
            if (EventHandlers.amount_of_players < 3)
            {
                response = "Невозможно поставить клетку, если на SCP-173 смотрит меньше 3 человек";
                return false;
            }
            else
            {
                if (dist > 4)
                {
                    response = "Подойдите к SCP-173 поближе и введите команду снова";
                    return false;
                }

                cageFor173.Instance.Events.SpawnSchematic(player);
                response = "Вы успешно ввели команду";
                return true;
            }
        }
    }
}