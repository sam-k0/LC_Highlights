using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using GameNetcodeStuff;


namespace LC_Highlights
{

    public static class Convert
    {
        public static string GetCauseOfDeathString(int causeOfDeathValue)
        {
            switch ((CauseOfDeath)causeOfDeathValue)
            {
                case CauseOfDeath.Unknown:
                    return "Unknown";
                case CauseOfDeath.Bludgeoning:
                    return "Bludgeoning";
                case CauseOfDeath.Gravity:
                    return "Gravity";
                case CauseOfDeath.Blast:
                    return "Blast";
                case CauseOfDeath.Strangulation:
                    return "Strangulation";
                case CauseOfDeath.Suffocation:
                    return "Suffocation";
                case CauseOfDeath.Mauling:
                    return "Mauling";
                case CauseOfDeath.Gunshots:
                    return "Gunshots";
                case CauseOfDeath.Crushing:
                    return "Crushing";
                case CauseOfDeath.Drowning:
                    return "Drowning";
                case CauseOfDeath.Abandoned:
                    return "Abandoned";
                case CauseOfDeath.Electrocution:
                    return "Electrocution";
                case CauseOfDeath.Kicking:
                    return "Kicking";
                default:
                    return "Unknown";
            }
        }
    }


    // Catches player killed event
    [HarmonyPatch(typeof(PlayerControllerB))]
    [HarmonyPatch("KillPlayer")]
    class PlayerKillPatch
    {
        public static void Prefix(PlayerControllerB __instance, ref CauseOfDeath causeOfDeath)
        {
            string hname = DateTime.Now.ToString("yyyyMMdd_HHmmss")+ "_"+ Convert.GetCauseOfDeathString((int)causeOfDeath);
            Highlight.SaveRecording(hname);
            Plugin.Log.LogMessage($"Saved recording {hname} because player died!");
            if(Plugin.cfgShowOverlayImmediately.Value)
            {
                Highlight.ShowSummary();
            }
            
        }
    }

}
