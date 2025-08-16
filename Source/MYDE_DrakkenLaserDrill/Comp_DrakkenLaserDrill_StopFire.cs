using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

public class Comp_DrakkenLaserDrill_StopFire : ThingComp
{
    private Texture2D DrakkenLaserDrill_StopFire_Icon;

    public CompProperties_DrakkenLaserDrill_StopFire Props => props as CompProperties_DrakkenLaserDrill_StopFire;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        yield return new Command_Action
        {
            action = DoSomething,
            defaultLabel = "DrakkenLaserDrill_StopFire_Label".Translate(),
            defaultDesc = "DrakkenLaserDrill_StopFire_Desc".Translate(),
            icon = DrakkenLaserDrill_StopFire_Icon
        };
    }

    private void DoSomething()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        building_DrakkenLaserDrill?.DestroyAllBeacon();
    }

    public override void CompTick()
    {
        base.CompTick();
        DrakkenLaserDrill_StopFire_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Destroy");
    }
}