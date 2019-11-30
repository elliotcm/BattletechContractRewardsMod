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

    [HarmonyPatch(typeof(Contract), "GenerateSalvage")]
    public static class Contract_GenerateSalvage {
        public static void Prefix(Contract __instance) {
            Mod.Logger.Info("======= Generating salvage =======");
            //__instance.BattleTechGame.Simulation.GetInterruptQueue().QueueRewardsPopup("itemCollection_loot_Item_rare");
        }
    }
}
