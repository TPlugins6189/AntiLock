using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtraConcentratedJuice.AntiLock
{
    internal static class AskVehicleLockOverride
    {
        internal static bool Prefix(CSteamID steamID)
        {
            UnturnedPlayer player = UnturnedPlayer.FromCSteamID(steamID);

            if (player == null)
                return false;

            if (player.IsAdmin && AntiLock.instance.Configuration.Instance.ignoreAdmins)
                return true;

            InteractableVehicle vehicle = player.Player.movement.getVehicle();

            if (vehicle == null || !vehicle.checkDriver(steamID))
                return false;

            if (vehicle.isLocked)
                return true;

            LockGroup group = AntiLock.instance.Configuration.Instance.lockGroups
                .Where(x => player.HasPermission(x.Permission))
                .OrderByDescending(x => x.MaxLocks)
                .FirstOrDefault();

            int max = group == null ? AntiLock.instance.Configuration.Instance.defaultLocks : group.MaxLocks;
            int count = VehicleManager.vehicles.Count(x => x.lockedOwner == steamID && x.isLocked);

            if (count >= max)
            {
                if (AntiLock.instance.Configuration.Instance.displayMaxlocksNotice)
                    UnturnedChat.Say(player, AntiLock.instance.Translate("max_locked_notice", max), Color.red);

                return false;
            }

            UnturnedChat.Say(player, AntiLock.instance.Translate("locked_notice", max - count - 1, max));

            return true;
        }

        internal static void Postfix() { /* lol */}
    }
}
