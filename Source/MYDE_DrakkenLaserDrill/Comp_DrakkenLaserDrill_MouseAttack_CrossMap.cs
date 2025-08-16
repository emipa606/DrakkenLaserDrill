using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_MouseAttack_CrossMap : ThingComp
{
    private readonly float MinLaser_Alpha = 0.6f;

    private readonly float MinLaser_Width = 0.3f;

    private readonly float MinLaserPos_A_Range_Limit = 0.4f;

    private readonly float MinLaserPos_B_Range_Limit = 0.4f;

    private readonly float MinLaserPos_C_Range_Limit = 0.4f;

    private readonly float MinLaserPos_D_Range_Limit = 0.4f;

    private readonly float MinLaserPos_E_Range_Limit = 0.4f;

    private readonly float MinLaserPos_F_Range_Limit = 0.4f;
    private Texture2D Building_DrakkenLaserDrill_MouseAttack_Icon_CrossMap;

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

    public CompProperties_DrakkenLaserDrill_MouseAttack_CrossMap Props =>
        props as CompProperties_DrakkenLaserDrill_MouseAttack_CrossMap;

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
        if (parent is Building_DrakkenLaserDrill { IfCrossMap: true })
        {
            yield return new Command_Action
            {
                action = DoSomething_I,
                defaultLabel = "DrakkenLaserDrill_MouseAttack_Label".Translate(),
                icon = Building_DrakkenLaserDrill_MouseAttack_Icon_CrossMap,
                defaultDesc = "DrakkenLaserDrill_MouseAttack_Desc".Translate()
            };
        }
    }

    private void DoSomething_I()
    {
        CameraJumper.TryJump(CameraJumper.GetWorldTarget(parent));
        Find.WorldSelector.ClearSelection();
        var Map = parent.Map;
        var MaxRange = 66;
        Find.WorldTargeter.BeginTargeting((Func<GlobalTargetInfo, bool>)ChoseWorldTarget, true, null, true,
            delegate { GenDraw.DrawWorldRadiusRing(Map.Parent.Tile, MaxRange); },
            target => Comp_DrakkenLaserDrill_Attack_CrossMap.ShowMaxRange(target, Map.Parent.Tile, MaxRange));
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
        Building_DrakkenLaserDrill?.DestroyAllBeacon();
        if (Building_DrakkenLaserDrill != null &&
            (Building_DrakkenLaserDrill.Now_Rebuilding || Building_DrakkenLaserDrill.IfImmunity))
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
            var centerVector = Target.CenterVector3;
            var cell = Target.Cell;
            var realPos = new Vector2(Target.CenterVector3.x, Target.CenterVector3.z);
            var building_DrakkenLaserDrill_Beacon_Mouse_CrossMap =
                (Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap)ThingMaker.MakeThing(MYDE_ThingDefOf
                    .MYDE_Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap);
            var headingFromTo = Find.WorldGrid.GetHeadingFromTo(parent.Map.Tile, TargetTileNum);
            var num = headingFromTo - 180f;
            if (num < 0f)
            {
                num += 360f;
            }

            var num2 = Find.WorldGrid.TraversalDistanceBetween(parent.Map.Tile, TargetTileNum);
            float num3 = num2 * parent.Map.Size.x;
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(TargetMap.Center.ToVector3(), num3, headingFromTo);
            ((Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap)GenSpawn.Spawn(
                    building_DrakkenLaserDrill_Beacon_Mouse_CrossMap, cell, TargetMap))
                .CheckSpawn(Building_DrakkenLaserDrill, realPos, centerVector, num, vector3_By_AngleFlat, num3);
            if (Building_DrakkenLaserDrill != null)
            {
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap =
                    building_DrakkenLaserDrill_Beacon_Mouse_CrossMap;
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
        Find.Targeter.BeginTargeting(targetingParameters, delegate { Building_DrakkenLaserDrill?.DestroyAllBeacon(); },
            delegate(LocalTargetInfo Target)
            {
                if (Building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap == null)
                {
                    return;
                }

                var centerVector = Target.CenterVector3;
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.TargetPos =
                    centerVector;
                var pos_Square = MYDE_ModFront.GetPos_Square(
                    Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.Position, 1, 1);
                GenDraw.DrawFieldEdges(pos_Square, Color.white);
            }, null);
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
        if (building_DrakkenLaserDrill?.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap == null ||
            building_DrakkenLaserDrill.Now_Rebuilding)
        {
            return;
        }

        if (building_DrakkenLaserDrill.DamageNum >= 6)
        {
            var drawPos = building_DrakkenLaserDrill.DrawPos;
            var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
                building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.True_Map_Range,
                building_DrakkenLaserDrill.GunAngle);
            var pos = (drawPos + vector3_By_AngleFlat) / 2f;
            pos.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
            var lengthHorizontal = (drawPos.ToIntVec3() - vector3_By_AngleFlat.ToIntVec3()).LengthHorizontal;
            var x = 1.2f;
            if (building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.LaserScaleTick < 30)
            {
                x = building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.LaserScaleTick / 25f;
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
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.True_Map_Range,
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
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.True_Map_Range,
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
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.True_Map_Range,
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
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.True_Map_Range,
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
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.True_Map_Range,
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
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.True_Map_Range,
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
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.LaserScaleTick < 30)
        {
            x = building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.LaserScaleTick / 100f;
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
        Building_DrakkenLaserDrill_MouseAttack_Icon_CrossMap =
            ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/MouseAttack_CrossMap");
        if (parent is Building_DrakkenLaserDrill { Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap: null })
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