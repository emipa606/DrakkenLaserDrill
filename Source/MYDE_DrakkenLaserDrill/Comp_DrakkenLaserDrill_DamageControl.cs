using System.Collections.Generic;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_DamageControl : ThingComp
{
    public CompProperties_DrakkenLaserDrill_DamageControl Props =>
        props as CompProperties_DrakkenLaserDrill_DamageControl;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        yield return new Gizmo_DrakkenLaserDrill_DamageControl
        {
            Comp = this
        };
    }
}