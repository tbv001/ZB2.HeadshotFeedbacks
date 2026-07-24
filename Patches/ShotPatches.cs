using HarmonyLib;

namespace HeadshotFeedback.Patches;

[HarmonyPatch(typeof(Shot))]
internal static class ShotPatches
{
    public static bool WasHeadshot;

    [HarmonyPrefix]
    [HarmonyPatch("HitDamageTaker")]
    private static void CheckIfHeadshot(IDamageTaker damageTaker, out bool __state)
    {
        __state = damageTaker.DamageMultiplier > 1f;
        if (__state)
        {
            WasHeadshot = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("HitDamageTaker")]
    private static void RevertWasHeadshot(bool __state)
    {
        if (__state)
        {
            WasHeadshot = false;
        }
    }
}
