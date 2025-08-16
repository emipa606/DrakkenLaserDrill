using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

public class Comp_DrakkenLaserDrill_Attack_CrossMap : ThingComp
{
    private readonly float MinLaser_Alpha = 0.6f;

    private readonly float MinLaser_Width = 0.3f;

    private readonly float MinLaserPos_A_Range_Limit = 0.4f;

    private readonly float MinLaserPos_B_Range_Limit = 0.4f;

    private readonly float MinLaserPos_C_Range_Limit = 0.4f;

    private readonly float MinLaserPos_D_Range_Limit = 0.4f;

    private readonly float MinLaserPos_E_Range_Limit = 0.4f;

    private readonly float MinLaserPos_F_Range_Limit = 0.4f;
    private Texture2D Building_DrakkenLaserDrill_Icon_AllAttack_CrossMap;
    private Texture2D Building_DrakkenLaserDrill_Icon_CrossMap;

    private IntVec3 FirstPos;

    private Vector3 MinLaserPos_A_End;

    public float MinLaserPos_A_Range = 0.5f;

    private Vector3 MinLaserPos_A_Start;

    public bool MinLaserPos_A_UpOrDown = true;

    private Vector3 MinLaserPos_B_End;

    public float MinLaserPos_B_Range = -0.5f;

    private Vector3 MinLaserPos_B_Start;

    public bool MinLaserPos_B_UpOrDown;

    private Vector3 MinLaserPos_C_End;

    public float MinLaserPos_C_Range = 0.2f;

    private Vector3 MinLaserPos_C_Start;

    public bool MinLaserPos_C_UpOrDown = true;

    private Vector3 MinLaserPos_D_End;

    public float MinLaserPos_D_Range = -0.2f;

    private Vector3 MinLaserPos_D_Start;

    public bool MinLaserPos_D_UpOrDown;

    private Vector3 MinLaserPos_E_End;

    public float MinLaserPos_E_Range = -0.1f;

    private Vector3 MinLaserPos_E_Start;

    public bool MinLaserPos_E_UpOrDown = true;

    private Vector3 MinLaserPos_F_End;

    public float MinLaserPos_F_Range = 0.1f;

    private Vector3 MinLaserPos_F_Start;

    public bool MinLaserPos_F_UpOrDown;

