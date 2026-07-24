using System;
using System.Reflection;
using BepInEx;
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

    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            gameObject.AddComponent<AudioLoader>();
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Successfully loaded!");
        }
        catch (Exception e)
        {
            Logger.LogError($"Failed to load: {e}");
        }
    }
}
