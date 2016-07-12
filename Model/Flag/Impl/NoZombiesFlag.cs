﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rocket.Unturned.Player;
using Safezone.Model.Safezone;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace Safezone.Model.Flag.Impl
{
    public class NoZombiesFlag : BoolFlag
    {
        public override string Description => "Allow/Disallow spawning of zombies in the given safezone";

        public override bool SupportsGroups => false;
        public override void UpdateState(List<UnturnedPlayer> players)
        {
            if (ZombieManager.ZombieRegions == null) return;

            foreach (ZombieRegion t in ZombieManager.ZombieRegions.Where(t => t.Zombies != null))
            {
                // ReSharper disable once MergeSequentialChecks
                foreach (var zombie in t.Zombies.Where(z => z!= null && z.transform?.position != null))
                {
                    if (zombie.isDead) continue;
                    SafeZone safeZone = SafeZonePlugin.Instance?.GetSafeZoneAt(zombie.transform.position);
                    if (safeZone == null || !GetValue<bool>()) continue;
                    zombie.gear = 0;
                    zombie.isDead = true;
                    Vector3 ragdoll = (Vector3)typeof(Zombie).GetField("ragdoll", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(zombie);
                    ZombieManager.sendZombieDead(zombie, ragdoll);
                }
            }
        }

        public override void OnSafeZoneEnter(UnturnedPlayer player)
        {
            //do nothing
        }

        public override void OnSafeZoneLeave(UnturnedPlayer player)
        {
            //do nothing
        }
    }
}