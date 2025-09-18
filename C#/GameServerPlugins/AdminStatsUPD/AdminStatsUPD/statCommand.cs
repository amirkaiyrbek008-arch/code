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
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace AdminStats
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StatCommand : ICommand
    {
        public string Command => "stats";
        public string[] Aliases => new string[0];
        public string Description => "Показывает статистику времени администраторов.";

        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CL.json");
        private static readonly string filePathAdm = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CLNam.json");

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "Вы ввели неправильно команду. Использование: stat reset/all/steamid";
                return false;
            }

            string arg = arguments.ElementAt(0).ToLower();

            if (arg == "reset")
            {
                if (!sender.CheckPermission(PlayerPermissions.SetGroup))
                {
                    response = "У вас нет прав для выполнения этой команды";
                    return false;
                }
                var emptyDict = new Dictionary<string, object>();
                string clearedJson = JsonConvert.SerializeObject(emptyDict, Formatting.Indented);
                File.WriteAllText(filePath, clearedJson);
                File.WriteAllText(filePathAdm, clearedJson);
                EventHandlers.names.Clear();
                EventHandlers.playTime.Clear();
                response = "Статистика администраторов сброшена.";
                return true;
            }
            else if (arg == "all")
            {
                if (EventHandlers.playTime.Count == 0)
                {
                    response = "Нет данных.";
                    return true;
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Таблица администраторов:");

                foreach (var kv in EventHandlers.playTime.Keys)
                {
                    foreach (var nam in EventHandlers.names.Keys)
                    {
                        if (nam.ToString() == kv.ToString())
                        {
                            if (EventHandlers.playTime[kv] > 60)
                            {
                                int hours = EventHandlers.playTime[kv] / 60;
                                int minutes = EventHandlers.playTime[kv] % 60;
                                sb.AppendLine($"{EventHandlers.names[nam]}({nam}) — {hours} часов, {minutes} минут");
                            }
                            else
                            {
                                sb.AppendLine($"{EventHandlers.names[nam]}({nam}) — {EventHandlers.playTime[kv]} мин");
                            }
                        }
                    }
                }

                response = sb.ToString();
                return true;
            }
            else
            {
                string nickName = null;
                string ids = null;
                foreach (var nam in EventHandlers.names)
                {
                    if (nam.Key.ToString() == arg.ToString())
                    {
                        nickName = nam.Value.ToString();
                        ids = nam.Key.ToString();
                        break;
                    }
                }
                if (EventHandlers.playTime.TryGetValue(arg, out int minutes))
                {
                    if (minutes > 60)
                    {
                        int hours = minutes / 60;
                        int minute = minutes % 60;
                        response = $"{nickName}({ids}) — {hours} часов, {minute} минут";
                    }
                    else
                    {
                        response = $"{nickName}({ids}) — {minutes} мин";
                    }
                    return true;
                }
                else
                {
                    response = "Нет данных для указанного игрока.";
                    return false;
                }
            }
        }
    }
}


