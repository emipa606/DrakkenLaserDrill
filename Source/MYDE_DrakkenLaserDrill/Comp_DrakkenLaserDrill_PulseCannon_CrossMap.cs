using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_PulseCannon_CrossMap : ThingComp
{
    private readonly float MinLaser_Deviation_Range = 12f;

    private readonly float MinLaser_Deviation_Range_Speed = 15f;

    private readonly float MinLaser_Rotate_Speed = 2f;

    private readonly float MinLaserPos_A_Range_Limit = 0.4f;

    private readonly float MinLaserPos_B_Range_Limit = 0.4f;

    private readonly float MinLaserPos_C_Range_Limit = 0.4f;

    private readonly float MinLaserPos_D_Range_Limit = 0.4f;

    private readonly float MinLaserPos_E_Range_Limit = 0.4f;

    private readonly float MinLaserPos_F_Range_Limit = 0.4f;
    private Texture2D Building_DrakkenLaserDrill_PulseCannon_Icon_CrossMap;
    private string Building_DrakkenLaserDrill_PulseCannon_Label_CrossMap;

    public int EffectTick;

    private int EffectTickMax = 5;

    public int LaserScaleTick;

    public float MinLaser_Alpha = 0.6f;

    public float MinLaser_Width = 0.3f;

    public int MinLaserChangeTick;

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

    public CompProperties_DrakkenLaserDrill_PulseCannon_CrossMap Props =>
        props as CompProperties_DrakkenLaserDrill_PulseCannon_CrossMap;

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
        Scribe_Values.Look(ref LaserScaleTick, "LaserScaleTick");
        Scribe_Values.Look(ref EffectTick, "EffectTick");
        Scribe_Values.Look(ref EffectTickMax, "EffectTickMax");
        Scribe_Values.Look(ref MinLaserChangeTick, "MinLaserChangeTick");
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        var ResearchProject =
            DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find(x =>
                x.defName == "MYDE_DrakkenLaserDrill_Research_PulseCannon");
        if (parent is not Building_DrakkenLaserDrill { IfCrossMap: true } Building_DrakkenLaserDrill ||
            !ResearchProject.IsFinished)
        {
            yield break;
        }

        var PowerConsumeNum = Building_DrakkenLaserDrill.Base_ConsumePowerFactor *
                              Building_DrakkenLaserDrill.DamageNum *
                              Building_DrakkenLaserDrill.PowerConsumeFactor_PulseCannon;
        _ = PowerConsumeNum * CompPower.WattsToWattDaysPerTick * 180f;
        yield return new Command_Action
        {
            action = DoSomething_I,
            defaultLabel = Building_DrakkenLaserDrill_PulseCannon_Label_CrossMap,
            icon = Building_DrakkenLaserDrill_PulseCannon_Icon_CrossMap,
            defaultDesc = "DrakkenLaserDrill_PulseCannon_Desc".Translate()
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
        var compPower = Building_DrakkenLaserDrill.TryGetComp<CompPower>();
        if (Building_DrakkenLaserDrill != null)
        {
            var num = Building_DrakkenLaserDrill.Base_ConsumePowerFactor * Building_DrakkenLaserDrill.DamageNum *
                      Building_DrakkenLaserDrill.PowerConsumeFactor_PulseCannon;
            var num2 = num * CompPower.WattsToWattDaysPerTick * 180f;
            var num3 = compPower.PowerNet.CurrentStoredEnergy();
            if (!(num2 < num3) ||
                !(Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation >=
                  Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax) ||
                Building_DrakkenLaserDrill.IfImmunity)
            {
                return;
            }
        }

        if (Building_DrakkenLaserDrill == null)
        {
            return;
        }

        Building_DrakkenLaserDrill.DestroyAllBeacon();
        if (Building_DrakkenLaserDrill.Now_Rebuilding)
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
            var cell = Target.Cell;
            var realPos = new Vector2(Target.CenterVector3.x, Target.CenterVector3.z);
            var building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap =
                (Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap)ThingMaker.MakeThing(MYDE_ThingDefOf
                    .MYDE_Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap);
            var headingFromTo = Find.WorldGrid.GetHeadingFromTo(parent.Map.Tile, TargetTileNum);
            var num4 = headingFromTo - 180f;
            if (num4 < 0f)
            {
                num4 += 360f;
            }

            var num5 = Find.WorldGrid.TraversalDistanceBetween(parent.Map.Tile, TargetTileNum);
            float num6 = num5 * parent.Map.Size.x;
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(TargetMap.Center.ToVector3(), num6, headingFromTo);
            ((Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap)GenSpawn.Spawn(
                    building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap, cell, TargetMap))
                .CheckSpawn(Building_DrakkenLaserDrill, realPos, num4, vector3_By_AngleFlat, num6);
            Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap =
                building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap;
            var mYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave =
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave;
            var effecter = new Effecter(mYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave)
            {
                scale = 1f
            };
            effecter.Trigger(new TargetInfo(Target.Cell, TargetMap), TargetInfo.Invalid);
            effecter.Cleanup();
            Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation = 0f;
        }, delegate(LocalTargetInfo target)
        {
            GenDraw.DrawRadiusRing(target.Cell, 12.9f, Color.red);
            GenDraw.DrawRadiusRing(target.Cell, 2.9f, Color.white);
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
        if (building_DrakkenLaserDrill != null &&
            (building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap == null ||
             building_DrakkenLaserDrill.Now_Rebuilding))
        {
            return;
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 6 })
        {
            var map = parent.Map;
            var drawPos = building_DrakkenLaserDrill.DrawPos;
            var vector = default(Vector3);
            for (var i = 0; i < 400; i += 2)
            {
                var vector3_By_AngleFlat =
                    MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, building_DrakkenLaserDrill.GunAngle);
                if (vector3_By_AngleFlat.x < 0f || vector3_By_AngleFlat.x > map.Size.x || vector3_By_AngleFlat.z < 0f ||
                    vector3_By_AngleFlat.z > map.Size.z)
                {
                    break;
                }

                vector = vector3_By_AngleFlat;
            }

            var x = 1f;
            if (LaserScaleTick <= 60)
            {
                x = LaserScaleTick / 10f;
            }
            else if (LaserScaleTick is <= 150 and > 60)
            {
                x = 6f;
            }
            else if (LaserScaleTick is <= 180 and > 150)
            {
                x = 6f - ((LaserScaleTick - 150) / 5f);
            }

            var angle = (drawPos - vector).AngleFlat();
            var a = 0.8f;
            var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 5.5f, angle);
            var pos = (vector3_By_AngleFlat2 + vector) / 2f;
            pos.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
            var lengthHorizontal = (vector3_By_AngleFlat2.ToIntVec3() - vector.ToIntVec3()).LengthHorizontal;
            var z = lengthHorizontal * 1.2f;
            var color = new Color(building_DrakkenLaserDrill.Color_Red / 255f,
                building_DrakkenLaserDrill.Color_Green / 255f, building_DrakkenLaserDrill.Color_Blue / 255f)
            {
                a = a
            };
            var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser_Big"),
                ShaderDatabase.Transparent, color);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, z));
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
        }

        var num = 65f;
        var rangeDeviation = 3.6f;
        var endRange = 7.8f;
        Draw_DecorationLaser(num, rangeDeviation, endRange);
        Draw_DecorationLaser(0f - num, rangeDeviation, endRange);
        Draw_MinLaserPos_Prepare();
    }

    private void Get_MinLaserPos_Prepare()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill is { DamageNum: >= 7 })
        {
            Get_MinLaserPos_A_Pos();
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 8 })
        {
            Get_MinLaserPos_B_Pos();
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 9 })
        {
            Get_MinLaserPos_C_Pos();
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 10 })
        {
            Get_MinLaserPos_D_Pos();
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 11 })
        {
            Get_MinLaserPos_E_Pos();
        }

        if (building_DrakkenLaserDrill is { DamageNum: >= 12 })
        {
            Get_MinLaserPos_F_Pos();
        }
    }

    private void Get_MinLaserPos_A_Pos()
    {
        var map = parent.Map;
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
        var vector = default(Vector3);
        for (var i = 0; i < 400; i += 2)
        {
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, building_DrakkenLaserDrill.GunAngle);
            if (vector3_By_AngleFlat.x < 0f || vector3_By_AngleFlat.x > map.Size.x || vector3_By_AngleFlat.z < 0f ||
                vector3_By_AngleFlat.z > map.Size.z)
            {
                break;
            }

            vector = vector3_By_AngleFlat;
        }

        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_A_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat2, minLaserPos_A_Range, num2);
        var range = MinLaser_Deviation_Range - (MinLaserChangeTick / MinLaser_Deviation_Range_Speed);
        var num3 = 0f + (MinLaserChangeTick * MinLaser_Rotate_Speed);
        if (num3 > 360f)
        {
            num3 -= 360f;
            if (num3 > 360f)
            {
                num3 -= 360f;
            }
        }

        var vector3_By_AngleFlat3 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_A_End = vector3_By_AngleFlat3;
    }

    private void Get_MinLaserPos_B_Pos()
    {
        var map = parent.Map;
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
        var vector = default(Vector3);
        for (var i = 0; i < 400; i += 2)
        {
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, building_DrakkenLaserDrill.GunAngle);
            if (vector3_By_AngleFlat.x < 0f || vector3_By_AngleFlat.x > map.Size.x || vector3_By_AngleFlat.z < 0f ||
                vector3_By_AngleFlat.z > map.Size.z)
            {
                break;
            }

            vector = vector3_By_AngleFlat;
        }

        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_B_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat2, minLaserPos_B_Range, num2);
        var range = MinLaser_Deviation_Range - (MinLaserChangeTick / MinLaser_Deviation_Range_Speed);
        var num3 = 180f + (MinLaserChangeTick * MinLaser_Rotate_Speed);
        if (num3 > 360f)
        {
            num3 -= 360f;
            if (num3 > 360f)
            {
                num3 -= 360f;
            }
        }

        var vector3_By_AngleFlat3 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_B_End = vector3_By_AngleFlat3;
    }

    private void Get_MinLaserPos_C_Pos()
    {
        var map = parent.Map;
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
        var vector = default(Vector3);
        for (var i = 0; i < 400; i += 2)
        {
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, building_DrakkenLaserDrill.GunAngle);
            if (vector3_By_AngleFlat.x < 0f || vector3_By_AngleFlat.x > map.Size.x || vector3_By_AngleFlat.z < 0f ||
                vector3_By_AngleFlat.z > map.Size.z)
            {
                break;
            }

            vector = vector3_By_AngleFlat;
        }

        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_C_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat2, minLaserPos_C_Range, num2);
        var range = MinLaser_Deviation_Range - (MinLaserChangeTick / MinLaser_Deviation_Range_Speed);
        var num3 = 60f + (MinLaserChangeTick * MinLaser_Rotate_Speed);
        if (num3 > 360f)
        {
            num3 -= 360f;
            if (num3 > 360f)
            {
                num3 -= 360f;
            }
        }

        var vector3_By_AngleFlat3 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_C_End = vector3_By_AngleFlat3;
    }

    private void Get_MinLaserPos_D_Pos()
    {
        var map = parent.Map;
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
        var vector = default(Vector3);
        for (var i = 0; i < 400; i += 2)
        {
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, building_DrakkenLaserDrill.GunAngle);
            if (vector3_By_AngleFlat.x < 0f || vector3_By_AngleFlat.x > map.Size.x || vector3_By_AngleFlat.z < 0f ||
                vector3_By_AngleFlat.z > map.Size.z)
            {
                break;
            }

            vector = vector3_By_AngleFlat;
        }

        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_D_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat2, minLaserPos_D_Range, num2);
        var range = MinLaser_Deviation_Range - (MinLaserChangeTick / MinLaser_Deviation_Range_Speed);
        var num3 = 240f + (MinLaserChangeTick * MinLaser_Rotate_Speed);
        if (num3 > 360f)
        {
            num3 -= 360f;
            if (num3 > 360f)
            {
                num3 -= 360f;
            }
        }

        var vector3_By_AngleFlat3 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_D_End = vector3_By_AngleFlat3;
    }

    private void Get_MinLaserPos_E_Pos()
    {
        var map = parent.Map;
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
        var vector = default(Vector3);
        for (var i = 0; i < 400; i += 2)
        {
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, building_DrakkenLaserDrill.GunAngle);
            if (vector3_By_AngleFlat.x < 0f || vector3_By_AngleFlat.x > map.Size.x || vector3_By_AngleFlat.z < 0f ||
                vector3_By_AngleFlat.z > map.Size.z)
            {
                break;
            }

            vector = vector3_By_AngleFlat;
        }

        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_E_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat2, minLaserPos_E_Range, num2);
        var range = MinLaser_Deviation_Range - (MinLaserChangeTick / MinLaser_Deviation_Range_Speed);
        var num3 = 120f + (MinLaserChangeTick * MinLaser_Rotate_Speed);
        if (num3 > 360f)
        {
            num3 -= 360f;
            if (num3 > 360f)
            {
                num3 -= 360f;
            }
        }

        var vector3_By_AngleFlat3 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_E_End = vector3_By_AngleFlat3;
    }

    private void Get_MinLaserPos_F_Pos()
    {
        var map = parent.Map;
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
        var vector = default(Vector3);
        for (var i = 0; i < 400; i += 2)
        {
            var vector3_By_AngleFlat =
                MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, building_DrakkenLaserDrill.GunAngle);
            if (vector3_By_AngleFlat.x < 0f || vector3_By_AngleFlat.x > map.Size.x || vector3_By_AngleFlat.z < 0f ||
                vector3_By_AngleFlat.z > map.Size.z)
            {
                break;
            }

            vector = vector3_By_AngleFlat;
        }

        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_F_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat2, minLaserPos_F_Range, num2);
        var range = MinLaser_Deviation_Range - (MinLaserChangeTick / MinLaser_Deviation_Range_Speed);
        var num3 = 300f + (MinLaserChangeTick * MinLaser_Rotate_Speed);
        if (num3 > 360f)
        {
            num3 -= 360f;
            if (num3 > 360f)
            {
                num3 -= 360f;
            }
        }

        var vector3_By_AngleFlat3 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_F_End = vector3_By_AngleFlat3;
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

        AltitudeLayer.PawnRope.AltitudeFor(incOffset);
        var x = 1f;
        var num = 20;
        var num2 = 160;
        var num3 = 170;
        var num4 = 180;
        if (LaserScaleTick < num)
        {
            x = LaserScaleTick / 20f;
        }

        if (LaserScaleTick <= num3 && LaserScaleTick > num2)
        {
            x = 5f;
        }
        else if (LaserScaleTick <= num4 && LaserScaleTick > num3)
        {
            x = 5f - ((LaserScaleTick - num3) / 2f);
        }

        var angle = (Start - End).AngleFlat();
        var a = 0.8f;
        if (LaserScaleTick >= 175)
        {
            a = 0.8f - ((LaserScaleTick - 175) / 62f);
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(Start, 5.5f, angle);
        vector3_By_AngleFlat.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
        var vect = default(Vector3);
        for (var i = 0; i < 500; i += 50)
        {
            var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(End, i, angle);
            if (building_DrakkenLaserDrill != null &&
                (vector3_By_AngleFlat2.x > building_DrakkenLaserDrill.Map.Size.x || vector3_By_AngleFlat2.x < 0f ||
                 vector3_By_AngleFlat2.z > building_DrakkenLaserDrill.Map.Size.z || vector3_By_AngleFlat2.z < 0f))
            {
                break;
            }

            vect = vector3_By_AngleFlat2;
        }

        var lengthHorizontal = (vector3_By_AngleFlat.ToIntVec3() - vect.ToIntVec3()).LengthHorizontal;
        var num5 = 2f;
        var z = (lengthHorizontal * num5) + 100f;
        if (building_DrakkenLaserDrill == null)
        {
            return;
        }

        var color = new Color(building_DrakkenLaserDrill.Color_Min_Red / 255f,
            building_DrakkenLaserDrill.Color_Min_Green / 255f, building_DrakkenLaserDrill.Color_Min_Blue / 255f)
        {
            a = a
        };
        var material =
            MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser_Big_ConcentratedBeam"),
                ShaderDatabase.Transparent, color);
        var matrix = default(Matrix4x4);
        matrix.SetTRS(vector3_By_AngleFlat, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, z));
        Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
    }

    private void Draw_DecorationLaser(float AngleDeviation, float RangeDeviation, float EndRange)
    {
        if (parent is not Building_DrakkenLaserDrill building_DrakkenLaserDrill)
        {
            return;
        }

        var drawPos = building_DrakkenLaserDrill.DrawPos;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos,
            building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap.True_Map_Range,
            building_DrakkenLaserDrill.GunAngle);
        var x = 1f;
        if (LaserScaleTick <= 60)
        {
            x = LaserScaleTick / 60f;
        }
        else if (LaserScaleTick is <= 150 and > 60)
        {
            x = 1f;
        }
        else if (LaserScaleTick is <= 180 and > 150)
        {
            x = 1f - ((LaserScaleTick - 150) / 30f);
        }

        var num = (drawPos - vector3_By_AngleFlat).AngleFlat();
        var a = 0.8f;
        var num2 = num + AngleDeviation;
        if (num2 > 360f)
        {
            num2 -= 360f;
        }
        else if (num2 < 0f)
        {
            num2 += 360f;
        }

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, RangeDeviation, num2);
        var vector3_By_AngleFlat3 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, EndRange, num);
        var pos = (vector3_By_AngleFlat2 + vector3_By_AngleFlat3) / 2f;
        pos.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
        var lengthHorizontal = (vector3_By_AngleFlat2.ToIntVec3() - vector3_By_AngleFlat3.ToIntVec3()).LengthHorizontal;
        var angle = (vector3_By_AngleFlat2 - vector3_By_AngleFlat3).AngleFlat();
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

    private void DoEffect()
    {
        if (parent is not Building_DrakkenLaserDrill building_DrakkenLaserDrill)
        {
            return;
        }

        var map = building_DrakkenLaserDrill.Map;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(building_DrakkenLaserDrill.DrawPos, 7.8f,
            building_DrakkenLaserDrill.GunAngle);
        var dataStatic = FleckMaker.GetDataStatic(vector3_By_AngleFlat, map,
            MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_HeatGlow_Intense, 3f);
        map.flecks.CreateFleck(dataStatic);
        FleckMaker.ThrowMicroSparks(vector3_By_AngleFlat, map);
        FleckMaker.ThrowLightningGlow(vector3_By_AngleFlat, map, 1.5f);
        var num = (vector3_By_AngleFlat - building_DrakkenLaserDrill.DrawPos).AngleFlat();
        for (var i = 0; i < 4; i++)
        {
            var num2 = Rand.Range(-0.3f, 0.3f);
            var loc = vector3_By_AngleFlat + new Vector3(num2, 0f, num2);
            var scale = Rand.Range(0.5f, 0.8f);
            var dataStatic2 = FleckMaker.GetDataStatic(loc, map,
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale);
            var num3 = num + Rand.Range(-30f, 30f);
            if (num3 > 180f)
            {
                num3 = num3 - 180f + -180f;
            }

            if (num3 < -180f)
            {
                num3 = num3 + 180f + 180f;
            }

            dataStatic2.velocityAngle = num3;
            dataStatic2.velocitySpeed = Rand.Range(5f, 10f);
            map.flecks.CreateFleck(dataStatic2);
        }
    }

    public override void CompTick()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill != null && building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation <
            building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax)
        {
            Building_DrakkenLaserDrill_PulseCannon_Label_CrossMap =
                (int)building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation + " / " +
                building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax;
            Building_DrakkenLaserDrill_PulseCannon_Icon_CrossMap =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Nothing/Nothing");
        }
        else if (building_DrakkenLaserDrill != null && building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation >=
                 building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax)
        {
            var num = building_DrakkenLaserDrill.Base_ConsumePowerFactor * building_DrakkenLaserDrill.DamageNum *
                      building_DrakkenLaserDrill.PowerConsumeFactor_PulseCannon;
            var num2 = num * CompPower.WattsToWattDaysPerTick * 180f;
            Building_DrakkenLaserDrill_PulseCannon_Label_CrossMap =
                "DrakkenLaserDrill_PulseCannon_Label".Translate() + "：" + num2.ToString();
            Building_DrakkenLaserDrill_PulseCannon_Icon_CrossMap =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/PulseCannon_CrossMap");
        }

        if (building_DrakkenLaserDrill != null &&
            (building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap == null ||
             building_DrakkenLaserDrill.Now_Rebuilding))
        {
            return;
        }

        LaserScaleTick++;
        MinLaserChangeTick++;
        Get_MinLaserPos_Prepare();
        EffectTick++;
        if (EffectTick < EffectTickMax)
        {
            return;
        }

        EffectTick = 0;
        DoEffect();
    }
}