using System;
using Convert = System.Convert;
using File = System.IO.File;
using LCM.Other;
using IniFile = LCM.Other.IniFile;
using UnityEngine;
using System.Text.RegularExpressions;
using MelonLoader;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using CodeStage.AntiCheat.Storage;
using UnhollowerBaseLib;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using UnityEngine.Networking.Match;
using System.Reflection;
using static MelonLoader.MelonLogger;

namespace LCM.Essentials
{
    public static class Config
    {
        public static string mainPath = Environment.CurrentDirectory + @"\Mods\CM";
        public static string configPath = mainPath + @"\config.ini";
        public static string[,] keys = {
            { "EnableModLogs", "true" },
            { "EnableCustomServers", "true" },
        };
        public static bool logsEnabled, blacklistEnabled, 
        musicEnabled, securityEnabled, noFileSizeLimit, 
        noRichText, scoreboardEnabled, filtersEnabled, 
        postProcessingEnabled, customServersEnabled;
        public static KeyCode scoreboardKey, hideHudKey, proneKey;
        public static float fov;
        public static string nicknameColor = "";

        public static void SetUpConfig()
        {
            Directory.CreateDirectory(mainPath);
            var config = new IniFile(configPath);
            for (int i = 0; i < keys.GetLength(0); i++)
            {
                if (!config.KeyExists(keys[i, 0])) { config.Write(keys[i, 0], keys[i, 1]); }
            }
            logsEnabled = Convert.ToBoolean(config.Read("EnableModLogs"));
            blacklistEnabled = Convert.ToBoolean(config.Read("EnableBlackList"));
            musicEnabled = Convert.ToBoolean(config.Read("EnableMusicPlayer"));
            securityEnabled = Convert.ToBoolean(config.Read("EnableSecurityPatches"));
            noFileSizeLimit = Convert.ToBoolean(config.Read("NoFileSizeLimit"));
            nicknameColor = config.Read("ChatNickNameColor");
            noRichText = Convert.ToBoolean(config.Read("NoRichText"));
            scoreboardEnabled = Convert.ToBoolean(config.Read("EnableScoreboard"));
            fov = Convert.ToSingle(config.Read("FOV"));
            filtersEnabled = Convert.ToBoolean(config.Read("EnableInfectedFilters"));
            postProcessingEnabled = Convert.ToBoolean(config.Read("EnablePostProcessing"));
            customServersEnabled = Convert.ToBoolean(config.Read("EnableCustomServers"));
            if (!Enum.TryParse(config.Read("ScoreboardKeyCode"), out scoreboardKey)) { scoreboardKey = KeyCode.F1; }
            if (!Enum.TryParse(config.Read("HideHUDKeyCode"), out hideHudKey)) { hideHudKey = KeyCode.H; }
            if (!Enum.TryParse(config.Read("ProneKeyCode"), out proneKey)) { proneKey = KeyCode.C; }
        }
    }


    public static class ServerManager
    {
        public static Il2CppSystem.Collections.Generic.List<UpdaterV2.serverInfo> customServers = new Il2CppSystem.Collections.Generic.List<UpdaterV2.serverInfo>();
        public static Il2CppSystem.Collections.Generic.List<UpdaterV2.serverInfo> serverList
        {
            get { return GameObject.FindObjectOfType<UpdaterV2>().OJKJNHPHFFO; }
        }

        public static void AddCustomServers()
        {
            PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
            PhotonNetwork.player.name = ObscuredPrefs.GetString("ZWName0001", "Player " + UnityEngine.Random.Range(1, 999)) + "|2";
            serverList.Clear();
            foreach (UpdaterV2.serverInfo server in customServers)
            {
                serverList.Add(server);
            }
            CuteLogger.Meow("added servers");
        }
        public static void AddCustomServer(string serverName, string serverType, string serverIP, CloudRegionCode cloudRegion)
        {
            var server = new UpdaterV2.serverInfo() { cloudRegion = cloudRegion, serverType = serverType, serverName = serverName, serverIP = serverIP };
            serverList.Add(server);
            customServers.Add(server);
        }
        public static IEnumerator GetLCMServers()
        {
            string basePath = Application.dataPath;
            string modsPath = Path.Combine(basePath, "..", "Mods");
            string filePath = Path.Combine(modsPath, "CM", "cm.txt");
            if (!File.Exists(filePath))
            {
                CuteLogger.Bark("Error: server file not found...");
                yield break;
            }
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split('|');
                if (parts.Length < 3)
                {
                    CuteLogger.Bark("Error: invalid server entry format...");
                    continue;
                }
                CloudRegionCode r;
                if (!Enum.TryParse(parts[2], out r)) { r = CloudRegionCode.eu; }
                customServers.Add(new UpdaterV2.serverInfo()
                {
                    serverName = parts[0],
                    serverType = "APP",
                    serverIP = parts[1],
                    cloudRegion = r
                });
            }

            yield return null;
        }
    }

    
}
