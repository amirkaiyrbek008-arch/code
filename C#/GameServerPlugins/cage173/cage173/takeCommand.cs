using cage173;
using CommandSystem;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using System;
using UnityEngine;
using UserSettings.ServerSpecific;
using ICommand = CommandSystem.ICommand;

namespace cage173
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class takeCommand : ICommand
    {
        public string Command => "take";
        public string[] Aliases => new string[0];
        public string Description => "Взять клетку с 173";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "Игрок не найден";
                return false;
            }

            if (!EventHandlers.inCage)
            {
                response = "SCP-173 не находится в клетке";
                return false;
            }
            if (EventHandlers.mainPlayer != player)
            {
                response = "Кто-то другой уже держит клетку";
            }

            Vector3 center = EventHandlers.scp173.Position;
            float dist = Vector3.Distance(center, player.Position);
            if (dist > 4)
            {
                response = "Подойтиде к SCP-173 поближе и введите команду снова";
                return false;
            }
            if (!EventHandlers.took)
            {
                EventHandlers.took = true;
                EventHandlers.mainPlayer = player;
                if (!EventHandlers.inCorouitne)
                {
                    Timing.RunCoroutine(EventHandlers.scp173InCage(EventHandlers.spawnedSchematic, player));
                }
                response = "Вы успешно взяли клетку";
                return true;

            }
            else
            {
                EventHandlers.took = false;
                EventHandlers.mainPlayer = null;
                response = "Вы успешно опустили клетку";
                return true;
            }

        }
    }
}