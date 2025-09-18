using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using LiteNetLib;
using MainRPCorePlugin.CustomItems;
using MainRPCorePlugin.Features;
using MEC;
using ProjectMER.Features;
using UnityEngine;

namespace MainRPCorePlugin
{
    class Plugin : Plugin<Config>
    {
        public override string Name => "MainPlugin";
        public override string Author => "Mariki";
        public static Plugin Instance;
        public Harmony harmony;
        public static CustomNicknames customnickanmes;
        public static HintSystem hintSystem;
        public static CustomOtrads customOtrads;
        public static TaskSystem tasksys;
        public static BlackCode blackCode;
        public static List<CoroutineHandle> Coroutines { get; } = new List<CoroutineHandle>();
        static bool bSendA = false;
        static bool bSendB = false;
        ushort KarholderInter = ushort.MaxValue;
        CoroutineHandle handle;
        public override void OnEnabled()
        {
            Instance = this;
            harmony = new Harmony("marikis-mrp");
            customnickanmes = new CustomNicknames();
            customnickanmes.Init();
            hintSystem = new HintSystem();
            tasksys = new TaskSystem();
            tasksys.Init();
            customOtrads = new CustomOtrads();
            blackCode = new BlackCode();
            blackCode.Init();
            customOtrads.Init();
            hintSystem.Init();  
            harmony.PatchAll();
            Exiled.Events.Handlers.Server.RoundStarted += HacktoolExtensions.SetupHashesToDoor;
            Exiled.Events.Handlers.Player.InteractingDoor += OnPlayerOpeningDoor;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestarting;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickup;

            base.OnEnabled();
        }
        
