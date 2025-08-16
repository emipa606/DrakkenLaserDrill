using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_AutoAttack : ThingComp
{
    private Texture2D Building_DrakkenLaserDrill_AllAttack_Icon;

    private Texture2D Building_DrakkenLaserDrill_AutoAttack_Icon;

    private string Building_DrakkenLaserDrill_AutoAttack_Label;

    private int EnemyComeTick;

    private int EnemyComeTickMax = 600;

    private bool IfAutoSwitch = true;

    public bool IfEnemyCome;

    public CompProperties_DrakkenLaserDrill_AutoAttack Props => props as CompProperties_DrakkenLaserDrill_AutoAttack;

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref IfAutoSwitch, "IfAutoSwitch");
        Scribe_Values.Look(ref IfEnemyCome, "IfEnemyCome");
        Scribe_Values.Look(ref EnemyComeTick, "EnemyComeTick");
        Scribe_Values.Look(ref EnemyComeTickMax, "EnemyComeTickMax");
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (parent is Building_DrakkenLaserDrill { IfCrossMap: true })
        {
            yield break;
        }

        yield return new Command_Action
        {
            action = DoSomething_AttackAllPawn,
            defaultLabel = "DrakkenLaserDrill_AllAttack_Label".Translate(),
            icon = Building_DrakkenLaserDrill_AllAttack_Icon,
            defaultDesc = "DrakkenLaserDrill_AllAttack_Desc".Translate()
        };
        yield return new Command_Action
        {
            action = DoSomething_AutoSwitch,
            defaultLabel = Building_DrakkenLaserDrill_AutoAttack_Label,
            icon = Building_DrakkenLaserDrill_AutoAttack_Icon,
            defaultDesc = "DrakkenLaserDrill_AutoAttack_Desc".Translate()
        };
    }

    private void DoSomething_AttackAllPawn()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill != null &&
            (building_DrakkenLaserDrill.Now_Rebuilding || building_DrakkenLaserDrill.IfImmunity))
        {
            return;
        }

        var map = parent.Map;
        var ifAttackDown = building_DrakkenLaserDrill is { IfAttackDown: true };
        if (building_DrakkenLaserDrill != null)
        {
            building_DrakkenLaserDrill.DestroyAllBeacon();
            if (!building_DrakkenLaserDrill.Now_Rebuilding &&
                (building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon == null ||
                 building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon.Destroyed))
            {
                var allPawns = map.mapPawns.AllPawns;
                var list = new List<Thing>();
                foreach (var pawn in allPawns)
                {
                    if (pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer) &&
                        !pawn.IsPrisoner && (!pawn.Downed || ifAttackDown) &&
                        !pawn.Dead && !pawn.Destroyed)
                    {
                        list.Add(pawn);
                    }
                }

                if (list.Count > 0)
                {
                    var building_DrakkenLaserDrill_Beacon =
                        (Building_DrakkenLaserDrill_Beacon)ThingMaker.MakeThing(MYDE_ThingDefOf
                            .MYDE_Building_DrakkenLaserDrill_Beacon);
                    Thing thing = null;
                    var list2 = new List<Thing>();
                    foreach (var item in list)
                    {
                        if (!item.Position.Fogged(map) && item.Position.x > 0 && item.Position.x < map.Size.x &&
                            item.Position.z > 0 && item.Position.z < map.Size.z)
                        {
                            list2.Add(item);
                        }
                    }

                    if (list2.Count > 0)
                    {
                        var num = 500f;
                        foreach (var thing1 in list2)
                        {
                            var position = building_DrakkenLaserDrill.Position;
                            var position2 = thing1.Position;
                            var lengthHorizontal = (position - position2).LengthHorizontal;
                            if (!(lengthHorizontal < num))
                            {
                                continue;
                            }

                            num = lengthHorizontal;
                            thing = thing1;
                        }

                        if (thing != null)
                        {
                            var drawPos = thing.DrawPos;
                            var position3 = thing.Position;
                            var realPos = new Vector2(drawPos.x, drawPos.z);
                            ((Building_DrakkenLaserDrill_Beacon)GenSpawn.Spawn(building_DrakkenLaserDrill_Beacon,
                                position3,
                                map)).CheckSpawn(building_DrakkenLaserDrill, realPos, list, thing);
                        }

                        building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon =
                            building_DrakkenLaserDrill_Beacon;
                    }
                    else
                    {
                        Log.Message("Drakken Laser Drill Not Valid Target");
                    }
                }
            }
        }

        IfEnemyCome = false;
    }

    private void DoSomething_AutoSwitch()
    {
        if (IfAutoSwitch)
        {
            IfAutoSwitch = false;
        }
        else if (!IfAutoSwitch)
        {
            IfAutoSwitch = true;
        }
    }

    public void PrepareToAttack()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill != null && (building_DrakkenLaserDrill.IfCrossMap || !IfAutoSwitch))
        {
            return;
        }

        IfEnemyCome = true;
        var map = parent.Map;
        var ifAttackDown = building_DrakkenLaserDrill is { IfAttackDown: true };
        var allPawns = map.mapPawns.AllPawns;
        var list = new List<Thing>();
        foreach (var pawn in allPawns)
        {
            if (pawn.Faction == Faction.OfPlayer || !pawn.Faction.HostileTo(Faction.OfPlayer) ||
                pawn.IsPrisoner || pawn.Downed && !ifAttackDown ||
                pawn.Dead || pawn.Destroyed)
            {
                continue;
            }

            if (pawn.Position.x < 0 || pawn.Position.x > map.Size.x || pawn.Position.z < 0 ||
                pawn.Position.z > map.Size.z)
            {
                break;
            }

            list.Add(pawn);
        }

        if (list.Count > 0)
        {
            EnemyComeTick = EnemyComeTickMax;
        }
    }

    public override void CompTick()
    {
        Building_DrakkenLaserDrill_AllAttack_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/AllAttack");
        if (IfAutoSwitch)
        {
            Building_DrakkenLaserDrill_AutoAttack_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Auto");
            Building_DrakkenLaserDrill_AutoAttack_Label = "DrakkenLaserDrill_AutoAttack_True_Label".Translate();
        }
        else if (!IfAutoSwitch)
        {
            Building_DrakkenLaserDrill_AutoAttack_Icon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/False");
            Building_DrakkenLaserDrill_AutoAttack_Label = "DrakkenLaserDrill_AutoAttack_False_Label".Translate();
        }

        if (!IfEnemyCome)
        {
            return;
        }

        EnemyComeTick++;
        if (EnemyComeTick < EnemyComeTickMax)
        {
            return;
        }

        EnemyComeTick = 0;
        DoSomething_AttackAllPawn();
        IfEnemyCome = false;
    }
}