using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_MouseAttack : ThingComp
{
    private Texture2D Building_DrakkenLaserDrill_MouseAttack_Icon;

    public CompProperties_DrakkenLaserDrill_MouseAttack Props => props as CompProperties_DrakkenLaserDrill_MouseAttack;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (parent is Building_DrakkenLaserDrill { IfCrossMap: false })
        {
            yield return new Command_Action
            {
                action = DoSomething,
                defaultLabel = "DrakkenLaserDrill_MouseAttack_Label".Translate(),
                icon = Building_DrakkenLaserDrill_MouseAttack_Icon,
                defaultDesc = "DrakkenLaserDrill_MouseAttack_Desc".Translate()
            };
        }
    }

    private void DoSomething()
    {
        var Map = parent.Map;
        var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        Building_DrakkenLaserDrill?.DestroyAllBeacon();
        if (Building_DrakkenLaserDrill is { Now_Rebuilding: true } ||
            Building_DrakkenLaserDrill is { IfImmunity: true } ||
            Building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon_Mouse is { Destroyed: false })
        {
            return;
        }

        var targetingParameters = new TargetingParameters
        {
            canTargetLocations = true,
            validator = target => target.IsValid && target.Cell.InBounds(Map)
        };
        Find.Targeter.BeginTargeting(targetingParameters, delegate(LocalTargetInfo Target)
        {
            var centerVector = Target.CenterVector3;
            var cell = Target.Cell;
            var realPos = new Vector2(Target.CenterVector3.x, Target.CenterVector3.z);
            var building_DrakkenLaserDrill_Beacon_Mouse =
                (Building_DrakkenLaserDrill_Beacon_Mouse)ThingMaker.MakeThing(MYDE_ThingDefOf
                    .MYDE_Building_DrakkenLaserDrill_Beacon_Mouse);
            ((Building_DrakkenLaserDrill_Beacon_Mouse)GenSpawn.Spawn(building_DrakkenLaserDrill_Beacon_Mouse, cell,
                Map)).CheckSpawn(Building_DrakkenLaserDrill, realPos, centerVector);
            if (Building_DrakkenLaserDrill != null)
            {
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse =
                    building_DrakkenLaserDrill_Beacon_Mouse;
            }

            DoSomething_Move();
        }, null, null);
    }

    private void DoSomething_Move()
    {
        var Map = parent.Map;
        var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (Building_DrakkenLaserDrill is { Now_Rebuilding: true })
        {
            return;
        }

        var targetingParameters = new TargetingParameters
        {
            canTargetLocations = true,
            validator = target => target.IsValid && target.Cell.InBounds(Map)
        };
        Find.Targeter.BeginTargeting(targetingParameters, delegate
        {
            if (Building_DrakkenLaserDrill is not { Building_DrakkenLaserDrill_Beacon_Mouse: not null })
            {
                return;
            }

            Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse.Destroy();
            Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse = null;
            Building_DrakkenLaserDrill.DestroyAllBeacon();
        }, delegate(LocalTargetInfo Target)
        {
            if (Building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon_Mouse == null)
            {
                return;
            }

            var centerVector = Target.CenterVector3;
            Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse.TargetPos = centerVector;
            var pos_Square =
                MYDE_ModFront.GetPos_Square(
                    Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse.Position, 1, 1);
            GenDraw.DrawFieldEdges(pos_Square, Color.white);
        }, null);
    }

    public override void CompTick()
    {
        Building_DrakkenLaserDrill_MouseAttack_Icon =
            ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/MouseAttack");
    }
}