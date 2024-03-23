using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using NVIDIA;
using BepInEx.Configuration;


namespace LC_Highlights
{
    [BepInPlugin(PluginMeta.PLUGIN_GUID, PluginMeta.PLUGIN_NAME, PluginMeta.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony thisHarmony;
        public static ManualLogSource Log;
        public static ConfigEntry<bool> cfgEnabled;
        public static ConfigEntry<int> cfgAfterDeathDelta;
        public static ConfigEntry<bool> cfgShowOverlayImmediately;
        private void Awake()
        {
            // configs
            cfgEnabled = Config.Bind("General", "Enable this plugin (Will take a video)", true, "Enables this mod to take videos using NVIDIA Highlights.");
            cfgAfterDeathDelta = Config.Bind("General", "Time after death (millis)", 2000, "This setting determines the length, in milliseconds, of the video segment captured after the player's character has died.");
            cfgShowOverlayImmediately = Config.Bind("General", "Show overlay immediately after death", true, "If the GeForce Experience overlay should be shown immediately after death.");
            Log = Logger;
            // Setup highlights SDK
            Highlights.HighlightScope[] reqScopes = new Highlights.HighlightScope[2]
            {
                Highlights.HighlightScope.Highlights,
                Highlights.HighlightScope.HighlightsRecordVideo,
            };

            var status = Highlights.CreateHighlightsSDK("LethalCompany Highlights", reqScopes);
            if (status != Highlights.ReturnCode.SUCCESS)
            {
                Log.LogError($"Failed to set up Highlights (Are you using NVIDIA GeForce?) - CODE: {status}");
                Highlights.UpdateLog();
                return;
            }

            Highlights.RequestPermissions(Highlight.LogCallback);

            Highlights.HighlightDefinition[] highlightDefinitions = new Highlights.HighlightDefinition[1];

            highlightDefinitions[0].Id = "MAP_PLAY";
            highlightDefinitions[0].HighlightTags = Highlights.HighlightType.Achievement;

            highlightDefinitions[0].Significance = Highlights.HighlightSignificance.Good;

            highlightDefinitions[0].UserDefaultInterest = true;
            highlightDefinitions[0].NameTranslationTable = new Highlights.TranslationEntry[]
            {
                new Highlights.TranslationEntry ("en-US", "Player Death"),
            };

            Highlights.ConfigureHighlights(highlightDefinitions, "en-US", Highlight.LogCallback);

            Highlights.OpenGroupParams ogp1 = new Highlights.OpenGroupParams();
            ogp1.Id = "MAP_PLAY_GROUP";
            ogp1.GroupDescriptionTable = new Highlights.TranslationEntry[]
            {
                new Highlights.TranslationEntry ("en-US", "Player Death group"),
            };

            Highlights.OpenGroup(ogp1, Highlight.LogCallback);
            Log.LogInfo("Success.");

            // Register a callback for player death I guess?
            thisHarmony = new Harmony(PluginMeta.PLUGIN_GUID);
            thisHarmony.PatchAll();

        }

    }
}