using HarmonyLib;
using RimWorld;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[HarmonyPatch(typeof(IncidentWorker_RaidEnemy), "TryExecuteWorker")]
internal class IncidentWorker_RaidEnemy_TryExecuteWorker
{
    private static void Postfix()
    {
        var list = Find.CurrentMap.listerThings.ThingsOfDef(MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill);
        if (list.Count <= 0)
        {
            return;
        }

        foreach (var thing in list)
        {
            var drill = (Building_DrakkenLaserDrill)thing;
            drill.TryGetComp<Comp_DrakkenLaserDrill_AutoAttack>().PrepareToAttack();
        }
    }
}