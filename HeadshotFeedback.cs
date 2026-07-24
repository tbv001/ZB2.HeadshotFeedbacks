using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HeadshotFeedback.Components;

namespace HeadshotFeedback;

[BepInPlugin(PluginGuid, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class HeadshotFeedback : BaseUnityPlugin
{
    internal const string PluginGuid = "com.theblackvoid.headshotfeedback";
    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(PluginGuid);
    public static ConfigEntry<bool> Use3DAudio;

    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            Configure();
            gameObject.AddComponent<AudioLoader>();
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Successfully loaded!");
        }
        catch (Exception e)
        {
            Logger.LogError($"Failed to load: {e}");
        }
    }

    private void Configure()
    {
        Use3DAudio = Config.Bind(new ConfigDefinition("General", "Use 3D Audio"), true,
            new ConfigDescription("Use 3D audio for hitmarker sounds"));
    }
}
