using CommandSystem;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ICommand = CommandSystem.ICommand;
using System.IO;
using LabApi.Features.Permissions;
using MapGeneration;

namespace NoDBlock
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StatCommand : ICommand
    {
        public string Command => "unlocksD";
        public string[] Aliases => new string[0];
        public string Description => "Открывает Двери Д Блока";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            Room dClassRoom = Room.Get(RoomName.LczClassDSpawn).FirstOrDefault();

            foreach (var door in dClassRoom.Doors)
            {
                door.IsLocked = false;
            }

            response = "Команда была успешно введена";
            return true;
        }
    }
}

