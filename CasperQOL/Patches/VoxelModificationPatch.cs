using CasperQOL;
using HarmonyLib;
using System;
using UnityEngine; // For Vector3Int and Debug

namespace YourNamespace
{
    [HarmonyPatch(typeof(PendingVoxelChanges), "TryDig")]
    public static class VoxelModificationPatch
    {
        public static int MaxIntegrity = 8000; // Define the maximum integrity based on your game's balance requirements

        [HarmonyPostfix]
        public static void TryDigPostfix(PendingVoxelChanges __instance, Vector3Int coord, int digStrength, int miningTier, ref int numResourcesTaken, bool __result)
        {
            //Debug.Log($"TryDig called with coordinates: {coord}, digStrength: {digStrength}, miningTier: {miningTier}, numResourcesTaken: {numResourcesTaken}, digResult: {__result}");

            if (SharedState.oreProtect && !__result && numResourcesTaken > 0)
            {
                try
                {
                    int chunkId = VoxelManager.GetChunkId(coord.x, coord.y, coord.z);
                    int orCreateIndexForChunk = __instance.GetOrCreateIndexForChunk(chunkId);
                    ref ChunkPendingVoxelChanges chunkChanges = ref __instance.chunkData[orCreateIndexForChunk];
                    int voxelIndex = chunkChanges.GetIndex(coord.x, coord.y, coord.z);
                    ref ModifiedCoordData voxelData = ref chunkChanges.GetOrAdd(voxelIndex);

                    //Debug.Log($"Voxel pre-modification - Type: {voxelData.curVoxType}, Integrity: {voxelData.integrity}");

                    // Ensure integrity does not exceed the original or max allowed integrity
                    int restoreAmount = Math.Min(numResourcesTaken, MaxIntegrity - voxelData.integrity);
                    voxelData.integrity += restoreAmount;

                    //Debug.Log($"Integrity restored by {restoreAmount} units due to ore protection. New Integrity: {voxelData.integrity}");
                }
                catch (Exception ex)
                {
                    //Debug.LogError($"Error in TryDigPostfix: {ex.Message}");
                }
            }
            else
            {
                //Debug.Log("Ore protection is off or dig result was true (voxel destroyed) or no resources were taken.");
            }
        }
    }
}
