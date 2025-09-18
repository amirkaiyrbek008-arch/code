using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventHandlers : CustomEventsHandler
{
    private List<Player> plrsInRange = new List<Player>();
    Player scpName;
    private string scpSteamId = string.Empty;
    CoroutineHandle handler;
    float radius = 20f;
    Room scp106;

    IEnumerator<float> disableInvisibility(Player player)
    {
        player.DisableEffect<CustomPlayerEffects.Invisible>();
        yield return Timing.WaitForSeconds(5f);
        player.EnableEffect<CustomPlayerEffects.Invisible>(1, 0, false);
    }

    IEnumerator<float> enemyHit(Player player)
    {
        player.DisableEffect<CustomPlayerEffects.Invisible>();
        yield return Timing.WaitForSeconds(0.5f);
        player.EnableEffect<CustomPlayerEffects.Invisible>(1, 0, false);
    }

    IEnumerator<float> hintForPlayers(Player bess)
    {
        while (true)
        {
            foreach (Player plr in Player.List)
            {
                if (plr != bess || !plr.IsSCP || plr.IsHuman)
                {
                    func1(plr, bess);
                }
            }
            yield return Timing.WaitForSeconds(1);
        }
    }

    public void func1(Player player, Player bess)
    {
        Vector3 center = bess.Position;
        float dist = Vector3.Distance(player.Position, center);
        if (radius > dist)
        {
            player.SendHint("Вы чувствуете чьё-то присутствие");
        }
    }

    public override void OnServerRoundStarted()
    {
        Timing.CallDelayed(0.5f, () =>
        {
            Player target = Player.List.FirstOrDefault(p => p.Role.GetTeam() == Team.SCPs);
            if (target != null)
            {
                if (Player.List.Count > 16 && scpSteamId == string.Empty)
                {
                    Spawn966(target);
                }
            }
        });
    }

    public void Spawn966(Player player)
    {
        player.SetRole(RoleTypeId.Scp0492);
        handler = Timing.RunCoroutine(hintForPlayers(player));
        player.DisplayName = $"[SCP-966] {player.Nickname}";

        foreach(Room room in Room.List)
        {
            if(room.Name == RoomName.Hcz106)
            {
                scp106 = room;
            }
        }

        player.Position = scp106.Position + new Vector3(0f, 1f, 0f);
        player.MaxHealth = 1000;
        player.Health = 1000;
        player.HumeShield = 300;
        player.HumeShieldRegenRate = 5;
        player.SendBroadcast("Вы - <color=red>SCP-966</color>", 15);
        player.EnableEffect<CustomPlayerEffects.Invisible>(1, 0, false);
        scpName = player;
        scpSteamId = player.UserId;
    }

    private void Clear966State()
    {
        if (!string.IsNullOrEmpty(scpSteamId))
        {
            if (scpName != null)
                scpName.DisplayName = null;

            scpSteamId = string.Empty;
            scpName = null;
            if (handler.IsRunning)
                Timing.KillCoroutines(handler);
        }
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        if (ev.Player.UserId == scpSteamId)
            Clear966State();
    }

    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
    {
        if (ev.Player.UserId == scpSteamId)
            Clear966State();
    }

    public override void OnPlayerDeath(PlayerDeathEventArgs ev)
    {
        if (ev.Player.UserId == scpSteamId)
            Clear966State();
    }

    public override void OnServerRoundEnded(RoundEndedEventArgs ev)
    {
        Clear966State();
    }

    public override void OnScp0492ConsumingCorpse(Scp0492ConsumingCorpseEventArgs ev)
    {
        if (ev.Player.UserId == scpSteamId)
            ev.IsAllowed = false;
    }

    public override void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev)
    {
        if (ev.Player.UserId == scpSteamId && ev.Player.Role == RoleTypeId.Scp0492)
        {
            Timing.RunCoroutine(disableInvisibility(ev.Player));
        }
    }

    public override void OnPlayerInteractingElevator(PlayerInteractingElevatorEventArgs ev)
    {
        if (ev.Player.UserId == scpSteamId && ev.Player.Role == RoleTypeId.Scp0492)
        {
            Timing.RunCoroutine(disableInvisibility(ev.Player));
        }
    }

    public override void OnScp049StartingResurrection(Scp049StartingResurrectionEventArgs ev)
    {
        if (ev.Player.UserId == scpSteamId)
            ev.IsAllowed = false;
    }

    public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        if (ev.Attacker.UserId == scpSteamId && ev.Attacker.Role == RoleTypeId.Scp0492)
        {
            if(ev.Player.CustomInfo == "SCP-035")
            {
                if (ev.DamageHandler is AttackerDamageHandler handler)
                {
                    handler.Damage = 20f;
                    ev.Attacker.EnableEffect<CustomPlayerEffects.Invisible>(1, 0, false);
                    Timing.RunCoroutine(enemyHit(ev.Attacker));
                }
            }
        }
    }
}
