using BattleTech;
using BattleTech.UI;
using Harmony;
using System.Collections.Generic;

namespace BattletechContractRewardsMod {
    [HarmonyPatch(typeof(Contract), "FinalizeSalvage")]
    public static class Contract_FinalizeSalvage {
        public static void Prefix(List<SalvageDef> priorityItems, Contract __instance) {
            Mod.Logger.Info($"======= Finalizing salvage with {priorityItems.Count.ToString()} priority items =======");

            SimGameInterruptManager.RewardsPopupEntry rewardsPopupEntry = new SimGameInterruptManager.RewardsPopupEntry("itemCollection_loot_Item_rare");
            bool playImmediately = false;
            __instance.BattleTechGame.Simulation.GetInterruptQueue().AddInterrupt(rewardsPopupEntry, playImmediately);
        }
    }
}
