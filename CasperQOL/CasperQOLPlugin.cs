using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using CasperQOL.Patches;
using EquinoxsModUtils;
using UnityEngine;
using BepInEx.Configuration;

namespace CasperQOL
{
    public static class SharedState
    {
        public static string stoodOn = "";
        public static float CustomMaxRunSpeed { get; set; } = 11f;
        public static float CustomMaxWalkSpeed { get; set; } = 8f;
        public static float DefaultMaxRunSpeed { get; set; } = 8f;
        public static float DefaultMaxWalkSpeed { get; set; } = 5f;
        public static string lastZone = "";

        // V2 menu things :

        public static bool speedToggle = false;
        public static bool lightToggle = false;
        public static bool protectToggle = false;
        public static bool oreProtect = false;
        public static  ConfigEntry<KeyboardShortcut> toggleKey;

        // end of v2

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
        private const string VersionString = "1.0.4";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake()
        {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            CreateConfigEntries();
            ApplyPatches();

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;

        }

        void Update()
        {
            if (SharedState.toggleKey.Value.IsDown())
            {
                ShowGUI.shouldShow = !ShowGUI.shouldShow;
            }
        }


        void OnGUI()
        {
            ShowGUI.DrawGUI();
        }

        private void ApplyPatches()
        {
            Harmony.CreateAndPatchAll(typeof(PlayerFirstPersonControllerPatch));
            Harmony.CreateAndPatchAll(typeof(HeadlampUpdatePatch));
            Harmony.CreateAndPatchAll(typeof(VoxelModificationPatch));
            Harmony.CreateAndPatchAll(typeof(PreBuiltMachinePatch));
        }
        private void CreateConfigEntries()
        {
            var defaultKey = new KeyboardShortcut(KeyCode.KeypadMinus);  // Default key
            SharedState.toggleKey = Config.Bind("Controls", "GUI Key", defaultKey, "Key to toggle the GUI.");
        }

    }
}
