using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using EquinoxsModUtils;

namespace CasperQOL.Patches
{
    internal static class PlayerFirstPersonControllerPatch
    {
        [HarmonyPatch(typeof(PlayerFirstPersonController), "FixedUpdate")]
        public static class SpeedModifierPatch
        {
            private static CapsuleCollider GetBodyCollider(PlayerFirstPersonController controller)
            {
                return AccessTools.FieldRefAccess<PlayerFirstPersonController, CapsuleCollider>(controller, "bodyCollider");
            }

            private static LayerMask GetGroundLayer(PlayerFirstPersonController controller)
            {
                return AccessTools.FieldRefAccess<PlayerFirstPersonController, LayerMask>(controller, "groundLayer");
            }

            private static int GetStandingOnLayer(PlayerFirstPersonController controller)
            {
                return AccessTools.FieldRefAccess<PlayerFirstPersonController, int>(controller, "standingOnLayer");
            }

            private static void SetStandingOnLayer(PlayerFirstPersonController controller, int layer)
            {
                AccessTools.FieldRefAccess<PlayerFirstPersonController, int>(controller, "standingOnLayer") = layer;
            }

            private static float CalculateLegHeight(PlayerFirstPersonController controller)
            {
                CapsuleCollider collider = GetBodyCollider(controller);
                return collider.bounds.extents.y;
            }

            static void Prefix(PlayerFirstPersonController __instance)
            {
                CapsuleCollider bodyCollider = GetBodyCollider(__instance);
                LayerMask groundLayer = GetGroundLayer(__instance);
                float legHeight = CalculateLegHeight(__instance);
                Vector3 position = __instance.transform.position;
                float groundSnapping = 0.5f;

                Vector3 rayOrigin = position + Vector3.up * (legHeight + 0.1f);
                float castDistance = legHeight + groundSnapping;

                //Debug.Log("Casting SphereCast from: " + rayOrigin + " downwards for a distance of: " + castDistance);

                RaycastHit hit;
                if (Physics.SphereCast(rayOrigin, bodyCollider.radius, Vector3.down, out hit, castDistance, groundLayer, QueryTriggerInteraction.Ignore))
                {
                    SetStandingOnLayer(__instance, hit.collider.gameObject.layer);
                    //Debug.Log("Hit: " + hit.collider.name + " at Layer: " + hit.collider.gameObject.layer);

                    if (hit.collider.gameObject.CompareTag("IInteractable"))
                    {
                        var interactableComponent = hit.collider.GetComponent<IInteractable>();
                        if (interactableComponent != null)
                        {
                            var resId = interactableComponent.myMachineRef.ResId;
                            var displayName = SaveState.GetResInfoFromId(resId).displayName;
                            SharedState.stoodOn = displayName;
                            //Debug.Log("Standing on Interactable: " + displayName);
                        }
                    }
                }
                else
                {
                    SharedState.stoodOn = "";
                    //Debug.Log("No ground detected.");
                }

                if (__instance.m_IsGrounded && SharedState.ValidResourceNames.Contains(SharedState.stoodOn) && SharedState.speedToggle)
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
                //Debug.Log("Updated Speeds to Run: " + runSpeed + ", Walk: " + walkSpeed);
            }
        }
    }
}
