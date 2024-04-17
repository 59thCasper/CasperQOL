using System;
using HarmonyLib;
using UnityEngine;


namespace CasperQOL.Patches
{
    [HarmonyPatch(typeof(PreBuiltMachine))]  
    [HarmonyPatch("BuildMe")]              
    public static class PreBuiltMachinePatch
    {
        // Prefix method to intercept the original BuildMe method
        static bool Prefix(PreBuiltMachine __instance, bool justUpdateProtectionStatus)
        {
            

            // Retrieve the type of the machine to be built
            MachineTypeEnum machineType = __instance.machineType.GetInstanceType();
            Debug.Log(machineType);

            // Check if the machine type is either ProductionTerminal or TransitDepot
            if (machineType != MachineTypeEnum.ProductionTerminal && machineType != MachineTypeEnum.TransitDepot)
            {

                Debug.LogError($"Attempt to build unsupported machine type {machineType} at {__instance.pos}. Only ProductionTerminal and TransitDepot are allowed.");


                return false;  
            }


            return true;
        }
    }
}
