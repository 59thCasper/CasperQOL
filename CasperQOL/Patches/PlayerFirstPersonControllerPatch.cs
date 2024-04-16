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

            // Direct calculation of legHeight based on bodyCollider and transform
            private static float CalculateLegHeight(PlayerFirstPersonController controller)
            {
                CapsuleCollider collider = GetBodyCollider(controller);
                return collider.center.y * controller.transform.lossyScale.y;
            }

            static void Prefix(PlayerFirstPersonController __instance)
            {
                bool wasGrounded = __instance.m_IsGrounded;
                CapsuleCollider bodyCollider = GetBodyCollider(__instance);
                LayerMask groundLayer = GetGroundLayer(__instance);
                float legHeight = CalculateLegHeight(__instance);
                Vector3 position = __instance.transform.position;
                float groundSnapping = 0.2f;  // Adjusted for less sensitivity

                // Perform SphereCast to detect ground
                RaycastHit hit;
                Vector3 rayOrigin = position + Vector3.up * legHeight;
                if (Physics.SphereCast(rayOrigin, bodyCollider.radius, Vector3.down, out hit, legHeight + groundSnapping, groundLayer, QueryTriggerInteraction.Ignore))
                {
                    SetStandingOnLayer(__instance, hit.collider.gameObject.layer);
                    Vector3 groundContactPoint = hit.point;
                    float adjustHeight = legHeight - hit.distance;

                    __instance.m_IsGrounded = true; // Update grounded state

                    if (!wasGrounded && __instance.m_Rigidbody.velocity.y < -0.1f) // More specific velocity threshold
                    {
                        if (Mathf.Abs(adjustHeight) > 0.05f) // Only adjust if the change is significant
                        {
                            __instance.m_Rigidbody.transform.position += Vector3.up * adjustHeight;
                        }
                    }

                    if (hit.collider.gameObject.CompareTag("IInteractable"))
                    {
                        var interactableComponent = hit.collider.GetComponent<IInteractable>();
                        if (interactableComponent != null)
                        {
                            var resId = interactableComponent.myMachineRef.ResId;
                            var displayName = SaveState.GetResInfoFromId(resId).displayName;
                            SharedState.stoodOn = displayName;
                        }
                    }
                    else
                    {
                        SharedState.stoodOn = "";
                    }
                }
                else
                {
                    __instance.m_IsGrounded = false; // No ground detected
                    SharedState.stoodOn = "";
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
            }
        }
    }
}
