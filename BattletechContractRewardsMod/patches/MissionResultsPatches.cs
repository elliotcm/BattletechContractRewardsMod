using BattleTech;
using BattleTech.UI;
using Harmony;

namespace BattletechContractRewardsMod {
    [HarmonyPatch(typeof(MissionResults), "OnContinueClicked")]
    public static class MissionResults_OnContinueClicked {
        public static void Prefix(MissionResults __instance, ref SimGameState ___simState) {
            Mod.Logger.Info("Contract completed, triggering reward");
            ___simState.GetInterruptQueue().QueueRewardsPopup("itemCollection_loot_Item_rare");
        }
    }
}
