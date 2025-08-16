using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_UpdateByBuilding : ThingComp
{
    private Texture2D Building_UpdateByBuilding_UpdateAP_Icon;

    private Texture2D Building_UpdateByBuilding_UpdateDamage_Icon;
    private Texture2D Building_UpdateByBuilding_UpdateEnergy_Icon;

    public CompProperties_UpdateByBuilding Props => props as CompProperties_UpdateByBuilding;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        yield return new Command_Action
        {
            action = DoSomething_UpdateEnergy,
            defaultLabel = "DrakkenLaserDrill_UpdateByBuilding_UpdateEnergy_Label".Translate(),
            icon = Building_UpdateByBuilding_UpdateEnergy_Icon,
            defaultDesc = "DrakkenLaserDrill_UpdateByBuilding_UpdateEnergy_Desc".Translate()
        };
        yield return new Command_Action
        {
            action = DoSomething_UpdateDamage,
            defaultLabel = "DrakkenLaserDrill_UpdateByBuilding_UpdateDamage_Label".Translate(),
            icon = Building_UpdateByBuilding_UpdateDamage_Icon,
            defaultDesc = "DrakkenLaserDrill_UpdateByBuilding_UpdateDamage_Desc".Translate()
        };
        yield return new Command_Action
        {
            action = DoSomething_UpdateAP,
            defaultLabel = "DrakkenLaserDrill_UpdateByBuilding_UpdateAP_Label".Translate(),
            icon = Building_UpdateByBuilding_UpdateAP_Icon,
            defaultDesc = "DrakkenLaserDrill_UpdateByBuilding_UpdateAP_Desc".Translate()
        };
    }

    private void DoSomething_UpdateEnergy()
    {
        BuildableDef mYDE_Building_DrakkenLaserDrill_UpdateEnergy =
            MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill_UpdateEnergy;
        var map = parent.Map;
        var list = new List<IntVec3>();
        var num = 3;
        for (var i = -num; i <= num; i++)
        {
            var item = parent.Position + new IntVec3(i, 0, -4);
            list.Add(item);
        }

        foreach (var item2 in list)
        {
            var thingList = item2.GetThingList(map);
            if (thingList.Count > 0)
            {
                continue;
            }

            GenSpawn.WipeExistingThings(item2, Rot4.North, mYDE_Building_DrakkenLaserDrill_UpdateEnergy, parent.Map,
                DestroyMode.Deconstruct);
            GenConstruct.PlaceBlueprintForBuild(mYDE_Building_DrakkenLaserDrill_UpdateEnergy,
                item2, map, Rot4.North, Faction.OfPlayer, null);
            break;
        }

        list.Clear();
    }

    private void DoSomething_UpdateDamage()
    {
        BuildableDef mYDE_Building_DrakkenLaserDrill_UpdateDamage =
            MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill_UpdateDamage;
        var map = parent.Map;
        var list = new List<IntVec3>();
        var num = 3;
        for (var i = -num; i <= num; i++)
        {
            var item = parent.Position + new IntVec3(i, 0, -4);
            list.Add(item);
        }

        foreach (var item2 in list)
        {
            var thingList = item2.GetThingList(map);
            if (thingList.Count > 0)
            {
                continue;
            }

            GenSpawn.WipeExistingThings(item2, Rot4.North, mYDE_Building_DrakkenLaserDrill_UpdateDamage, parent.Map,
                DestroyMode.Deconstruct);
            GenConstruct.PlaceBlueprintForBuild(mYDE_Building_DrakkenLaserDrill_UpdateDamage,
                item2, map, Rot4.North, Faction.OfPlayer, null);
            break;
        }

        list.Clear();
    }

    private void DoSomething_UpdateAP()
    {
        BuildableDef mYDE_Building_DrakkenLaserDrill_UpdateAP =
            MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill_UpdateAP;
        var map = parent.Map;
        var list = new List<IntVec3>();
        var num = 3;
        for (var i = -num; i <= num; i++)
        {
            var item = parent.Position + new IntVec3(i, 0, -4);
            list.Add(item);
        }

        foreach (var item2 in list)
        {
            var thingList = item2.GetThingList(map);
            if (thingList.Count > 0)
            {
                continue;
            }

            GenSpawn.WipeExistingThings(item2, Rot4.North, mYDE_Building_DrakkenLaserDrill_UpdateAP, parent.Map,
                DestroyMode.Deconstruct);
            GenConstruct.PlaceBlueprintForBuild(mYDE_Building_DrakkenLaserDrill_UpdateAP,
                item2, map, Rot4.North, Faction.OfPlayer, null);
            break;
        }

        list.Clear();
    }

    public override void CompTick()
    {
        Building_UpdateByBuilding_UpdateEnergy_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Energy");
        Building_UpdateByBuilding_UpdateDamage_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Damage");
        Building_UpdateByBuilding_UpdateAP_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/AP");
    }
}