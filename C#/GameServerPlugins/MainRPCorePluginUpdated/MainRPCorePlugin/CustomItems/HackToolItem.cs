//using Exiled.API.Features;
//using Exiled.API.Features.Doors;
//using Exiled.API.Features.Items;
//using Exiled.Events.EventArgs.Player;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MainRPCorePlugin.CustomItems
//{
    
//    public class HackToolItem: CustomItemBase
//    {
//        public Player owner;

//        Door curdoor;

//        List<Door> saveddoors = new List<Door>();

//        public int curidx = 0;
//        public bool IsHacking = false;

//        public string curhash = "9999";
//        public override void OnChangingItem(ChangedItemEventArgs ev)
//        {
//            if (ev.Item == item)
//            {
//                ev.Player.Broadcast(3, "<color=#00B7EB>Вы Держите В Руках Устройство Для Взлома!</color>");
//            }
//        }
        
//        public void OnChangingItem(ChangingItemEventArgs ev)
//        {
//            if (ev.Player == owner &&IsHacking)
//            {
//                ev.IsAllowed = false;
//                if (ev.Item.Type == ItemType.Adrenaline || ev.Item.Type == ItemType.SCP500 || ev.Item.Type == ItemType.Painkillers || ev.Item.Type == ItemType.Medkit)
//                {
//                    if (curidx < 3)
//                    {
//                        curidx++;
//                    }
//                }
//                else if (ev.Item.IsKeycard)
//                {
//                    if (curidx > 0)
//                    {
//                        curidx--;
//                    }
//                }
//                else
//                {
//                    IsHacking = false;
//                    ev.IsAllowed = true;
//                }
//            }
//        }
//        public void OnOpeningDoor(InteractingDoorEventArgs ev)
//        {
//            if (ev.Player == owner && ev.Player.CurrentItem == item && ev.Door.KeycardPermissions != Exiled.API.Enums.KeycardPermissions.None)
//            {
//                if (!saveddoors.Contains(ev.Door)&& !IsHacking)
//                {
//                    Log.Info(HacktoolExtensions.GetDoorUnlockCode(ev.Door));
//                    ev.IsAllowed = false;
//                    curdoor = ev.Door;
//                    curidx = 0;
//                    curhash = "9999";
//                    ev.Player.AddItem(ItemType.Adrenaline);
//                    IsHacking = true;
//                }
//            }
//        }
//        public void OnNoclip(TogglingNoClipEventArgs ev)
//        {
//            if (ev.Player == owner && IsHacking)
//            {
//                ev.IsAllowed = false;
//                string val = "";
//                if (curhash[curidx] == '9')
//                {
//                    val = "0";
//                }
//                else if (curhash[curidx] == '0')
//                {
//                    val = "1";
//                }
//                else
//                {
//                    val = "0";
//                }
//                curhash = curhash.Substring(0, curidx) + val + curhash.Substring(curidx + 1);
//                if (curhash == HacktoolExtensions.GetDoorUnlockCode(curdoor))
//                {
//                    saveddoors.Add(curdoor);
//                    curdoor.IsOpen = !curdoor.IsOpen;
//                    IsHacking = false;
//                }
//            }
//        }
//        public override void InitCustomEvents()
//        {
//            Exiled.Events.Handlers.Player.TogglingNoClip += OnNoclip;
//            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
//            Exiled.Events.Handlers.Player.InteractingDoor += OnOpeningDoor;
//        }
//        public override void OnPickup(PickingUpItemEventArgs ev)
//        {
//            if (ev.Pickup.Serial == item.Serial)
//            {
//                ev.Player.Broadcast(3, "<color=#00B7EB>Вы Подобрали Устройство Для Взлома!</color>");
//                owner = ev.Player;
                
//            }
//        }
//        public static HackToolItem Create()
//        {
//            HackToolItem hacktool = new HackToolItem();
//            hacktool.item = Item.Create(ItemType.KeycardChaosInsurgency);
//            customitems.Add(hacktool.item.Serial, hacktool);
//            hacktool.Init();
//            return hacktool;
//        }
//    }
//    public static class HacktoolExtensions {
//        public static Dictionary<Door, string> doors = new Dictionary<Door, string>();
//        public static Dictionary<string, string> colors = new Dictionary<string, string>() { { "1", "#ff0000" }, { "0", "#0000ff" } };

//        public static string GetDoorUnlockCode(this Door door)
//        {
//            return doors[door];

//        }
//        public static string GetRawFormat(HackToolItem item)
//        {
//            string str = "";
//            for (int i = 0; i<item.curhash.Length; i++)
//            {
//                string key = "";
//                if (item.curhash[i] == '0')
//                {
//                    key = "0";
//                }
//                else if (item.curhash[i] == '1')
//                {
//                    key = "1";
//                }
//                string color = "";
//                if (key == "")
//                {
//                    color = "#FFFFFF";
//                }
//                else
//                {
//                    color = colors[key];
//                }
//                if (i == item.curidx)
//                {
//                    str += $"<size=115%><color={color}>█</color></size>";
//                }
//                else
//                {
//                    str+= $"<color={color}>█</color>";
//                }
                
//            }
//            return str;
//        }
//        public static void SetupHashesToDoor()
//        {
//            foreach (Door door in Door.List)
//            {
//                string hash = GenerateHash();
//                doors.Add(door, hash);
//            }

//        }
//        public static string GenerateHash()
//        {
//            Random random = new Random();
//            string binaryString = "";

//            for (int i = 0; i < 4; i++)
//            {
//                binaryString += random.Next(2).ToString();
//            }
//            return binaryString;
//        }

//    }


//}
