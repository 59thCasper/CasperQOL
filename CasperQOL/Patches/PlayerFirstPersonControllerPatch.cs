using HarmonyLib;
using UnityEngine;

namespace CasperQOL.Patches
{

    internal static class PlayerFirstPersonControllerPatch
    {
        [HarmonyPatch(typeof(PlayerFirstPersonController), "FixedUpdate")]
        public static class SpeedModifierPatch
        {
            static void Prefix(PlayerFirstPersonController __instance)
            {
                if (__instance.m_IsGrounded && SharedState.ValidResourceNames.Contains(SharedState.stoodOn))
                {
                    UpdateSpeed(__instance, SharedState.CustomMaxRunSpeed, SharedState.CustomMaxWalkSpeed);
                }
                else
                {
                    UpdateSpeed(__instance, SharedState.DefaultMaxRunSpeed, SharedState.DefaultMaxWalkSpeed);
                }
            }

            private static void UpdateSpeed(PlayerFirstPersonController controller, float runSpeed, float walkSpeed)
            {
                controller.maxRunSpeed = runSpeed;
                controller.maxWalkSpeed = walkSpeed;
                //Debug.Log($"Updated walk speed to {walkSpeed}");
            }
        }
    }
}

