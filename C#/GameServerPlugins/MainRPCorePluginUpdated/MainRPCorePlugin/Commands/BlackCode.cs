using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace MainRPCorePlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class BlackCode: ICommand
    {
        
        public string Command { get; } = "blackcode";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Запускает BlackCode(Читай Правила!)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player senderpl = Player.Get(sender);

            AudioPlayer audioPlayer = AudioPlayer.Create("BlackCodeSequence");
            Speaker speaker = audioPlayer.AddSpeaker("BlackCode", 1f, false, 3f, 5000f);
            AudioClipPlayback audioClipPlayback = audioPlayer.AddClip("blackcode_snd", 1f, false, true);
            foreach (Room room in Room.List)
            {
                room.Color = Color.Blue;
            }

            Timing.CallDelayed(63, () =>
            {
                Plugin.customOtrads.SpawnAlpha1(Player.List.Where(x => x.IsDead).ToList());

            });
            if (audioClipPlayback.IsPaused == false)
            {
                response = "Успещно активирован BlackCode!\n";
                return true;
            }

            response = "Что-то пошло не так.";
            return false;
        }

    }
}
