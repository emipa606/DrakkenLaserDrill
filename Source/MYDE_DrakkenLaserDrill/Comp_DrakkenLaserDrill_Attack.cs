using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_Attack : ThingComp
{
    private Texture2D Building_DrakkenLaserDrill_Icon;

    private IntVec3 FirstPos;

    public CompProperties_DrakkenLaserDrill_Attack Props => props as CompProperties_DrakkenLaserDrill_Attack;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (parent is not Building_DrakkenLaserDrill { IfCrossMap: true })
        {
            yield return new Command_Action
            {
                action = DoSomething_I,
                defaultLabel = "DrakkenLaserDrill_Attack_Label".Translate(),
                icon = Building_DrakkenLaserDrill_Icon,
                defaultDesc = "DrakkenLaserDrill_Attack_Desc".Translate()
            };
        }
    }

    private void DoSomething_I()
    {
        var Map = parent.Map;
        var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (Building_DrakkenLaserDrill != null &&
            (Building_DrakkenLaserDrill.Now_Rebuilding || Building_DrakkenLaserDrill.IfImmunity))
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
            Building_DrakkenLaserDrill?.DestroyAllBeacon();
            FirstPos = Target.Cell;
            DoSomething_II();
        }, null, null);
    }

    private void DoSomething_II()
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
        Find.Targeter.BeginTargeting(targetingParameters, delegate(LocalTargetInfo Target)
        {
            var ifAttackDown = Building_DrakkenLaserDrill is { IfAttackDown: true };
            var list = new List<IntVec3>();
            var list2 = new List<Thing>();
            var list3 = new List<Thing>();
            var num = Math.Abs(Target.Cell.x - FirstPos.x);
            var num2 = Math.Abs(Target.Cell.z - FirstPos.z);
            if (Target.Cell.x >= FirstPos.x && Target.Cell.z >= FirstPos.z)
            {
                for (var i = 0; i <= num; i++)
                {
                    for (var j = 0; j <= num2; j++)
                    {
                        var item = FirstPos + new IntVec3(i, 0, j);
                        list.Add(item);
                    }
                }
            }
            else if (Target.Cell.x < FirstPos.x && Target.Cell.z >= FirstPos.z)
            {
                for (var k = -num; k <= 0; k++)
                {
                    for (var l = 0; l <= num2; l++)
                    {
                        var item2 = FirstPos + new IntVec3(k, 0, l);
                        list.Add(item2);
                    }
                }
            }
            else if (Target.Cell.x < FirstPos.x && Target.Cell.z < FirstPos.z)
            {
                for (var m = -num; m <= 0; m++)
                {
                    for (var n = -num2; n <= 0; n++)
                    {
                        var item3 = FirstPos + new IntVec3(m, 0, n);
                        list.Add(item3);
                    }
                }
            }
            else if (Target.Cell.x >= FirstPos.x && Target.Cell.z < FirstPos.z)
            {
                for (var num3 = 0; num3 <= num; num3++)
                {
                    for (var num4 = -num2; num4 <= 0; num4++)
                    {
                        var item4 = FirstPos + new IntVec3(num3, 0, num4);
                        list.Add(item4);
                    }
                }
            }

            for (var num5 = 0; num5 < list.Count; num5++)
            {
                list2.AddRange(list[num5].GetThingList(Map));
            }

            foreach (var thing in list2)
            {
                if (thing is Building)
                {
                    if (thing.Faction != Faction.OfPlayer && thing.def.useHitPoints)
                    {
                        list3.Add(thing);
                    }
                }
                else if (thing is Pawn pawn && pawn.Faction != Faction.OfPlayer &&
                         !pawn.IsPrisoner && (!pawn.Downed || ifAttackDown) && pawn is
                         {
                             Dead: false, Destroyed: false
                         })
                {
                    list3.Add(pawn);
                }
            }

            if (Building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon != null)
            {
                var thing = list3.RandomElement();
                foreach (var item5 in list3)
                {
                    if (!item5.Position.Fogged(Map) && item5 is Pawn)
                    {
                        thing = item5;
                    }
                }

                var drawPos = thing.DrawPos;
                var realPos = new Vector2(drawPos.x, drawPos.z);
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon.RealPos = realPos;
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon.Position = Target.Cell;
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon.ListThing = list3;
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon.TargetThing = thing;
            }
            else if (Building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon == null ||
                     Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon.Destroyed)
            {
                var building_DrakkenLaserDrill_Beacon =
                    (Building_DrakkenLaserDrill_Beacon)ThingMaker.MakeThing(MYDE_ThingDefOf
                        .MYDE_Building_DrakkenLaserDrill_Beacon);
                var thing2 = list3.RandomElement();
                foreach (var item6 in list3)
                {
                    if (!item6.Position.Fogged(Map) && item6 is Pawn)
                    {
                        thing2 = item6;
                    }
                }

                var drawPos2 = thing2.DrawPos;
                var position2 = thing2.Position;
                var realPos2 = new Vector2(drawPos2.x, drawPos2.z);
                ((Building_DrakkenLaserDrill_Beacon)GenSpawn.Spawn(building_DrakkenLaserDrill_Beacon, position2, Map))
                    .CheckSpawn(Building_DrakkenLaserDrill, realPos2, list3, thing2);
                if (Building_DrakkenLaserDrill != null)
                {
                    Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon = building_DrakkenLaserDrill_Beacon;
                }
            }
        }, delegate(LocalTargetInfo Target)
        {
            var list = new List<IntVec3>();
            var num = Math.Abs(Target.Cell.x - FirstPos.x);
            var num2 = Math.Abs(Target.Cell.z - FirstPos.z);
            if (Target.Cell.x >= FirstPos.x && Target.Cell.z >= FirstPos.z)
            {
                for (var i = 0; i <= num; i++)
                {
                    for (var j = 0; j <= num2; j++)
                    {
                        var item = FirstPos + new IntVec3(i, 0, j);
                        list.Add(item);
                    }
                }
            }
            else if (Target.Cell.x < FirstPos.x && Target.Cell.z >= FirstPos.z)
            {
                for (var k = -num; k <= 0; k++)
                {
                    for (var l = 0; l <= num2; l++)
                    {
                        var item2 = FirstPos + new IntVec3(k, 0, l);
                        list.Add(item2);
                    }
                }
            }
            else if (Target.Cell.x < FirstPos.x && Target.Cell.z < FirstPos.z)
            {
                for (var m = -num; m <= 0; m++)
                {
                    for (var n = -num2; n <= 0; n++)
                    {
                        var item3 = FirstPos + new IntVec3(m, 0, n);
                        list.Add(item3);
                    }
                }
            }
            else if (Target.Cell.x >= FirstPos.x && Target.Cell.z < FirstPos.z)
            {
                for (var num3 = 0; num3 <= num; num3++)
                {
                    for (var num4 = -num2; num4 <= 0; num4++)
                    {
                        var item4 = FirstPos + new IntVec3(num3, 0, num4);
                        list.Add(item4);
                    }
                }
            }

            GenDraw.DrawFieldEdges(list, Color.white);
        }, null);
    }

    public override void CompTick()
    {
        Building_DrakkenLaserDrill_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Beacon");
    }
}