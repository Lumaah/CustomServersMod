
using MelonLoader;
using LCM.Essentials;
using UnityEngine;
using System.Collections;
using LCM.Other;
using UnhollowerRuntimeLib;

[assembly: MelonInfo(typeof(LCM.LCM), "LumaahCustomServers", "1.0", "Lumaah")]
[assembly: MelonGame("ZeoWorks", "Slendytubbies 3")]

namespace LCM
{
    public struct AppInfo
    {
        public string Name;
        public string Version;
        public bool UpdateAvailable;
        public string DiscordLink;
    }

    public class LCM : MelonMod
    {
        public static AppInfo AppInfo;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "MainMenu")
            {
                ServerManager.AddCustomServers();
            }
        }

        public override void OnApplicationStart()
        {
            AppInfo.Name = Info.Name;
            AppInfo.Version = Info.Version;
        }

        public override void OnApplicationLateStart()
        {
            MelonCoroutines.Start(ServerManager.GetLCMServers());
            CuteLogger.ClearLogs();
        }

    }
}
