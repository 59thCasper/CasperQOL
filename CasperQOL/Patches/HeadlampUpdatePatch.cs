using HarmonyLib;
using UnityEngine;

namespace CasperQOL.Patches
{
    [HarmonyPatch(typeof(Headlamp))]
    [HarmonyPatch("Update")]
    public class HeadlampUpdatePatch
    {
        static bool previousToggleState = SharedState.lightToggle;

        static void Prefix(Headlamp __instance)
        {
            if (SharedState.lightToggle != previousToggleState)
            {
                __instance.ToggleLight();
                previousToggleState = SharedState.lightToggle;
            }
        }
    }
}