        public void OnRoundStart()
        {
            Room room = Room.Get(Exiled.API.Enums.RoomType.EzIntercom);
            ObjectSpawner.SpawnSchematic("Kartholder", room.WorldPosition(new Vector3(-2.857f, -4.885f, -3.756f)), room.Rotation.eulerAngles + new Vector3(0, 90, 10), new Vector3(0.2f, 0.2f, 0.2f));
            Pickup pickup = Pickup.CreateAndSpawn(ItemType.SCP500, room.WorldPosition(new Vector3(-2.857f, -4.865f, -3.756f)), Quaternion.Euler(room.Rotation.eulerAngles + new Vector3(0, 0, 90)));
            pickup.Scale = new Vector3(0.2f, 1, 1.3f);
            pickup.Rigidbody.useGravity = false;
            pickup.Rigidbody.velocity = Vector3.zero;
            KarholderInter = pickup.Serial;
            pickup.PickupTime = 10000;
        }
        public void OnPickup(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.Serial != KarholderInter)
                return;
            ev.IsAllowed = false;
            if (ev.Player.IsCHI)
            {
                if (ev.Player.CurrentItem != null)
                {
                    if (ev.Player.CurrentItem.Type == ItemType.KeycardChaosInsurgency)
                    {
                        if (ev.Player.CurrentRoom.Type == Exiled.API.Enums.RoomType.EzIntercom)
                        {
                            Cassie.Message("Процесс авторизации, ЗАВЕРШЕН | Результаты: неизвестный USB-накопитель ¥× уровнем доступа к C.A.S.S.I.I₱!# | Доступ РАЗРЕШЕН |<color=red> Загрузка нового пользователя в сис...</color> ] <size=0> pitch_1 attention . authorized process . . completed . . results . . . unknown usbdrive with level access of yd_0.01 jam_49_9 yd_0.01 jam_49_9 yd_0.01 jam_49_9 cassie . . access granted . pitch_2.5 .g5 .g5 pitch_3.0 .g5 pitch_4.5 .g4 .g5 .g3 pitch_1 loading new user in pitch_1 jam_58_40 system pitch_0.15 .g4 jam_20_10 .g5 pitch_1 jam_30_4 Pitch_0.2 .g4 .g3 .g5 jam_60_15 .g4 jam_20_3 .g3 jam_70_3 .g6 Bell_End Pitch_1 . . . Bell_start Bell_start . pitch_0.6 jam_10_4 .g4 Pitch_2 .g1 .g2 .g4 .g4 .g3 .g2 Pitch_3 .g1 .g2 .g4 .g4 .g3 .g2 Pitch_4 .g1 .g2 .g4 .g4 .g3 .g2 .g1 .g2 .g4 .g4 .g3 .g2 .g1 .g2 .g4 .g4 .g3 .g2 Pitch_6 .g1 .g2 .g4 .g4 .g4 g4 .g4 . pitch_0.1 .g7", isSubtitles: true);
                            Timing.CallDelayed(10f, () => { Intercom.State = PlayerRoles.Voice.IntercomState.NotFound; });
                            return;
                        }
                        else
                        {
                            return;
                        }

                    }
                }
                else
                {
                    return;
                }
            }
            else if (ev.Player.IsFoundationForces)
            {
                if (ev.Player.CurrentItem != null)
                {
                    if (ev.Player.CurrentItem.Type == ItemType.KeycardMTFCaptain)
                    {
                        if (ev.Player.CurrentRoom.Type == Exiled.API.Enums.RoomType.EzIntercom)
                        {
                            Timing.CallDelayed(10f, () => { Intercom.State = Intercom.State = PlayerRoles.Voice.IntercomState.Ready; });
                            
                            Cassie.Message("Pitch_20 <b><color=red>За!$ск си^#@м ск!%иров%!ния</color> . . .<split><split><split>| <color=red>Угроза обна&@ен в c)%(тем!%АССИ</color> <split><split><split>| <color=red>Акт!:№;а:ия Фа\"*(:в\":ла</color>. . . .<split><split>| <color=green>Угроза была устранена </color>| <split><color=green>Интерком в рабочем режиме </color>|<size=0> pitch_0.8 jam_041_3 Started jam_055_4 SCANNING jam_023_5 system pitch_0.2 .g2 .g2 .g2 . pitch_0.8 threat has jam_055_5 spotted in jam_055_6 CASSIE system . jam_055_4 ACTIVATING jam_044_3 fire Jam_043_4 wall pitch_0.2 .g5 .g5 .g5 pitch_0.4 .g5 pitch_0.6 .g5 pitch_0.8 .g5 pitch_1 .g5 . threat has been terminated . cassie in working jam_015_3 order", isSubtitles: true);
                            return;
                        }
                        else
                        {
                            return;
                        }

                    }
                }
                else
                {
                    return;
                }
            }
        }
        public void OnRestarting()
        {
            bSendA = false;
            bSendB = false;
        }
        public void OnPlayerOpeningDoor(InteractingDoorEventArgs ev)
        {
            
            if (ev.Player.IsCHI && ev.Door.Type == Exiled.API.Enums.DoorType.GateA && !bSendA)
            {
                bSendA = true;
                Cassie.Message("<color=#FBBC13><b>Внимание! </b>| </color>Неавторизованный доступ к системе ворот | <color=#237F0A>Загрузка </color><b>неизвестного программного обеспечения </b>| . . . | Загрузка <color=#237F0A><b>завершена </b></color>| Ворота Альфа <color=#237F0A><b>открыты </b></color>и <color=#A30505><b>отключены </b></color>| Перезапуск системы ворот Альфа | <color=#A30505><b>Ошибка </b></color>| <color=#FBBC13><b>Внимание </b></color><color=orange>инженерному </color>отряду, направляйтесь к воротам Альфа для проверки систем <size=0> pitch_1 attention . unauthorized access in gates system . loading unknown software . pitch_3.5 .g1 .g1 .g1 .g1 .g1 .g1 . . .g2 .g2 .g2 .g2 .g2 .g2 . . .g3 .g3 .g3 .g3 .g3 pitch_1 . loading completed . . gate nato_a open and deactivated . . reactivation of gate nato_a system . pitch_2 .g3 pitch_1 . pitch_2 .g3 pitch_1 . pitch_2 .g3 pitch_1 . pitch_2 .g3 pitch_1 . failure . attention to engine squad . report to gate nato_a for check system", isSubtitles: true);
            }
            if (ev.Player.IsCHI && ev.Door.Type == Exiled.API.Enums.DoorType.GateB && !bSendB)
            {
                bSendB = true;
                Cassie.Message("<color=#FBBC13><b>Внимание! </b>| </color>Неавторизованный доступ к системе ворот | <color=#237F0A>Загрузка </color><b>неизвестного программного обеспечения </b>| . . . | Загрузка <color=#237F0A><b>завершена </b></color>| Ворота Браво <color=#237F0A><b>открыты </b></color>и <color=#A30505><b>отключены </b></color>| Перезапуск системы ворот Браво | <color=#A30505><b>Ошибка </b></color>| <color=#FBBC13><b>Внимание </b></color><color=orange>инженерному </color>отряду, направляйтесь к воротам Браво для проверки систем <size=0> pitch_1 attention . unauthorized access in gates system . loading unknown software . pitch_3.5 .g1 .g1 .g1 .g1 .g1 .g1 . . .g2 .g2 .g2 .g2 .g2 .g2 . . .g3 .g3 .g3 .g3 .g3 pitch_1 . loading completed . . gate nato_b open and deactivated . . reactivation of gate nato_b system . pitch_2 .g3 pitch_1 . pitch_2 .g3 pitch_1 . pitch_2 .g3 pitch_1 . pitch_2 .g3 pitch_1 . failure . attention to engine squad . report to gate nato_b for check system", isSubtitles: true);
            }
        }
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp079)
            {
                ev.Player.Role.As<Scp079Role>().Experience = 5000;
            }
        }
        
        public override void OnDisabled()
        {
            Instance = this;
            customnickanmes = new CustomNicknames();
            customnickanmes.DeInit();
            hintSystem = new HintSystem();
            harmony = new Harmony("marikis-mrp");
            base.OnDisabled();
        }
    }
}
