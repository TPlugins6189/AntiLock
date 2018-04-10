using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraConcentratedJuice.AntiLock
{
    public class AntiLockConfiguration : IRocketPluginConfiguration
    {
        public int defaultLocks;
        public bool ignoreAdmins;
        public bool displayMaxlocksNotice;
        public bool displayLockNotice;
        public List<LockGroup> lockGroups;

        public void LoadDefaults()
        {
            defaultLocks = 0;
            ignoreAdmins = true;
            displayMaxlocksNotice = true;
            displayLockNotice = true;

            lockGroups = new List<LockGroup>
            {
                new LockGroup
                {
                    MaxLocks = 5,
                    Permission = "antilock.lock"
                },
                new LockGroup
                {
                    MaxLocks = 10,
                    Permission = "antilock.biglock"
                }
            };
        }
    }
}
