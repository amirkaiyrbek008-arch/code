using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRPCorePlugin.CustomItems
{
    public class Shifrator: CustomItemBase
    {
        public Player owner;
        public override void OnChangingItem(ChangedItemEventArgs ev)
        {
            if (ev.Item == item)
            {
                ev.Player.Broadcast(3, "<color=#00B7EB>Вы Держите В Руках Шифратор!</color>");
            }
        }
        public override void OnPickup(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.Serial == item.Serial)
            {
                ev.Player.Broadcast(3, "<color=#00B7EB>Вы Подобрали Шифратор!</color>");
                owner = ev.Player;
                Scp096Role.TurnedPlayers.Add(ev.Player);
            }
        }

        public void OnItemDrop(DroppingItemEventArgs ev)
        {
            if (ev.Item.Serial == item.Serial)
            {
                ev.Player.Broadcast(3, "<color=#00B7EB>Вы Выбросили Шифратор!</color>");
                owner = null;
                Scp096Role.TurnedPlayers.Remove(ev.Player);
            }
        }
        public override void InitCustomEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnItemDrop;
        }
        
        
        public static Shifrator Create()
        {
            Shifrator shifrator = new Shifrator();
            shifrator.item = Item.Create(ItemType.Radio);
            customitems.Add(shifrator.item.Serial, shifrator);
            shifrator.Init();
            return shifrator;
        }
    }
}
