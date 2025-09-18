
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Xml;
using LabApi.Features.Wrappers;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace AdminStats
{
    public static class PlayerTimeStorage
    {
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CL.json");
        private static readonly string filePathAdm = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CLNam.json");
        public static Dictionary<string, int> Load()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "{}");
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
        }
        public static Dictionary<string, string> LoadNames()
        {
            if (!File.Exists(filePathAdm))
            {
                File.WriteAllText(filePathAdm, "{}");
            }

            string json = File.ReadAllText(filePathAdm);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }

        public static void Save(Dictionary<string, int> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static void SaveNames(Dictionary<string, string> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePathAdm, json);
        }

        public static void AddPlayTime(Player player, int minutes)
        {
            var data = Load();
            string playerId = player.UserId;

            if (data.ContainsKey(playerId))
                data[playerId] += minutes;
            else
                data[playerId] = minutes;

            Save(data);
        }
    }
}

