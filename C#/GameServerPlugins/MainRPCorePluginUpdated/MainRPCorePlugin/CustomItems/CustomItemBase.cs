using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Features.Items;
namespace MainRPCorePlugin.CustomItems
{
    public abstract class CustomItemBase
    {
        public static Dictionary<ushort, CustomItemBase> customitems = new Dictionary<ushort, CustomItemBase>();
        public string Name;
        public Item item;
        
        public void Init()
        {
            Exiled.Events.Handlers.Player.PickingUpItem += OnPickup;
            Exiled.Events.Handlers.Player.ChangedItem += OnChangingItem;
            InitCustomEvents();
        }

        public abstract void InitCustomEvents();

        public abstract void OnPickup(Exiled.Events.EventArgs.Player.PickingUpItemEventArgs ev);
        public abstract void OnChangingItem(Exiled.Events.EventArgs.Player.ChangedItemEventArgs ev);

    }
}
