using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HeadshotFeedbacks.Components;

namespace HeadshotFeedbacks;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class HeadshotFeedbacks : BaseUnityPlugin
{
    public const string PluginName = "Headshot Feedbacks";
    public const string PluginVersion = "1.0.0";
    public const string PluginGuid = "com.theblackvoid.headshotfeedbacks";
    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(PluginGuid);

    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            gameObject.AddComponent<AudioLoader>();
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Successfully loaded and patched!");
        }
        catch (Exception e)
        {
            Logger.LogError($"Failed to load: {e}");
        }
    }
}
