using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Building_DrakkenLaserDrill_Beacon_PulseCannon : ThingWithComps
{
    private readonly float MinLaser_Alpha = 0.6f;

    private readonly float MinLaser_Deviation_Range = 12f;

    private readonly float MinLaser_Deviation_Range_Speed = 15f;

    private readonly float MinLaser_HeatGlow_Scale = 2f;

    private readonly float MinLaser_Rotate_Speed = 2f;

    private readonly int MinLaser_Spark_Num = 1;

    private readonly float MinLaser_Width = 1f;

    private readonly float MinLaserPos_A_Range_Limit = 0.4f;

    private readonly float MinLaserPos_B_Range_Limit = 0.4f;

    private readonly float MinLaserPos_C_Range_Limit = 0.4f;

    private readonly float MinLaserPos_D_Range_Limit = 0.4f;

    private readonly float MinLaserPos_E_Range_Limit = 0.4f;

    private readonly float MinLaserPos_F_Range_Limit = 0.4f;
    private float Angle;

    private Building_DrakkenLaserDrill Building_DrakkenLaserDrill;

    private int EffectTick;

    private int EffectTickMax = 5;

    private int LaserScaleTick;

    private int MinLaser_TakeDamageTick;

    private int MinLaser_TakeDamageTickMax = 10;

    private int MinLaserChangeTick;

    private Vector3 MinLaserPos_A_End;

    private float MinLaserPos_A_Range = 0.5f;
    private Vector3 MinLaserPos_A_Start;

    private bool MinLaserPos_A_UpOrDown = true;

    private Vector3 MinLaserPos_B_End;

    private float MinLaserPos_B_Range = -0.5f;

    private Vector3 MinLaserPos_B_Start;

    private bool MinLaserPos_B_UpOrDown;

    private Vector3 MinLaserPos_C_End;

    private float MinLaserPos_C_Range = 0.2f;

    private Vector3 MinLaserPos_C_Start;

    private bool MinLaserPos_C_UpOrDown = true;

    private Vector3 MinLaserPos_D_End;

    private float MinLaserPos_D_Range = -0.2f;

    private Vector3 MinLaserPos_D_Start;

    private bool MinLaserPos_D_UpOrDown;

    private Vector3 MinLaserPos_E_End;

    private float MinLaserPos_E_Range = -0.1f;

    private Vector3 MinLaserPos_E_Start;

    private bool MinLaserPos_E_UpOrDown = true;

    private Vector3 MinLaserPos_F_End;

    private float MinLaserPos_F_Range = 0.1f;

    private Vector3 MinLaserPos_F_Start;

    private bool MinLaserPos_F_UpOrDown;

    private Vector2 RealPos;

    private int TakeDamageTick;

    private int TakeDamageTickMax = 180;

    private int WaveEffectTick;

    private int WaveEffectTickMax = 45;

    public void CheckSpawn(Building_DrakkenLaserDrill buildingDrakkenLaserDrill, Vector2 realPos)
    {
        Building_DrakkenLaserDrill = buildingDrakkenLaserDrill;
        RealPos = realPos;
        Building_DrakkenLaserDrill.IfImmunity = true;
        var compPowerTrader = Building_DrakkenLaserDrill.TryGetComp<CompPowerTrader>();
        compPowerTrader.PowerOutput = 0f - (Building_DrakkenLaserDrill.Base_ConsumePowerFactor *
                                            Building_DrakkenLaserDrill.DamageNum * Building_DrakkenLaserDrill
                                                .PowerConsumeFactor_PulseCannon);
    }

    public override void ExposeData()
    {
        base.ExposeData();
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
        Scribe_References.Look(ref Building_DrakkenLaserDrill, "Building_DrakkenLaserDrill");
        Scribe_Values.Look(ref LaserScaleTick, "LaserScaleTick");
        Scribe_Values.Look(ref TakeDamageTick, "TakeDamageTick");
        Scribe_Values.Look(ref TakeDamageTickMax, "TakeDamageTickMax");
        Scribe_Values.Look(ref RealPos, "RealPos");
        Scribe_Values.Look(ref Angle, "Angle");
        Scribe_Values.Look(ref EffectTick, "EffectTick");
        Scribe_Values.Look(ref EffectTickMax, "EffectTickMax");
        Scribe_Values.Look(ref WaveEffectTick, "WaveEffectTick");
        Scribe_Values.Look(ref WaveEffectTickMax, "WaveEffectTickMax");
        Scribe_Values.Look(ref MinLaserChangeTick, "MinLaserChangeTick");
        Scribe_Values.Look(ref MinLaser_TakeDamageTick, "MinLaser_TakeDamageTick");
        Scribe_Values.Look(ref MinLaser_TakeDamageTickMax, "MinLaser_TakeDamageTickMax");
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        base.DrawAt(drawLoc, flip);
        if (Building_DrakkenLaserDrill.Now_Rebuilding)
        {
            return;
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 6)
        {
            var drawPos = Building_DrakkenLaserDrill.DrawPos;
            var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
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

            var num = (drawPos - vector).AngleFlat();
            var a = 0.8f;
            var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 5.5f, num);
            var pos = (vector3_By_AngleFlat + vector) / 2f;
            pos.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
            var lengthHorizontal = (vector3_By_AngleFlat.ToIntVec3() - vector.ToIntVec3()).LengthHorizontal;
            var color = new Color(Building_DrakkenLaserDrill.Color_Red / 255f,
                Building_DrakkenLaserDrill.Color_Green / 255f, Building_DrakkenLaserDrill.Color_Blue / 255f)
            {
                a = a
            };
            var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser_Big"),
                ShaderDatabase.Transparent, color);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.AngleAxis(num, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
            Building_DrakkenLaserDrill.GunAngle = num;
        }

        var num2 = 65f;
        var rangeDeviation = 3.6f;
        var endRange = 7.8f;
        Draw_DecorationLaser(num2, rangeDeviation, endRange);
        Draw_DecorationLaser(0f - num2, rangeDeviation, endRange);
        Draw_MinLaserPos_Prepare();
    }

    protected override void Tick()
    {
        base.Tick();
        var compPowerTrader = Building_DrakkenLaserDrill.TryGetComp<CompPowerTrader>();
        if (!compPowerTrader.PowerOn)
        {
            DestroyBeacon();
        }

        EffectTick++;
        if (EffectTick >= EffectTickMax)
        {
            EffectTick = 0;
            DoEffectAndTakeDamage();
        }

        Do_MinLaserPos_Effect_Prepare();
        WaveEffectTick++;
        if (WaveEffectTick >= WaveEffectTickMax)
        {
            WaveEffectTick = 0;
            var mYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave =
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave;
            var effecter = new Effecter(mYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave)
            {
                scale = 1f
            };
            effecter.Trigger(new TargetInfo(Position, Map), TargetInfo.Invalid);
            effecter.Cleanup();
        }

        LaserScaleTick++;
        MinLaserChangeTick++;
        Get_MinLaserPos_Prepare();
        MinLaser_TakeDamageTick++;
        if (MinLaser_TakeDamageTick >= MinLaser_TakeDamageTickMax)
        {
            MinLaser_TakeDamageTick = 0;
            LaserAndMinLaser_TakeDamage_Prepare();
        }

        Building_DrakkenLaserDrill.SetBlueBarPos();
        TakeDamageTick++;
        if (TakeDamageTick < TakeDamageTickMax)
        {
            return;
        }

        TakeDamageTick = 0;
        DoExplosionAndDestroy();
    }

    private void DoExplosionAndDestroy()
    {
        var map = Map;
        GenExplosion.DoExplosion(Position, map, 12.9f, DamageDefOf.Bomb, Building_DrakkenLaserDrill,
            Building_DrakkenLaserDrill.DamageNum * 10, Building_DrakkenLaserDrill.DamageArmorPenetration,
            DamageDefOf.Flame.soundExplosion, Building_DrakkenLaserDrill.def, null, null, null, 0f, 1, null, null, 0);
        var vaporize_Heatwave = EffecterDefOf.Vaporize_Heatwave;
        var effecter = new Effecter(vaporize_Heatwave)
        {
            scale = 4f
        };
        effecter.Trigger(new TargetInfo(Position, map), TargetInfo.Invalid);
        effecter.Cleanup();
        DestroyBeacon();
    }

    private void DestroyBeacon()
    {
        Destroy();
    }

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_PulseCannon = null;
        Building_DrakkenLaserDrill.IfImmunity = false;
        Building_DrakkenLaserDrill.ChangePowerConsumeToZero();
        Building_DrakkenLaserDrill.TryGetComp<Comp_DrakkenLaserDrill_AutoAttack>().PrepareToAttack();
        base.Destroy(mode);
    }

    private void DoEffectAndTakeDamage()
    {
        var map = Map;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = 2;
        for (var i = -num; i <= num; i++)
        {
            for (var j = -num; j <= num; j++)
            {
                var vect = DrawPos + new Vector3(i, 0f, j);
                var list = new List<Thing>();
                list.AddRange(vect.ToIntVec3().GetThingList(map));
                if (list.Count <= 0)
                {
                    continue;
                }

                // ReSharper disable once ForCanBeConvertedToForeach
                for (var k = 0; k < list.Count; k++)
                {
                    if (list[k] is not Building)
                    {
                        var dinfo = new DamageInfo(DamageDefOf.Cut, Building_DrakkenLaserDrill.DamageNum,
                            Building_DrakkenLaserDrill.DamageArmorPenetration, 0f, Building_DrakkenLaserDrill, null,
                            Building_DrakkenLaserDrill.def);
                        list[k].TakeDamage(dinfo);
                    }
                    else if (list[k] is Building)
                    {
                        var dinfo2 = new DamageInfo(DamageDefOf.Cut, Building_DrakkenLaserDrill.DamageNum * 5f,
                            Building_DrakkenLaserDrill.DamageArmorPenetration, 0f, Building_DrakkenLaserDrill, null,
                            Building_DrakkenLaserDrill.def);
                        list[k].TakeDamage(dinfo2);
                    }
                }
            }
        }

        var dataStatic = FleckMaker.GetDataStatic(vector, map,
            MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_HeatGlow_Intense, 6f);
        map.flecks.CreateFleck(dataStatic);
        FleckMaker.ThrowMicroSparks(vector, Map);
        FleckMaker.ThrowLightningGlow(vector, Map, 1.5f);
        for (var l = 0; l < 8; l++)
        {
            var num2 = Rand.Range(-0.3f, 0.3f);
            var loc = vector + new Vector3(num2, 0f, num2);
            var scale = Rand.Range(1f, 1.5f);
            var dataStatic2 = FleckMaker.GetDataStatic(loc, Map,
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale);
            var velocityAngle = Rand.Range(-180f, 180f);
            dataStatic2.velocityAngle = velocityAngle;
            dataStatic2.velocitySpeed = Rand.Range(5f, 10f);
            Map.flecks.CreateFleck(dataStatic2);
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(Building_DrakkenLaserDrill.DrawPos, 7.8f,
            Building_DrakkenLaserDrill.GunAngle);
        var dataStatic3 = FleckMaker.GetDataStatic(vector3_By_AngleFlat, map,
            MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_HeatGlow_Intense, 3f);
        map.flecks.CreateFleck(dataStatic3);
        FleckMaker.ThrowMicroSparks(vector3_By_AngleFlat, Map);
        FleckMaker.ThrowLightningGlow(vector3_By_AngleFlat, Map, 1.5f);
        var num3 = (vector3_By_AngleFlat - Building_DrakkenLaserDrill.DrawPos).AngleFlat();
        for (var m = 0; m < 4; m++)
        {
            var num4 = Rand.Range(-0.3f, 0.3f);
            var loc2 = vector3_By_AngleFlat + new Vector3(num4, 0f, num4);
            var scale2 = Rand.Range(0.5f, 0.8f);
            var dataStatic4 = FleckMaker.GetDataStatic(loc2, Map,
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale2);
            var num5 = num3 + Rand.Range(-30f, 30f);
            if (num5 > 180f)
            {
                num5 = num5 - 180f + -180f;
            }

            if (num5 < -180f)
            {
                num5 = num5 + 180f + 180f;
            }

            dataStatic4.velocityAngle = num5;
            dataStatic4.velocitySpeed = Rand.Range(5f, 10f);
            Map.flecks.CreateFleck(dataStatic4);
        }

        var compPowerTrader = Building_DrakkenLaserDrill.TryGetComp<CompPowerTrader>();
        compPowerTrader.PowerOutput = 0f - (Building_DrakkenLaserDrill.Base_ConsumePowerFactor *
                                            Building_DrakkenLaserDrill.DamageNum * Building_DrakkenLaserDrill
                                                .PowerConsumeFactor_PulseCannon);
    }

    private void LaserAndMinLaser_TakeDamage_Prepare()
    {
        if (Building_DrakkenLaserDrill.Now_Rebuilding)
        {
            return;
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 7)
        {
            Laser_TakeDamage(MinLaserPos_A_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 8)
        {
            Laser_TakeDamage(MinLaserPos_B_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 9)
        {
            Laser_TakeDamage(MinLaserPos_C_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 10)
        {
            Laser_TakeDamage(MinLaserPos_D_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 11)
        {
            Laser_TakeDamage(MinLaserPos_E_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 12)
        {
            Laser_TakeDamage(MinLaserPos_F_End);
        }
    }

    private void Laser_TakeDamage(Vector3 Pos)
    {
        var list = new List<Thing>();
        list.AddRange(Pos.ToIntVec3().GetThingList(Map));
        if (list.Count > 0)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is not Building)
                {
                    var dinfo = new DamageInfo(DamageDefOf.Cut, Building_DrakkenLaserDrill.DamageNum,
                        Building_DrakkenLaserDrill.DamageArmorPenetration, 0f, Building_DrakkenLaserDrill, null,
                        Building_DrakkenLaserDrill.def);
                    list[i].TakeDamage(dinfo);
                }
                else if (list[i] is Building)
                {
                    var dinfo2 = new DamageInfo(DamageDefOf.Cut, Building_DrakkenLaserDrill.DamageNum * 5f,
                        Building_DrakkenLaserDrill.DamageArmorPenetration, 0f, Building_DrakkenLaserDrill, null,
                        Building_DrakkenLaserDrill.def);
                    list[i].TakeDamage(dinfo2);
                }
            }
        }

        FleckMaker.ThrowMicroSparks(Pos, Map);
        FleckMaker.ThrowLightningGlow(Pos, Map, 1.5f);
    }

    private void Get_MinLaserPos_Prepare()
    {
        if (Building_DrakkenLaserDrill.DamageNum >= 7)
        {
            Get_MinLaserPos_A_Pos();
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 8)
        {
            Get_MinLaserPos_B_Pos();
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 9)
        {
            Get_MinLaserPos_C_Pos();
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 10)
        {
            Get_MinLaserPos_D_Pos();
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 11)
        {
            Get_MinLaserPos_E_Pos();
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 12)
        {
            Get_MinLaserPos_F_Pos();
        }
    }

    private void Get_MinLaserPos_A_Pos()
    {
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
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_A_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_A_Range, num2);
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

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_A_End = vector3_By_AngleFlat2;
    }

    private void Get_MinLaserPos_B_Pos()
    {
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
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_B_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_B_Range, num2);
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

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_B_End = vector3_By_AngleFlat2;
    }

    private void Get_MinLaserPos_C_Pos()
    {
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
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_C_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_C_Range, num2);
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

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_C_End = vector3_By_AngleFlat2;
    }

    private void Get_MinLaserPos_D_Pos()
    {
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
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_D_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_D_Range, num2);
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

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_D_End = vector3_By_AngleFlat2;
    }

    private void Get_MinLaserPos_E_Pos()
    {
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
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_E_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_E_Range, num2);
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

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_E_End = vector3_By_AngleFlat2;
    }

    private void Get_MinLaserPos_F_Pos()
    {
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
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 6f, num);
        MinLaserPos_F_Start = MYDE_ModFront.GetVector3_By_AngleFlat(vector3_By_AngleFlat, minLaserPos_F_Range, num2);
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

        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, range, num3);
        MinLaserPos_F_End = vector3_By_AngleFlat2;
    }

    private void Do_MinLaserPos_Effect_Prepare()
    {
        if (Building_DrakkenLaserDrill.DamageNum >= 7)
        {
            Do_MinLaserPos_Effect(MinLaserPos_A_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 8)
        {
            Do_MinLaserPos_Effect(MinLaserPos_B_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 9)
        {
            Do_MinLaserPos_Effect(MinLaserPos_C_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 10)
        {
            Do_MinLaserPos_Effect(MinLaserPos_D_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 11)
        {
            Do_MinLaserPos_Effect(MinLaserPos_E_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 12)
        {
            Do_MinLaserPos_Effect(MinLaserPos_F_End);
        }
    }

    private void Do_MinLaserPos_Effect(Vector3 VPos)
    {
        var map = Map;
        var dataStatic = FleckMaker.GetDataStatic(VPos, map,
            MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_HeatGlow_Intense, MinLaser_HeatGlow_Scale);
        map.flecks.CreateFleck(dataStatic);
        for (var i = 0; i < MinLaser_Spark_Num; i++)
        {
            var num = Rand.Range(-0.3f, 0.3f);
            var loc = VPos + new Vector3(num, 0f, num);
            var scale = Rand.Range(0.5f, 1f);
            var dataStatic2 = FleckMaker.GetDataStatic(loc, Map,
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale);
            var velocityAngle = Rand.Range(-180f, 180f);
            dataStatic2.velocityAngle = velocityAngle;
            dataStatic2.velocitySpeed = Rand.Range(5f, 10f);
            Map.flecks.CreateFleck(dataStatic2);
        }
    }

    private void Draw_MinLaserPos_Prepare()
    {
        if (Building_DrakkenLaserDrill.DamageNum >= 7)
        {
            Draw_MinLaserPos(MinLaserPos_A_UpOrDown, MinLaserPos_A_Start, MinLaserPos_A_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 8)
        {
            Draw_MinLaserPos(MinLaserPos_B_UpOrDown, MinLaserPos_B_Start, MinLaserPos_B_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 9)
        {
            Draw_MinLaserPos(MinLaserPos_C_UpOrDown, MinLaserPos_C_Start, MinLaserPos_C_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 10)
        {
            Draw_MinLaserPos(MinLaserPos_D_UpOrDown, MinLaserPos_D_Start, MinLaserPos_D_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 11)
        {
            Draw_MinLaserPos(MinLaserPos_E_UpOrDown, MinLaserPos_E_Start, MinLaserPos_E_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 12)
        {
            Draw_MinLaserPos(MinLaserPos_F_UpOrDown, MinLaserPos_F_Start, MinLaserPos_F_End);
        }
    }

    private void Draw_MinLaserPos(bool UpOrDown, Vector3 Start, Vector3 End)
    {
        var incOffset = 4f;
        if (!UpOrDown)
        {
            incOffset = 2f;
        }

        var pos = (Start + End) / 2f;
        pos.y = AltitudeLayer.PawnRope.AltitudeFor(incOffset);
        var lengthHorizontal = (Start.ToIntVec3() - End.ToIntVec3()).LengthHorizontal;
        var x = MinLaser_Width;
        if (LaserScaleTick < 30)
        {
            x = 0.1f + (LaserScaleTick / 33f);
        }

        var angle = (Start - End).AngleFlat();
        var color = new Color(Building_DrakkenLaserDrill.Color_Min_Red / 255f,
            Building_DrakkenLaserDrill.Color_Min_Green / 255f, Building_DrakkenLaserDrill.Color_Min_Blue / 255f)
        {
            a = MinLaser_Alpha
        };
        var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser"),
            ShaderDatabase.Transparent, color);
        var matrix = default(Matrix4x4);
        matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
        Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
    }

    private void Draw_DecorationLaser(float AngleDeviation, float RangeDeviation, float EndRange)
    {
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
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

        var num = (drawPos - vector).AngleFlat();
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

        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, RangeDeviation, num2);
        var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, EndRange, num);
        var pos = (vector3_By_AngleFlat + vector3_By_AngleFlat2) / 2f;
        pos.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
        var lengthHorizontal = (vector3_By_AngleFlat.ToIntVec3() - vector3_By_AngleFlat2.ToIntVec3()).LengthHorizontal;
        var angle = (vector3_By_AngleFlat - vector3_By_AngleFlat2).AngleFlat();
        var color = new Color(Building_DrakkenLaserDrill.Color_Red / 255f,
            Building_DrakkenLaserDrill.Color_Green / 255f, Building_DrakkenLaserDrill.Color_Blue / 255f)
        {
            a = a
        };
        var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser"),
            ShaderDatabase.Transparent, color);
        var matrix = default(Matrix4x4);
        matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
        Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
    }
}