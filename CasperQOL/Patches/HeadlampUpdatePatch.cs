using HarmonyLib;
using UnityEngine;

namespace CasperQOL.Patches
{
    [HarmonyPatch(typeof(Headlamp))]
    [HarmonyPatch("Update")]
    public class HeadlampUpdatePatch
    {
        static bool previousToggleState = SharedState.lightToggle;  // Track the previous state to detect changes

        static void Prefix(Headlamp __instance)
        {
            if (SharedState.lightToggle != previousToggleState)
            {
                __instance.ToggleLight();  // Toggle the light when the state changes
                previousToggleState = SharedState.lightToggle;  // Update the previous state
                //Debug.Log("Light Toggle changed via GUI: " + SharedState.lightToggle);
            }
        }
    }
}
