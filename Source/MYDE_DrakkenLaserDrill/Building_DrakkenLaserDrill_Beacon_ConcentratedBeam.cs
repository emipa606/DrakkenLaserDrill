using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Building_DrakkenLaserDrill_Beacon_ConcentratedBeam : ThingWithComps
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
    private float Angle;

    private Building_DrakkenLaserDrill Building_DrakkenLaserDrill;

    private int DestroyTick;

    private int DestroyTickMax = 180;

    private int EffectTick;

    private int EffectTickMax = 5;

    private int LaserScaleTick;

    private List<IntVec3> ListAllPos = [];

    private List<IntVec3> ListAllPos_Effect = [];

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

    private int TakeDamageTickMax = 160;

    public void CheckSpawn(Building_DrakkenLaserDrill buildingDrakkenLaserDrill, Vector2 realPos,
        List<IntVec3> listAllPos, List<IntVec3> listAllPosEffect)
    {
        Building_DrakkenLaserDrill = buildingDrakkenLaserDrill;
        RealPos = realPos;
        Building_DrakkenLaserDrill.IfImmunity = true;
        ListAllPos = listAllPos;
        ListAllPos_Effect = listAllPosEffect;
        var compPowerTrader = Building_DrakkenLaserDrill.TryGetComp<CompPowerTrader>();
        compPowerTrader.PowerOutput = 0f - (Building_DrakkenLaserDrill.Base_ConsumePowerFactor *
                                            Building_DrakkenLaserDrill.DamageNum * Building_DrakkenLaserDrill
                                                .PowerConsumeFactor_ConcentratedBeam);
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
        Scribe_Values.Look(ref DestroyTick, "DestroyTick");
        Scribe_Values.Look(ref DestroyTickMax, "DestroyTickMax");
        Scribe_Values.Look(ref RealPos, "RealPos");
        Scribe_Values.Look(ref Angle, "Angle");
        Scribe_Values.Look(ref EffectTick, "EffectTick");
        Scribe_Values.Look(ref EffectTickMax, "EffectTickMax");
        Scribe_Values.Look(ref MinLaserChangeTick, "MinLaserChangeTick");
        DeepProfiler.Start("Load All ListPos");
        Scribe_Collections.Look(ref ListAllPos, "ListAllPos", LookMode.Value);
        DeepProfiler.End();
        DeepProfiler.Start("Load All ListPosEffect");
        Scribe_Collections.Look(ref ListAllPos_Effect, "ListAllPos_Effect", LookMode.Value);
        DeepProfiler.End();
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
            var x = 2f;
            var num = 20;
            var num2 = 160;
            var num3 = 170;
            var num4 = 180;
            if (LaserScaleTick < num)
            {
                x = LaserScaleTick / 10f;
            }

            if (LaserScaleTick <= num3 && LaserScaleTick > num2)
            {
                x = 15f;
            }
            else if (LaserScaleTick <= num4 && LaserScaleTick > num3)
            {
                x = 15f - ((LaserScaleTick - num3) * 1.5f);
            }

            var num5 = (drawPos - vector).AngleFlat();
            var a = 0.8f;
            if (LaserScaleTick >= 175)
            {
                a = 0.8f - ((LaserScaleTick - 175) / 62f);
            }

            var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, 5.5f, num5);
            vector3_By_AngleFlat.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
            var vect = default(Vector3);
            for (var i = 0; i < 500; i += 50)
            {
                var vector3_By_AngleFlat2 = MYDE_ModFront.GetVector3_By_AngleFlat(vector, i, num5);
                if (vector3_By_AngleFlat2.x > Map.Size.x || vector3_By_AngleFlat2.x < 0f ||
                    vector3_By_AngleFlat2.z > Map.Size.z || vector3_By_AngleFlat2.z < 0f)
                {
                    break;
                }

                vect = vector3_By_AngleFlat2;
            }

            var lengthHorizontal = (vector3_By_AngleFlat.ToIntVec3() - vect.ToIntVec3()).LengthHorizontal;
            var num6 = 2f;
            var z = (lengthHorizontal * num6) + 100f;
            var color = new Color(Building_DrakkenLaserDrill.Color_Red / 255f,
                Building_DrakkenLaserDrill.Color_Green / 255f, Building_DrakkenLaserDrill.Color_Blue / 255f)
            {
                a = a
            };
            var material =
                MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser_Big_ConcentratedBeam"),
                    ShaderDatabase.Transparent, color);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(vector3_By_AngleFlat, Quaternion.AngleAxis(num5, Vector3.up), new Vector3(x, 1f, z));
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
            Building_DrakkenLaserDrill.GunAngle = num5;
        }

        var num7 = 65f;
        var rangeDeviation = 3.6f;
        var endRange = 7.8f;
        Draw_DecorationLaser(num7, rangeDeviation, endRange);
        Draw_DecorationLaser(0f - num7, rangeDeviation, endRange);
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
            DoEffect();
        }

        TakeDamageTick++;
        if (TakeDamageTick >= TakeDamageTickMax)
        {
            TakeDamageTick = TakeDamageTickMax - 4;
            TakeDamage();
        }

        DestroyTick++;
        if (DestroyTick == 160)
        {
            var map = Map;
            for (var i = 0; i < ListAllPos.Count; i++)
            {
                var loc = ListAllPos[i].ToVector3Shifted();
                var size = Rand.Range(0.8f, 1.5f);
                FleckMaker.ThrowSmoke(loc, map, size);
                FleckMaker.ThrowMicroSparks(loc, map);
            }
        }

        if (DestroyTick >= DestroyTickMax)
        {
            DestroyTick = 0;
            DestroyBeacon();
        }

        LaserScaleTick++;
        MinLaserChangeTick++;
        Get_MinLaserPos_Prepare();
        Building_DrakkenLaserDrill.SetBlueBarPos();
    }

    private void TakeDamage()
    {
        var map = Map;
        var list = new List<Thing>();
        for (var i = 0; i < ListAllPos.Count; i++)
        {
            list.AddRange(ListAllPos[i].GetThingList(map));
        }

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var j = 0; j < list.Count; j++)
        {
            if (list[j].Faction == Faction.OfPlayer || list[j] is Filth)
            {
                continue;
            }

            if (list[j] is not Building)
            {
                var dinfo = new DamageInfo(DamageDefOf.Cut, Building_DrakkenLaserDrill.DamageNum * 2f,
                    Building_DrakkenLaserDrill.DamageArmorPenetration, 0f, Building_DrakkenLaserDrill, null,
                    Building_DrakkenLaserDrill.def);
                list[j].TakeDamage(dinfo);
            }
            else
            {
                if (list[j] is not Building)
                {
                    continue;
                }

                if (list[j] is Mineable)
                {
                    list[j].HitPoints -= (int)(Building_DrakkenLaserDrill.DamageNum * 10f);
                    if (list[j].HitPoints > 0)
                    {
                        continue;
                    }

                    var mineable = list[j] as Mineable;
                    if (mineable?.def.building.mineableThing != null)
                    {
                        var num = Mathf.Max(1, mineable.def.building.EffectiveMineableYield);
                        var thing = ThingMaker.MakeThing(mineable.def.building.mineableThing);
                        thing.stackCount = num;
                        GenPlace.TryPlaceThing(thing, mineable.Position, Map, ThingPlaceMode.Near, null, null,
                            default(Rot4));
                    }

                    mineable?.Destroy(DestroyMode.KillFinalize);
                }
                else
                {
                    var dinfo2 = new DamageInfo(DamageDefOf.Cut, Building_DrakkenLaserDrill.DamageNum * 10f,
                        Building_DrakkenLaserDrill.DamageArmorPenetration, 0f, Building_DrakkenLaserDrill, null,
                        Building_DrakkenLaserDrill.def);
                    list[j].TakeDamage(dinfo2);
                }
            }
        }
    }

    private void DestroyBeacon()
    {
        Destroy();
    }

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_ConcentratedBeam = null;
        Building_DrakkenLaserDrill.IfImmunity = false;
        Building_DrakkenLaserDrill.ChangePowerConsumeToZero();
        Building_DrakkenLaserDrill.TryGetComp<Comp_DrakkenLaserDrill_AutoAttack>().PrepareToAttack();
        base.Destroy(mode);
    }

    private void DoEffect()
    {
        var map = Map;
        var vector3_By_AngleFlat = MYDE_ModFront.GetVector3_By_AngleFlat(Building_DrakkenLaserDrill.DrawPos, 7.8f,
            Building_DrakkenLaserDrill.GunAngle);
        var dataStatic = FleckMaker.GetDataStatic(vector3_By_AngleFlat, map,
            MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_HeatGlow_Intense, 3f);
        map.flecks.CreateFleck(dataStatic);
        FleckMaker.ThrowMicroSparks(vector3_By_AngleFlat, Map);
        FleckMaker.ThrowLightningGlow(vector3_By_AngleFlat, Map, 1.5f);
        var num = (vector3_By_AngleFlat - Building_DrakkenLaserDrill.DrawPos).AngleFlat();
        for (var i = 0; i < 4; i++)
        {
            var num2 = Rand.Range(-0.3f, 0.3f);
            var loc = vector3_By_AngleFlat + new Vector3(num2, 0f, num2);
            var scale = Rand.Range(0.5f, 0.8f);
            var dataStatic2 = FleckMaker.GetDataStatic(loc, Map,
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
            Map.flecks.CreateFleck(dataStatic2);
        }

        var num4 = 1 + (DestroyTick / 5);
        var r = Building_DrakkenLaserDrill.Color_Red / 255f;
        var g = Building_DrakkenLaserDrill.Color_Green / 255f;
        var b = Building_DrakkenLaserDrill.Color_Blue / 255f;
        for (var j = 0; j <= num4; j++)
        {
            var num5 = Rand.Range(-0.5f, 0.5f);
            var vector = ListAllPos_Effect.RandomElement().ToVector3Shifted();
            var loc2 = vector + new Vector3(num5, 0f, num5);
            var scale2 = Rand.Range(1f, 1.5f);
            var dataStatic3 = FleckMaker.GetDataStatic(loc2, Map,
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_ConcentratedBeam_Spark, scale2);
            dataStatic3.rotation = num + 90f;
            dataStatic3.velocityAngle = num;
            dataStatic3.velocitySpeed = Rand.Range(40f, 60f);
            dataStatic3.instanceColor = new Color(r, g, b);
            dataStatic3.def.solidTime = 0.6f;
            if (DestroyTick >= 120)
            {
                dataStatic3.def.solidTime = 0.6f - ((DestroyTick - 120f) / 100f);
            }

            Map.flecks.CreateFleck(dataStatic3);
        }

        var r2 = Building_DrakkenLaserDrill.Color_Min_Red / 255f;
        var g2 = Building_DrakkenLaserDrill.Color_Min_Green / 255f;
        var b2 = Building_DrakkenLaserDrill.Color_Min_Blue / 255f;
        for (var k = 0; k <= num4; k++)
        {
            var num6 = Rand.Range(-0.3f, 0.3f);
            var vector2 = ListAllPos_Effect.RandomElement().ToVector3();
            var loc3 = vector2 + new Vector3(num6, 0f, num6);
            var scale3 = Rand.Range(1f, 1.5f);
            var dataStatic4 = FleckMaker.GetDataStatic(loc3, Map,
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_ConcentratedBeam_Spark, scale3);
            dataStatic4.rotation = num + 90f;
            dataStatic4.velocityAngle = num;
            dataStatic4.velocitySpeed = Rand.Range(40f, 60f);
            dataStatic4.instanceColor = new Color(r2, g2, b2);
            dataStatic4.def.solidTime = 0.6f;
            if (DestroyTick >= 120)
            {
                dataStatic4.def.solidTime = 0.6f - ((DestroyTick - 120f) / 100f);
            }

            Map.flecks.CreateFleck(dataStatic4);
        }

        var compPowerTrader = Building_DrakkenLaserDrill.TryGetComp<CompPowerTrader>();
        compPowerTrader.PowerOutput = 0f - (Building_DrakkenLaserDrill.Base_ConsumePowerFactor *
                                            Building_DrakkenLaserDrill.DamageNum * Building_DrakkenLaserDrill
                                                .PowerConsumeFactor_ConcentratedBeam);
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
            if (vector3_By_AngleFlat2.x > Map.Size.x || vector3_By_AngleFlat2.x < 0f ||
                vector3_By_AngleFlat2.z > Map.Size.z || vector3_By_AngleFlat2.z < 0f)
            {
                break;
            }

            vect = vector3_By_AngleFlat2;
        }

        var lengthHorizontal = (vector3_By_AngleFlat.ToIntVec3() - vect.ToIntVec3()).LengthHorizontal;
        var num5 = 2f;
        var z = (lengthHorizontal * num5) + 100f;
        var color = new Color(Building_DrakkenLaserDrill.Color_Min_Red / 255f,
            Building_DrakkenLaserDrill.Color_Min_Green / 255f, Building_DrakkenLaserDrill.Color_Min_Blue / 255f)
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