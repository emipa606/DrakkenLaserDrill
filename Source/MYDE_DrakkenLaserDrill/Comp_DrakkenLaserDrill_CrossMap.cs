using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_CrossMap : ThingComp
{
    private Texture2D Building_DrakkenLaserDrill_CrossMap_Icon;

    private string Building_DrakkenLaserDrill_CrossMap_Label;

    public CompProperties_DrakkenLaserDrill_CrossMap Props => props as CompProperties_DrakkenLaserDrill_CrossMap;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        yield return new Command_Action
        {
            action = DoSomething_CrossMapSwitch,
            defaultLabel = Building_DrakkenLaserDrill_CrossMap_Label,
            icon = Building_DrakkenLaserDrill_CrossMap_Icon,
            defaultDesc = "DrakkenLaserDrill_CrossMap_Desc".Translate()
        };
    }

    private void DoSomething_CrossMapSwitch()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill is { IfCrossMap: true })
        {
            building_DrakkenLaserDrill.IfCrossMap = false;
        }
        else if (building_DrakkenLaserDrill is not { IfCrossMap: true })
        {
            if (building_DrakkenLaserDrill != null)
            {
                building_DrakkenLaserDrill.IfCrossMap = true;
            }
        }
    }

    public override void CompTick()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill is { IfCrossMap: true })
        {
            Building_DrakkenLaserDrill_CrossMap_Icon =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/CrossMap_True");
            Building_DrakkenLaserDrill_CrossMap_Label = "DrakkenLaserDrill_CrossMap_True_Label".Translate();
        }
        else if (building_DrakkenLaserDrill is { IfCrossMap: false })
        {
            Building_DrakkenLaserDrill_CrossMap_Icon =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/CrossMap_False");
            Building_DrakkenLaserDrill_CrossMap_Label = "DrakkenLaserDrill_CrossMap_False_Label".Translate();
        }
    }
}