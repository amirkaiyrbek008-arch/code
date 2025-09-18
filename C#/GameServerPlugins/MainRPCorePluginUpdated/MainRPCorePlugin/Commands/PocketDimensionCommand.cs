using CommandSystem;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MainRPCorePlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PocketDimensionCommand : ICommand
    {
        public static List<Player> Cooldown  = new List<Player>();
        public string Command { get; } = "kormannoeismerenie";

        public string[] Aliases { get; } = { "ki", "pc", "pocketdimesnion"};

        public string Description { get; } = "Уйти В Корманное Измерение!";

        public bool SanitizeResponse { get; }
        private static IEnumerator<float> GoPocketV3(Scp106Role scp106)
        {

            scp106.IsSubmerged = true;

            scp106.Owner.EnableEffect<Ensnared>();

            yield return Timing.WaitUntilTrue(() => scp106.SinkholeController.IsHidden);

            scp106.IsSubmerged = false;

            scp106.Owner.EnableEffect<PocketCorroding>();
            scp106.Owner.DisableEffect(EffectType.Ensnared);
            scp106.Owner.DisableEffect(EffectType.Corroding);


            yield return Timing.WaitUntilFalse(() => scp106.SinkholeController.IsHidden);
        }
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player pl = Player.Get(sender);
            if (pl.Role.Type == PlayerRoles.RoleTypeId.Scp106)
            {
                if (Cooldown.Contains(pl))
                {
                    response = "<color=red>Кулдаун Не Прошел!</color>";
                    return false;
                }

                Timing.RunCoroutine(GoPocketV3(pl.Role.As<Scp106Role>()));


                response = "<color=green>Успешно Телепортируешься в карманное измерение.</color>";
                Cooldown.Add(pl);
                Timing.CallDelayed(30f, () =>
                {
                    Cooldown.Remove(pl);
                });
                return true;

            }
            response = "<color=red>Только SCP-106 Можент Использовать Данную Комманду! Или Произошла Ошибка!</color>";
            return false;
        }
    }
}
