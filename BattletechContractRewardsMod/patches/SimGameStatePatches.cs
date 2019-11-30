using BattleTech;
using Harmony;

namespace BattletechContractRewardsMod {
    [HarmonyPatch(typeof(SimGameState), "OnCompleteContract")]
    public static class SimGameState_OnCompleteContract {
        public static void Prefix(Contract contract, SimGameState __instance) {
            Mod.Logger.Info("Contract completed, triggering reward");
            __instance.GetInterruptQueue().QueueRewardsPopup("itemCollection_loot_Item_rare");
        }
    }
}
