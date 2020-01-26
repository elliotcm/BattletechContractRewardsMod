using BattleTech;
using BattleTech.UI;
using Harmony;
using System.Collections.Generic;
using System;

namespace BattletechContractRewardsMod {
    [HarmonyPatch(typeof(Contract), "FinalizeSalvage")]
    public static class Contract_FinalizeSalvage {
        public static void Prefix(Contract __instance) {
            Mod.Logger.Debug("Finalizing salvage");

            if (shouldGiveRewards(__instance)) {
                Mod.Logger.Info("Providing under-par reward");

                SimGameInterruptManager.RewardsPopupEntry rewardsPopupEntry = new SimGameInterruptManager.RewardsPopupEntry(selectReward(__instance));
                bool playImmediately = false;
                __instance.BattleTechGame.Simulation.GetInterruptQueue().AddInterrupt(rewardsPopupEntry, playImmediately);
            }
        }

        private static string selectReward(Contract contract) {
            float underParBy = amountUnderPar(contract);
            float difficulty = contract.Difficulty;

            if (difficulty >= 5) {
                if (underParBy >= 1.5) {
                    return "itemCollection_loot_ItemTriple_SLDF";
                } else if (underParBy >= 1) {
                    return "itemCollection_loot_ItemDouble_SLDF";
                } else if (underParBy > 0) {
                    return "itemCollection_loot_Item_SLDF";
                }
            } else if (difficulty >= 4) {
                return "itemCollection_loot_ItemDouble_rare";
            } else if (difficulty >= 3) {
                return "itemCollection_loot_Item_rare";
            } else if (difficulty >= 2) {
                return "itemCollection_loot_ItemTriple_uncommon";
            } else if (difficulty >= 1) {
                return "itemCollection_loot_Item_uncommon";
            }
            
            return null;
        }

        private static bool shouldGiveRewards(Contract contract) {
            return isSuccessfulMission(contract) &&
                isUnderPar(contract);
        }

        private static bool isSuccessfulMission(Contract contract) {
            Mod.Logger.Debug($"Successful mission? contract.State {contract.State} == Contract.ContractState.Complete {Contract.ContractState.Complete}: {contract.State == Contract.ContractState.Complete}");

            return contract.State == Contract.ContractState.Complete;
        }

        private static bool isUnderPar(Contract contract) {
            Mod.Logger.Debug($"Under par? amountUnderPar {amountUnderPar(contract)} > 0: {amountUnderPar(contract) > 0}");

            return amountUnderPar(contract) > 0;
        }

        private static float amountUnderPar(Contract contract) {
            SimGameState simGameState = UnityGameInstance.Instance.Game.Simulation;
            float combinedTonnage = 0f;
            
            int lanceTonnageRating = SimGameBattleSimulator.GetLanceTonnageRating(simGameState, mechDefs(contract), out combinedTonnage);

            return contract.Difficulty - lanceTonnageRating;
        }

        private static List<MechDef> mechDefs(Contract contract) {
            return contract.PlayerUnitResults.ConvertAll(new Converter<UnitResult, MechDef>(UnitResultToMechDef));
        }

        private static MechDef UnitResultToMechDef(UnitResult unitResult) {
            return unitResult.mech;
        }
    }
}
