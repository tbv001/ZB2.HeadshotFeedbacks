using HarmonyLib;
using UnityEngine;
using HeadshotFeedback.Components;

namespace HeadshotFeedback.Patches;

[HarmonyPatch(typeof(PlayerHUD))]
internal static class PlayerHudPatches
{
    public static bool IsCurrentHitmarkerHeadshot;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerHUD.HitMarker))]
    private static bool IsHeadshot(PlayerHUD __instance)
    {
        IsCurrentHitmarkerHeadshot = ShotPatches.WasHeadshot;

        var traverse = Traverse.Create(__instance);
        traverse.Field("hitMarker").SetValue(1f);

        if (IsCurrentHitmarkerHeadshot)
        {
            AudioLoader.PlaySfx();
        }
        else
        {
            AudioController.instance.PlayGlobalFX(AudioController.GlobalFXID.HitMarker);
        }

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch("UpdateHitmarkerDisplay")]
    private static void ColorItRed(PlayerHUD __instance)
    {
        if (!IsCurrentHitmarkerHeadshot) return;

        foreach (var image in __instance.hitMarkerLine)
        {
            if (!image.enabled)
            continue;

            var alpha = image.color.a;
            image.color = new Color(1f, 0f, 0f, alpha);
        }
    }
}
