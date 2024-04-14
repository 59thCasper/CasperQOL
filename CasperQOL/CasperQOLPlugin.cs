using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using CasperQOL.Patches;
using EquinoxsModUtils;

namespace CasperQOL
{
    public static class SharedState
    {
        public static string stoodOn = "";
        public static float CustomMaxRunSpeed { get; set; } = 14f;  // New max run speed when condition is met
        public static float CustomMaxWalkSpeed { get; set; } = 11f;
        public static float DefaultMaxRunSpeed { get; set; } = 8f;
        public static float DefaultMaxWalkSpeed { get; set; } = 5f;

        public static readonly HashSet<string> ValidResourceNames = new HashSet<string>
        {
            ResourceNames.CalycitePlatform1x1,
            ResourceNames.CalycitePlatform3x3,
            ResourceNames.CalycitePlatform5x5,
        };

    }

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class CasperQOL : BaseUnityPlugin
    {
        private const string MyGUID = "com.casper.CasperQOL";
        private const string PluginName = "CasperQOL";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);



        private void Awake()
        {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            ApplyPatches();

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;

        }



        private void ApplyPatches()
        {
            Harmony.CreateAndPatchAll(typeof(PlayerFirstPersonControllerPatch));
            Harmony.CreateAndPatchAll(typeof(PlayerInspectorLateUpdatePatch));
        }
    }
}
