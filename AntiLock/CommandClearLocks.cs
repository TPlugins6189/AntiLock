using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace ExtraConcentratedJuice.AntiLock
{
    public class CommandClearLocks : IRocketCommand
    {
        #region Properties

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "clearlocks";

        public string Help => "Clears all of the caller's locks, or another player's lock if an argument is passed.";

        public string Syntax => "/clearlocks <player [optional]>";

        public List<string> Aliases => new List<string> { "unlockall" };

        public List<string> Permissions => new List<string> { "antilock.clearlocks" };

        #endregion

        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length == 1)
            {
                if (!caller.HasPermission("antilock.clearlocks.other"))
                {
                    UnturnedChat.Say(caller, R.Translate("command_no_permission"), Color.red);
                    return;
                }

                UnturnedPlayer other = UnturnedPlayer.FromName(args[0]);

                if (other == null)
                {
                    UnturnedChat.Say(caller, AntiLock.instance.Translate("player_not_found"), Color.red);
                    return;
                }

                int count = ClearLocks(other);
                UnturnedChat.Say(caller, AntiLock.instance.Translate("clear_success_other", other.DisplayName, count));
            }
            else
            {
                int count = ClearLocks((UnturnedPlayer)caller);
                UnturnedChat.Say(caller, AntiLock.instance.Translate("clear_success", count));
            }
        }

        private int ClearLocks(UnturnedPlayer p)
        {
            if (p == null || p.CSteamID == null)
                return 0;

            int count = 0;

            foreach (InteractableVehicle v in VehicleManager.vehicles)
                if (v.lockedOwner == p.CSteamID && v.isLocked)
                {
                    VehicleManager.instance.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        v.instanceID,
                        p.CSteamID,
                        p.Player.quests.groupID,
                        !v.isLocked
                    });
                    count++;
                }

            return count;
        }
    }
}