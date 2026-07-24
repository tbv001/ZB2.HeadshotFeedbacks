using HarmonyLib;
using UnityEngine;

namespace HeadshotFeedback.Patches;

[HarmonyPatch(typeof(Shot))]
internal static class ShotPatches
{
    public static bool WasHeadshot;
    public static Vector3 LastHeadshotPosition;

    [HarmonyPrefix]
    [HarmonyPatch("HitDamageTaker")]
    private static void CheckIfHeadshot(Vector3 hitpoint, IDamageTaker damageTaker, out bool __state)
    {
        __state = damageTaker.DamageMultiplier > 1f;
        if (__state)
        {
            WasHeadshot = true;
            LastHeadshotPosition = hitpoint;
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
