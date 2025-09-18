using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using Exiled.API.Features;
namespace MainRPCorePlugin.Features
{
    [HarmonyPatch(typeof(Scp079Recontainer), nameof(Scp079Recontainer.OnServerRoleChanged))]
    public static class Scp079DecontDisabler
    {
        public static bool Prefix(ReferenceHub hub, RoleTypeId newRole, RoleChangeReason reason)
        {
            Player player = Player.Get(hub);
            if (Plugin.Instance.Config.Scp079ReconModule)
            {
                if (player.Role.Type == RoleTypeId.Scp079)
                {

                    if (Generator.List.Where(x => !x.IsEngaged).Count() > 0)
                    {
                        return false;
                    }
                }
                
                
            }
            return true;

        } 
    }
}
