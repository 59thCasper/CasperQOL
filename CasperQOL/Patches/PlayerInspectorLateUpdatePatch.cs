using HarmonyLib;
using UnityEngine;

namespace CasperQOL.Patches
{
    [HarmonyPatch(typeof(PlayerInspector), "LateUpdate")]
    public static class PlayerInspectorLateUpdatePatch
    {
        static void Postfix(PlayerInspector __instance)
        {
            Transform playerCamera = AccessTools.FieldRefAccess<PlayerInspector, Transform>(__instance, "playerCamera");
            if (playerCamera == null)
            {
                Debug.LogWarning("PlayerInspectorLateUpdatePatch: playerCamera is null.");
                return;
            }

            LayerMask collisionLayers = AccessTools.FieldRefAccess<PlayerInspector, LayerMask>(__instance, "collisionLayers");
            Ray ray = new Ray(playerCamera.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, collisionLayers))
            {
                SharedState.stoodOn = __instance.machineHit.IsValid() ? __instance.machineHit.MyBuilderInfo.displayName : "";
            }
            else
            {
                SharedState.stoodOn = "";
            }
        }
    }
}
