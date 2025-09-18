using LabApi.Events.CustomHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabApi.Features.Wrappers;
using LabApi.Events.Arguments.PlayerEvents;
using PlayerRoles;


namespace DoorNoAccessForSCP
{
    public class EventHandlers : CustomEventsHandler
    {
        public override void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev)
        {
            if (!ev.Player.IsSCP)
            {
                return; 
            }
            else
            {
                if (ev.Player.Role == RoleTypeId.Scp106 || ev.Player.Role == RoleTypeId.Scp173 || ev.Player.Role == RoleTypeId.Scp939 || ev.Player.Role == RoleTypeId.Scp096)
                {
                    if (ev.Door.IsOpened)
                    {
                        if (ev.Door.IsLocked == false)
                        {
                            ev.CanOpen = false;
                        }
                    }
                }
            }
        }
    }
}
