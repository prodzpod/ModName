﻿using Hacknet;

namespace ZeroDayToolKit.Patches
{
    [HarmonyLib.HarmonyPatch(typeof(TrackerCompleteSequence), nameof(TrackerCompleteSequence.CompShouldStartTrackerFromLogs))] // stricter log control
    public class TrackerCheckLogs
    {
        static bool Prefix(object osobj, Computer c, string targetIP, ref bool __result)
        {
            OS os = (OS)osobj;
            Folder log = c.files.root.searchForFolder("log");
            if (targetIP == null) targetIP = os.thisComputer.ip;
            foreach (FileEntry file in log.files)
            {
                string data = file.data;
                if (data.Contains(targetIP) && !data.Contains("Connection") && !data.Contains("Disconnected"))
                {
                    __result = true;
                    return false;
                }
            }
            __result = false;
            return false; // skips redundant log check, will return true if i see mods that do postfixes
        }
    }
}
