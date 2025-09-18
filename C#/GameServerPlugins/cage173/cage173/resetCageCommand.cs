using CommandSystem;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ICommand = CommandSystem.ICommand;
using System.IO;

namespace cage173
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class resetCageCommand : ICommand
    {
        public string Command => "resetcage";
        public string[] Aliases => new string[0];
        public string Description => "Обновляет плагин на клетку 173(ВВОДИТЬ ТОЛЬКО ЕСЛИ 173 НЕ МОЖЕТ УБИВАТЬ БЕЗ КЛЕТКИ ИЛИ ПРИ НАПИСАНИИ КОМАНДЫ .cage ПИШЕТ ЧТО НА 173 УЖЕ ЕСТЬ КЛЕТТКА.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            EventHandlers.ResetCageState();
            if(EventHandlers.spawnedSchematic != null)
            {
                EventHandlers.spawnedSchematic.Destroy ();
            }
            response = "Плагин на клетку был обновлен";
            return true;
        }
    }
}

