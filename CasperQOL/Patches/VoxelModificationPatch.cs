using HarmonyLib;
using System;
using UnityEngine;

namespace CasperQOL
{
    [HarmonyPatch(typeof(PendingVoxelChanges), "TryDig")]
    public static class VoxelModificationPatch
    {
        public static int MaxIntegrity = 10000;

        [HarmonyPostfix]
        public static void TryDigPostfix(PendingVoxelChanges __instance, Vector3Int coord, int digStrength, int miningTier, ref int numResourcesTaken, bool __result)
        {
            if (SharedState.oreProtect && !__result && numResourcesTaken > 0)
            {
                try
                {
                    int chunkId = VoxelManager.GetChunkId(coord.x, coord.y, coord.z);
                    int orCreateIndexForChunk = __instance.GetOrCreateIndexForChunk(chunkId);
                    ref ChunkPendingVoxelChanges chunkChanges = ref __instance.chunkData[orCreateIndexForChunk];
                    int voxelIndex = chunkChanges.GetIndex(coord.x, coord.y, coord.z);
                    ref ModifiedCoordData voxelData = ref chunkChanges.GetOrAdd(voxelIndex);

                    int restoreAmount = Math.Min(numResourcesTaken, MaxIntegrity - voxelData.integrity);
                    voxelData.integrity += restoreAmount;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error in TryDigPostfix: {ex.Message}");
                }
            }
        }
    }
}