    public CompProperties_DrakkenLaserDrill_Attack_CrossMap Props =>
        props as CompProperties_DrakkenLaserDrill_Attack_CrossMap;

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref MinLaserPos_A_Range, "MinLaserPos_A_Range");
        Scribe_Values.Look(ref MinLaserPos_A_UpOrDown, "MinLaserPos_A_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_B_Range, "MinLaserPos_B_Range");
        Scribe_Values.Look(ref MinLaserPos_B_UpOrDown, "MinLaserPos_B_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_C_Range, "MinLaserPos_C_Range");
        Scribe_Values.Look(ref MinLaserPos_C_UpOrDown, "MinLaserPos_C_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_D_Range, "MinLaserPos_D_Range");
        Scribe_Values.Look(ref MinLaserPos_D_UpOrDown, "MinLaserPos_D_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_E_Range, "MinLaserPos_E_Range");
        Scribe_Values.Look(ref MinLaserPos_E_UpOrDown, "MinLaserPos_E_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_F_Range, "MinLaserPos_F_Range");
        Scribe_Values.Look(ref MinLaserPos_F_UpOrDown, "MinLaserPos_F_UpOrDown");
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (parent is not Building_DrakkenLaserDrill { IfCrossMap: true })
        {
            yield break;
        }

        yield return new Command_Action
        {
            action = DoSomething_I,
            defaultLabel = "DrakkenLaserDrill_Attack_Label".Translate(),
            icon = Building_DrakkenLaserDrill_Icon_CrossMap,
            defaultDesc = "DrakkenLaserDrill_Attack_Desc".Translate()
        };
        yield return new Command_Action
        {
            action = DoSomething_I_AllAttack,
            defaultLabel = "DrakkenLaserDrill_AllAttack_Label".Translate(),
            icon = Building_DrakkenLaserDrill_Icon_AllAttack_CrossMap,
            defaultDesc = "DrakkenLaserDrill_AllAttack_Desc".Translate()
        };
    }

    private void DoSomething_I()
    {
        CameraJumper.TryJump(CameraJumper.GetWorldTarget(parent));
        Find.WorldSelector.ClearSelection();
        var Map = parent.Map;
        var MaxRange = 66;
        Find.WorldTargeter.BeginTargeting((Func<GlobalTargetInfo, bool>)ChoseWorldTarget, true, null, true,
            delegate { GenDraw.DrawWorldRadiusRing(Map.Parent.Tile, MaxRange); },
            target => ShowMaxRange(target, Map.Parent.Tile, MaxRange));
    }

    private bool ChoseWorldTarget(GlobalTargetInfo Target)
    {
        var map = Current.Game.FindMap(Target.WorldObject.Tile);
        var num = Find.WorldGrid.TraversalDistanceBetween(parent.Map.Parent.Tile, Target.Tile);
        var num2 = 66;
        if (num > num2 && !MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange)
        {
            return true;
        }

        if (map == null)
        {
            Log.Error("DrakkenLaserDrill_CrossMap_Error".Translate());
            return false;
        }

        DoSomething_II(map, Target.WorldObject.Tile);

        return true;
    }

    private void DoSomething_II(Map TargetMap, int TargetTileNum)
    {
        Current.Game.CurrentMap = TargetMap;
        var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (Building_DrakkenLaserDrill is { Now_Rebuilding: true } or { IfImmunity: true })
        {
            return;
        }

        var targetingParameters = new TargetingParameters
        {
            canTargetLocations = true,
            validator = target => target.IsValid && target.Cell.InBounds(TargetMap)
        };
        Find.Targeter.BeginTargeting(targetingParameters, delegate(LocalTargetInfo Target)
        {
            Building_DrakkenLaserDrill?.DestroyAllBeacon();
            FirstPos = Target.Cell;
            DoSomething_III(TargetMap, TargetTileNum);
        }, null, null);
    }

    private void DoSomething_III(Map TargetMap, int TargetTileNum)
    {
        var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (Building_DrakkenLaserDrill is { Now_Rebuilding: true })
        {
            return;
        }

        var targetingParameters = new TargetingParameters
        {
            canTargetLocations = true,
            validator = target => target.IsValid && target.Cell.InBounds(TargetMap)
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
                list2.AddRange(list[num5].GetThingList(TargetMap));
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

            if (Building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon_CrossMap is { Destroyed: false })
            {
                return;
            }

            {
                var building_DrakkenLaserDrill_Beacon_CrossMap =
                    (Building_DrakkenLaserDrill_Beacon_CrossMap)ThingMaker.MakeThing(MYDE_ThingDefOf
                        .MYDE_Building_DrakkenLaserDrill_Beacon_CrossMap);
                var thing = list3.RandomElement();
                foreach (var item5 in list3)
                {
                    if (!item5.Position.Fogged(TargetMap) && item5 is Pawn)
                    {
                        thing = item5;
                    }
                }

                var drawPos = thing.DrawPos;
                var position = thing.Position;
                var realPos = new Vector2(drawPos.x, drawPos.z);
                var headingFromTo = Find.WorldGrid.GetHeadingFromTo(parent.Map.Tile, TargetTileNum);
                var num7 = headingFromTo - 180f;
                if (num7 < 0f)
                {
                    num7 += 360f;
                }

                var num8 = Find.WorldGrid.TraversalDistanceBetween(parent.Map.Tile, TargetTileNum);
                float num9 = num8 * parent.Map.Size.x;
                var vector3_By_AngleFlat =
                    MYDE_ModFront.GetVector3_By_AngleFlat(TargetMap.Center.ToVector3(), num9, headingFromTo);
                ((Building_DrakkenLaserDrill_Beacon_CrossMap)GenSpawn.Spawn(building_DrakkenLaserDrill_Beacon_CrossMap,
                    position, TargetMap)).CheckSpawn(Building_DrakkenLaserDrill, realPos, list3, thing, num7,
                    vector3_By_AngleFlat, num9);
                if (Building_DrakkenLaserDrill != null)
                {
                    Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap =
                        building_DrakkenLaserDrill_Beacon_CrossMap;
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

    private void DoSomething_I_AllAttack()
    {
        CameraJumper.TryJump(CameraJumper.GetWorldTarget(parent));
        Find.WorldSelector.ClearSelection();
        var Map = parent.Map;
        var MaxRange = 66;
        Find.WorldTargeter.BeginTargeting((Func<GlobalTargetInfo, bool>)ChoseWorldTarget_AllAttack, true, null, true,
            delegate { GenDraw.DrawWorldRadiusRing(Map.Parent.Tile, MaxRange); },
            target => ShowMaxRange(target, Map.Parent.Tile, MaxRange));
    }

    private bool ChoseWorldTarget_AllAttack(GlobalTargetInfo Target)
    {
        var map = Current.Game.FindMap(Target.WorldObject.Tile);
        var num = Find.WorldGrid.TraversalDistanceBetween(parent.Map.Parent.Tile, Target.Tile);
        var num2 = 66;
        if (num > num2 && !MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange)
        {
            return true;
        }

        if (map == null)
        {
            Log.Error("DrakkenLaserDrill_CrossMap_Error".Translate());
            return false;
        }

        DoSomething_II_AllAttack(map, Target.WorldObject.Tile);

        return true;
    }

    private void DoSomething_II_AllAttack(Map TargetMap, int TargetTileNum)
    {
        Current.Game.CurrentMap = TargetMap;
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill is { Now_Rebuilding: true } or { IfImmunity: true })
        {
            return;
        }

        var ifAttackDown = building_DrakkenLaserDrill is { IfAttackDown: true };
        building_DrakkenLaserDrill?.DestroyAllBeacon();
        if (building_DrakkenLaserDrill is { Now_Rebuilding: true })
        {
            return;
        }

        var allPawns = TargetMap.mapPawns.AllPawns;
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

        if (list.Count <= 0)
        {
            return;
        }

        var building_DrakkenLaserDrill_Beacon_CrossMap =
            (Building_DrakkenLaserDrill_Beacon_CrossMap)ThingMaker.MakeThing(MYDE_ThingDefOf
                .MYDE_Building_DrakkenLaserDrill_Beacon_CrossMap);
        Thing thing = null;
        var list2 = new List<Thing>();
        foreach (var item in list)
        {
            if (!item.Position.Fogged(TargetMap) && item.Position.x > 0 && item.Position.x < TargetMap.Size.x &&
                item.Position.z > 0 && item.Position.z < TargetMap.Size.z)
            {
                list2.Add(item);
            }
        }

        if (list2.Count > 0)
        {
            var num = 500f;
            foreach (var thing1 in list2)
            {
                var center = TargetMap.Center;
                var position = thing1.Position;
                var lengthHorizontal = (center - position).LengthHorizontal;
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
                var position2 = thing.Position;
                var realPos = new Vector2(drawPos.x, drawPos.z);
                var headingFromTo = Find.WorldGrid.GetHeadingFromTo(parent.Map.Tile, TargetTileNum);
                var num2 = headingFromTo - 180f;
                if (num2 < 0f)
                {
                    num2 += 360f;
                }

                var num3 = Find.WorldGrid.TraversalDistanceBetween(parent.Map.Tile, TargetTileNum);
                float num4 = num3 * parent.Map.Size.x;
                var vector3_By_AngleFlat =
                    MYDE_ModFront.GetVector3_By_AngleFlat(TargetMap.Center.ToVector3(), num4, headingFromTo);
                ((Building_DrakkenLaserDrill_Beacon_CrossMap)GenSpawn.Spawn(building_DrakkenLaserDrill_Beacon_CrossMap,
                    position2, TargetMap)).CheckSpawn(building_DrakkenLaserDrill, realPos, list, thing, num2,
                    vector3_By_AngleFlat, num4);
            }

            if (building_DrakkenLaserDrill != null)
            {
                building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap =
                    building_DrakkenLaserDrill_Beacon_CrossMap;
            }
        }
        else
        {
            Log.Message("Drakken Laser Drill Not Valid Target");
        }
    }

    public static string ShowMaxRange(GlobalTargetInfo target, int tile, int MaxRange)
    {
        if (MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange)
        {
            return "DrakkenLaserDrill_AttackTarget".Translate() + "DrakkenLaserDrill_AttackTarget_Tip".Translate();
        }

        if (!target.IsValid)
        {
            return null;
        }

        var num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile);
        if (num <= MaxRange)
        {
            return "DrakkenLaserDrill_AttackTarget".Translate() + "DrakkenLaserDrill_AttackTarget_Tip".Translate();
        }

        GUI.color = ColorLibrary.RedReadable;
        return "DrakkenLaserDrill_OutOfRange".Translate();
    }

    public override void PostDraw()
    {
        base.PostDraw();
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon_CrossMap == null ||
            building_DrakkenLaserDrill.Now_Rebuilding)
        {
            return;
        }

        if (building_DrakkenLaserDrill.DamageNum >= 6)
        {
            var drawPos = building_DrakkenLaserDrill.DrawPos;
            var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
                building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.True_Map_Range,
                building_DrakkenLaserDrill.GunAngle);
            var pos = (drawPos + vector3_By_AngleFlat) / 2f;
            pos.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
            var lengthHorizontal = (drawPos.ToIntVec3() - vector3_By_AngleFlat.ToIntVec3()).LengthHorizontal;
            var x = 1.2f;
            if (building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.LaserScaleTick < 30)
            {
                x = building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.LaserScaleTick / 25f;
            }

            var angle = (drawPos - vector3_By_AngleFlat).AngleFlat();
            var a = 0.8f;
            var color = new Color(building_DrakkenLaserDrill.Color_Red / 255f,
                building_DrakkenLaserDrill.Color_Green / 255f, building_DrakkenLaserDrill.Color_Blue / 255f)
            {
                a = a
            };
            var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser"),
                ShaderDatabase.Transparent, color);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
        }

        Draw_MinLaserPos_Prepare();
    }

    private void Get_MinLaserPos_A_Pos()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (MinLaserPos_A_UpOrDown)
        {
            MinLaserPos_A_Range -= 0.01f;
            if (MinLaserPos_A_Range <= 0f - MinLaserPos_A_Range_Limit)
            {
                MinLaserPos_A_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_A_UpOrDown)
        {
            MinLaserPos_A_Range += 0.01f;
            if (MinLaserPos_A_Range >= MinLaserPos_A_Range_Limit)
            {
                MinLaserPos_A_UpOrDown = true;
            }
        }

        var minLaserPos_A_Range = MinLaserPos_A_Range;
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var drawPos = building_DrakkenLaserDrill.DrawPos;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.True_Map_Range,
            building_DrakkenLaserDrill.GunAngle);
        var num = (drawPos - vector3_By_AngleFlat).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_A_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_A_Range, num2);
        MinLaserPos_A_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_A_Range, num2);
    }

    private void Get_MinLaserPos_B_Pos()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (MinLaserPos_B_UpOrDown)
        {
            MinLaserPos_B_Range -= 0.01f;
            if (MinLaserPos_B_Range <= 0f - MinLaserPos_B_Range_Limit)
            {
                MinLaserPos_B_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_B_UpOrDown)
        {
            MinLaserPos_B_Range += 0.01f;
            if (MinLaserPos_B_Range >= MinLaserPos_B_Range_Limit)
            {
                MinLaserPos_B_UpOrDown = true;
            }
        }

        var minLaserPos_B_Range = MinLaserPos_B_Range;
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var drawPos = building_DrakkenLaserDrill.DrawPos;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.True_Map_Range,
            building_DrakkenLaserDrill.GunAngle);
        var num = (drawPos - vector3_By_AngleFlat).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_B_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_B_Range, num2);
        MinLaserPos_B_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_B_Range, num2);
    }

    private void Get_MinLaserPos_C_Pos()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (MinLaserPos_C_UpOrDown)
        {
            MinLaserPos_C_Range -= 0.01f;
            if (MinLaserPos_C_Range <= 0f - MinLaserPos_C_Range_Limit)
            {
                MinLaserPos_C_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_C_UpOrDown)
        {
            MinLaserPos_C_Range += 0.01f;
            if (MinLaserPos_C_Range >= MinLaserPos_C_Range_Limit)
            {
                MinLaserPos_C_UpOrDown = true;
            }
        }

        var minLaserPos_C_Range = MinLaserPos_C_Range;
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var drawPos = building_DrakkenLaserDrill.DrawPos;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.True_Map_Range,
            building_DrakkenLaserDrill.GunAngle);
        var num = (drawPos - vector3_By_AngleFlat).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_C_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_C_Range, num2);
        MinLaserPos_C_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_C_Range, num2);
    }

    private void Get_MinLaserPos_D_Pos()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (MinLaserPos_D_UpOrDown)
        {
            MinLaserPos_D_Range -= 0.01f;
            if (MinLaserPos_D_Range <= 0f - MinLaserPos_D_Range_Limit)
            {
                MinLaserPos_D_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_D_UpOrDown)
        {
            MinLaserPos_D_Range += 0.01f;
            if (MinLaserPos_D_Range >= MinLaserPos_D_Range_Limit)
            {
                MinLaserPos_D_UpOrDown = true;
            }
        }

        var minLaserPos_D_Range = MinLaserPos_D_Range;
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var drawPos = building_DrakkenLaserDrill.DrawPos;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.True_Map_Range,
            building_DrakkenLaserDrill.GunAngle);
        var num = (drawPos - vector3_By_AngleFlat).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_D_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_D_Range, num2);
        MinLaserPos_D_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_D_Range, num2);
    }

    private void Get_MinLaserPos_E_Pos()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (MinLaserPos_E_UpOrDown)
        {
            MinLaserPos_E_Range -= 0.01f;
            if (MinLaserPos_E_Range <= 0f - MinLaserPos_E_Range_Limit)
            {
                MinLaserPos_E_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_E_UpOrDown)
        {
            MinLaserPos_E_Range += 0.01f;
            if (MinLaserPos_E_Range >= MinLaserPos_E_Range_Limit)
            {
                MinLaserPos_E_UpOrDown = true;
            }
        }

        var minLaserPos_E_Range = MinLaserPos_E_Range;
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var drawPos = building_DrakkenLaserDrill.DrawPos;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.True_Map_Range,
            building_DrakkenLaserDrill.GunAngle);
        var num = (drawPos - vector3_By_AngleFlat).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_E_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_E_Range, num2);
        MinLaserPos_E_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_E_Range, num2);
    }

    private void Get_MinLaserPos_F_Pos()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (MinLaserPos_F_UpOrDown)
        {
            MinLaserPos_F_Range -= 0.01f;
            if (MinLaserPos_F_Range <= 0f - MinLaserPos_F_Range_Limit)
            {
                MinLaserPos_F_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_F_UpOrDown)
        {
            MinLaserPos_F_Range += 0.01f;
            if (MinLaserPos_F_Range >= MinLaserPos_F_Range_Limit)
            {
                MinLaserPos_F_UpOrDown = true;
            }
        }

        var minLaserPos_F_Range = MinLaserPos_F_Range;
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var drawPos = building_DrakkenLaserDrill.DrawPos;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.True_Map_Range,
            building_DrakkenLaserDrill.GunAngle);
        var num = (drawPos - vector3_By_AngleFlat).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_F_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_F_Range, num2);
        MinLaserPos_F_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_F_Range, num2);
    }

    private void Draw_MinLaserPos_Prepare()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill is { DamageNum: >= 7 })
        {
            Draw_MinLaserPos(MinLaserPos_A_UpOrDown, MinLaserPos_A_Start, MinLaserPos_A_End);
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 8 })
        {
            Draw_MinLaserPos(MinLaserPos_B_UpOrDown, MinLaserPos_B_Start, MinLaserPos_B_End);
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 9 })
        {
            Draw_MinLaserPos(MinLaserPos_C_UpOrDown, MinLaserPos_C_Start, MinLaserPos_C_End);
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 10 })
        {
            Draw_MinLaserPos(MinLaserPos_D_UpOrDown, MinLaserPos_D_Start, MinLaserPos_D_End);
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 11 })
        {
            Draw_MinLaserPos(MinLaserPos_E_UpOrDown, MinLaserPos_E_Start, MinLaserPos_E_End);
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 12 })
        {
            Draw_MinLaserPos(MinLaserPos_F_UpOrDown, MinLaserPos_F_Start, MinLaserPos_F_End);
        }
    }

    private void Draw_MinLaserPos(bool UpOrDown, Vector3 Start, Vector3 End)
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        var incOffset = 4f;
        if (!UpOrDown)
        {
            incOffset = 2f;
        }

        var pos = (Start + End) / 2f;
        pos.y = AltitudeLayer.PawnRope.AltitudeFor(incOffset);
        var lengthHorizontal = (Start.ToIntVec3() - End.ToIntVec3()).LengthHorizontal;
        var x = MinLaser_Width;
        if (building_DrakkenLaserDrill != null &&
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.LaserScaleTick < 30)
        {
            x = building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap.LaserScaleTick / 100f;
        }

        var angle = (Start - End).AngleFlat();
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var color = new Color(building_DrakkenLaserDrill.Color_Min_Red / 255f,
            building_DrakkenLaserDrill.Color_Min_Green / 255f, building_DrakkenLaserDrill.Color_Min_Blue / 255f)
        {
            a = MinLaser_Alpha
        };
        var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser"),
            ShaderDatabase.Transparent, color);
        var matrix = default(Matrix4x4);
        matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
        Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
    }

    public override void CompTick()
    {
        Building_DrakkenLaserDrill_Icon_CrossMap =
            ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Beacon_CrossMap");
        Building_DrakkenLaserDrill_Icon_AllAttack_CrossMap =
            ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/AllAttack _CrossMap");
        if (parent is Building_DrakkenLaserDrill building_DrakkenLaserDrill &&
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap == null)
        {
            return;
        }

        Get_MinLaserPos_A_Pos();
        Get_MinLaserPos_B_Pos();
        Get_MinLaserPos_C_Pos();
        Get_MinLaserPos_D_Pos();
        Get_MinLaserPos_E_Pos();
        Get_MinLaserPos_F_Pos();
    }
}