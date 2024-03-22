using System;
using System.Collections.Generic;
using System.Text;
using NVIDIA;

namespace LC_Highlights
{
    internal class Highlight
    {
        private static DateTime _startTime;
        private static string _lastName;

        public static void LogCallback(Highlights.ReturnCode ret, int id)
        {
            Plugin.Log.LogDebug($"Callback from NVIDIA SDK with code {ret} for ID {id}.");
        }


        public static void SaveRecording(string name)
        {
            _lastName = name;

            if (!Plugin.cfgEnabled.Value)
                return;

            DateTime currentTime = DateTime.Now;
            DateTime timeBefore = currentTime.AddSeconds(-15);
            TimeSpan difference = DateTime.Now - timeBefore;

            Highlights.VideoHighlightParams vhp = new Highlights.VideoHighlightParams();
            vhp.highlightId = "MAP_PLAY";
            vhp.groupId = "MAP_PLAY_GROUP";
            vhp.startDelta = (int)-difference.TotalMilliseconds;
            vhp.endDelta = 0;

            vhp.startDelta += 0;
            vhp.endDelta += Plugin.cfgAfterDeathDelta.Value;

            Highlights.SetVideoHighlight(vhp, LogCallback);
        }

        public static void ShowSummary()
        {
            if (!Plugin.cfgEnabled.Value)
                return;

            Highlights.GroupView[] groupViews = new Highlights.GroupView[1];
            Highlights.GroupView gv1 = new Highlights.GroupView();
            gv1.GroupId = "MAP_PLAY_GROUP";
            gv1.SignificanceFilter = Highlights.HighlightSignificance.Good;
            gv1.TagFilter = Highlights.HighlightType.Achievement;
            groupViews[0] = gv1;

            Highlights.OpenSummary(groupViews, LogCallback);
        }
    }
}
