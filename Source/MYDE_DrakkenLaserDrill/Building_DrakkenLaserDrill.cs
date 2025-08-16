using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Building_DrakkenLaserDrill : Building
{
    private static readonly Mesh Plane_Scale_130 = MeshMakerPlanes.NewPlaneMesh(13f);

    private readonly int AddColdDownTickMax = 60;

    public readonly float PowerConsumeFactor_ConcentratedBeam = 6f;

    public readonly float PowerConsumeFactor_PulseCannon = 10f;

    public bool AddColdDownBool;

    private int AddColdDownTick;

    public float Base_ConsumePowerFactor = MYDE_DrakkenLaserDrill_Setting.Base_ConsumePowerFactor;
    public Building_DrakkenLaserDrill_Beacon Building_DrakkenLaserDrill_Beacon;

    public Building_DrakkenLaserDrill_Beacon_ConcentratedBeam Building_DrakkenLaserDrill_Beacon_ConcentratedBeam;

    public Building_DrakkenLaserDrill_Beacon_ConcentratedBeam_CrossMap
        Building_DrakkenLaserDrill_Beacon_ConcentratedBeam_CrossMap;

    public Building_DrakkenLaserDrill_Beacon_CrossMap Building_DrakkenLaserDrill_Beacon_CrossMap;

    public Building_DrakkenLaserDrill_Beacon_Mouse Building_DrakkenLaserDrill_Beacon_Mouse;

    public Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap;

    public Building_DrakkenLaserDrill_Beacon_PulseCannon Building_DrakkenLaserDrill_Beacon_PulseCannon;

    public Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap
        Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap;

    public float Color_Blue = 3f;

    public float Color_Green = 234f;

    public float Color_Min_Blue = 3f;

    public float Color_Min_Green = 128f;

    public float Color_Min_Red = 255f;

    public float Color_Red = 255f;

    public float ConcentratedBeam_EnergyAccumulation;

    public float ConcentratedBeam_EnergyAccumulationMax = 3000f;

    public float DamageArmorPenetration = 0.6f;

    public float DamageArmorPenetrationMax = 0.6f;

    public int DamageNum = 6;

    public int DamageNumMax = 6;

    public float GunAngle;

    public bool IfAttackDown = true;

    public bool IfCrossMap;

    public bool IfImmunity;

    public bool Now_Rebuilding;

    private Vector3 Power_Center_BlueBar;

    public float PulseCannon_EnergyAccumulation;

    public float PulseCannon_EnergyAccumulationMax = 5000f;

    private int RebuildCheckTick;

    private int RebuildCheckTickMax = 600;

    public float StoredEnergyMax = 6000f;

    public Thing TargetThing;

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        SetBlueBarPos();
        ChangePowerConsumeToZero();
        Set_StoredEnergyMax();
        Check_Highest_DamageNumMax();
        Check_Highest_DamageArmorPenetrationMax();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref Building_DrakkenLaserDrill_Beacon, "Building_DrakkenLaserDrill_Beacon");
        Scribe_References.Look(ref Building_DrakkenLaserDrill_Beacon_CrossMap,
            "Building_DrakkenLaserDrill_Beacon_CrossMap");
        Scribe_References.Look(ref Building_DrakkenLaserDrill_Beacon_Mouse, "Building_DrakkenLaserDrill_Beacon_Mouse");
        Scribe_References.Look(ref Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap,
            "Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap");
        Scribe_References.Look(ref Building_DrakkenLaserDrill_Beacon_PulseCannon,
            "Building_DrakkenLaserDrill_Beacon_PulseCannon");
        Scribe_Values.Look(ref PulseCannon_EnergyAccumulation, "PulseCannon_EnergyAccumulation");
        Scribe_Values.Look(ref PulseCannon_EnergyAccumulationMax, "PulseCannon_EnergyAccumulationMax");
        Scribe_References.Look(ref Building_DrakkenLaserDrill_Beacon_ConcentratedBeam,
            "Building_DrakkenLaserDrill_Beacon_ConcentratedBeam");
        Scribe_References.Look(ref Building_DrakkenLaserDrill_Beacon_ConcentratedBeam_CrossMap,
            "Building_DrakkenLaserDrill_Beacon_ConcentratedBeam_CrossMap");
        Scribe_Values.Look(ref ConcentratedBeam_EnergyAccumulation, "ConcentratedBeam_EnergyAccumulation");
        Scribe_Values.Look(ref ConcentratedBeam_EnergyAccumulationMax, "ConcentratedBeam_EnergyAccumulationMax");
        Scribe_References.Look(ref TargetThing, "TargetThing");
        Scribe_Values.Look(ref Now_Rebuilding, "Now_Rebuilding");
        Scribe_Values.Look(ref RebuildCheckTick, "RebuildCheckTick");
        Scribe_Values.Look(ref RebuildCheckTickMax, "RebuildCheckTickMax");
        Scribe_Values.Look(ref IfImmunity, "IfImmunity");
        Scribe_Values.Look(ref GunAngle, "GunAngle");
        Scribe_Values.Look(ref IfAttackDown, "IfAttackDown");
        Scribe_Values.Look(ref IfCrossMap, "IfCrossMap");
        Scribe_Values.Look(ref DamageNum, "DamageNum");
        Scribe_Values.Look(ref DamageNumMax, "DamageNumMax");
        Scribe_Values.Look(ref DamageArmorPenetration, "DamageArmorPenetration");
        Scribe_Values.Look(ref DamageArmorPenetrationMax, "DamageArmorPenetrationMax");
        Scribe_Values.Look(ref StoredEnergyMax, "StoredEnergyMax");
        Scribe_Values.Look(ref Base_ConsumePowerFactor, "Base_ConsumePowerFactor");
        Scribe_Values.Look(ref Color_Red, "Color_Red");
        Scribe_Values.Look(ref Color_Green, "Color_Green");
        Scribe_Values.Look(ref Color_Blue, "Color_Blue");
        Scribe_Values.Look(ref Color_Min_Red, "Color_Min_Red");
        Scribe_Values.Look(ref Color_Min_Green, "Color_Min_Green");
        Scribe_Values.Look(ref Color_Min_Blue, "Color_Min_Blue");
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        base.DrawAt(drawLoc, flip);
        var power_Center_BlueBar = Power_Center_BlueBar;
        power_Center_BlueBar.y = AltitudeLayer.PawnRope.AltitudeFor(5.1f);
        var material = MaterialPool.MatFrom("DrakkenLaserDrill_Building/Blue", ShaderDatabase.Transparent);
        var compPowerBattery = this.TryGetComp<CompPowerBattery>();
        var storedEnergyPct = compPowerBattery.StoredEnergyPct;
        var matrix = default(Matrix4x4);
        matrix.SetTRS(power_Center_BlueBar, Quaternion.AngleAxis(GunAngle, Vector3.up),
            new Vector3(1f, 1f, storedEnergyPct));
        Graphics.DrawMesh(Plane_Scale_130, matrix, material, 0);
        if (!Now_Rebuilding)
        {
            var drawPos = DrawPos;
            drawPos.y = AltitudeLayer.PawnRope.AltitudeFor(5f);
            var material2 = MaterialPool.MatFrom("DrakkenLaserDrill_Building/LaserDrill", ShaderDatabase.Transparent);
            var matrix2 = default(Matrix4x4);
            matrix2.SetTRS(drawPos, Quaternion.AngleAxis(GunAngle, Vector3.up), new Vector3(13f, 1f, 13f));
            Graphics.DrawMesh(MeshPool.plane10, matrix2, material2, 0);
        }
        else if (Now_Rebuilding)
        {
            var drawPos2 = DrawPos;
            drawPos2.y = AltitudeLayer.PawnRope.AltitudeFor(5f);
            var material3 =
                MaterialPool.MatFrom("DrakkenLaserDrill_Building/LaserDrill_Close", ShaderDatabase.Transparent);
            var matrix3 = default(Matrix4x4);
            matrix3.SetTRS(drawPos2, Quaternion.AngleAxis(GunAngle, Vector3.up), new Vector3(13f, 1f, 13f));
            Graphics.DrawMesh(MeshPool.plane10, matrix3, material3, 0);
        }
    }

    protected override void Tick()
    {
        base.Tick();
        RebuildCheckTick++;
        if (RebuildCheckTick >= RebuildCheckTickMax)
        {
            RebuildCheckTick = 0;
            if (Now_Rebuilding && HitPoints == MaxHitPoints)
            {
                Now_Rebuilding = false;
            }

            Base_ConsumePowerFactor = MYDE_DrakkenLaserDrill_Setting.Base_ConsumePowerFactor;
        }

        if (AddColdDownBool)
        {
            AddColdDownTick++;
            if (AddColdDownTick >= AddColdDownTickMax)
            {
                AddColdDownTick = 0;
                AddColdDownBool = false;
            }
        }

        Add_ConcentratedBeam_EnergyAccumulation();
        Add_PulseCannon_EnergyAccumulation();
    }

    public void DamageTarget()
    {
        if (!Now_Rebuilding)
        {
            var map = TargetThing.Map;
            if (TargetThing is Pawn pawn)
            {
                if (pawn.Dead)
                {
                    Building_DrakkenLaserDrill_Beacon.FindNextTarget(pawn);
                    TargetThing = null;
                    ChangePowerConsumeToZero();
                    return;
                }

                if (pawn.Downed && !IfAttackDown)
                {
                    Building_DrakkenLaserDrill_Beacon.FindNextTarget(TargetThing);
                    TargetThing = null;
                    ChangePowerConsumeToZero();
                    return;
                }
            }
            else if (TargetThing is Building && TargetThing.Destroyed)
            {
                Building_DrakkenLaserDrill_Beacon.FindNextTarget(TargetThing);
                TargetThing = null;
                ChangePowerConsumeToZero();
                return;
            }

            var compPowerTrader = this.TryGetComp<CompPowerTrader>();
            compPowerTrader.PowerOutput = 0f - (Base_ConsumePowerFactor * DamageNum);
            if (compPowerTrader.PowerOn)
            {
                if (TargetThing is Pawn)
                {
                    var dinfo = new DamageInfo(DamageDefOf.Cut, DamageNum, DamageArmorPenetration, 0f, this, null, def);
                    TargetThing.TakeDamage(dinfo);
                }
                else if (TargetThing is Building)
                {
                    if (TargetThing is Mineable mineable)
                    {
                        mineable.HitPoints -= (int)(DamageNum * 5f);
                        if (mineable.HitPoints <= 0)
                        {
                            if (mineable.def.building.mineableThing != null)
                            {
                                var num = Mathf.Max(1, mineable.def.building.EffectiveMineableYield);
                                if (def.building.mineableYieldWasteable)
                                {
                                    num = Mathf.Max(1, GenMath.RoundRandom(num));
                                }

                                var thing = ThingMaker.MakeThing(mineable.def.building.mineableThing);
                                thing.stackCount = num;
                                GenPlace.TryPlaceThing(thing, mineable.Position, map, ThingPlaceMode.Near, null, null,
                                    default(Rot4));
                            }

                            mineable.Destroy(DestroyMode.KillFinalize);
                        }
                    }
                    else
                    {
                        var dinfo2 = new DamageInfo(DamageDefOf.Cut, DamageNum * 5f, DamageArmorPenetration, 0f, this,
                            null, def);
                        TargetThing.TakeDamage(dinfo2);
                    }
                }
            }
            else if (!compPowerTrader.PowerOn)
            {
                DestroyAllBeacon();
            }

            FleckMaker.ThrowMicroSparks(TargetThing.DrawPos, map);
            FleckMaker.ThrowLightningGlow(TargetThing.DrawPos, map, 1.5f);
            var num2 = (TargetThing.DrawPos - DrawPos).AngleFlat();
            for (var i = 0; i < 5; i++)
            {
                var num3 = Rand.Range(-0.3f, 0.3f);
                var loc = TargetThing.DrawPos + new Vector3(num3, 0f, num3);
                var scale = Rand.Range(0.5f, 1f);
                var dataStatic = FleckMaker.GetDataStatic(loc, map,
                    MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale);
                var num4 = num2 + Rand.Range(-30f, 30f);
                if (num4 > 180f)
                {
                    num4 = num4 - 180f + -180f;
                }

                if (num4 < -180f)
                {
                    num4 = num4 + 180f + 180f;
                }

                dataStatic.velocityAngle = num4;
                dataStatic.velocitySpeed = Rand.Range(5f, 10f);
                map.flecks.CreateFleck(dataStatic);
            }
        }
        else if (Now_Rebuilding)
        {
            DestroyAllBeacon();
        }
    }

    public void DamagePos()
    {
        if (!Now_Rebuilding)
        {
            var map = Map;
            var num = 3;
            var compPowerTrader = this.TryGetComp<CompPowerTrader>();
            compPowerTrader.PowerOutput = 0f - (Base_ConsumePowerFactor * DamageNum * num);
            if (compPowerTrader.PowerOn)
            {
                var list = new List<Thing>();
                var pos_Square = MYDE_ModFront.GetPos_Square(Building_DrakkenLaserDrill_Beacon_Mouse.Position, 1, 1);
                for (var i = 0; i < pos_Square.Count; i++)
                {
                    list.AddRange(pos_Square[i].GetThingList(map));
                }

                // ReSharper disable once ForCanBeConvertedToForeach
                for (var j = 0; j < list.Count; j++)
                {
                    if (list[j] is not Building)
                    {
                        var dinfo = new DamageInfo(DamageDefOf.Cut, DamageNum, DamageArmorPenetration, 0f, this, null,
                            def);
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
                            list[j].HitPoints -= (int)(DamageNum * 5f);
                            if (list[j].HitPoints > 0)
                            {
                                continue;
                            }

                            var mineable = list[j] as Mineable;
                            if (mineable?.def.building.mineableThing != null)
                            {
                                var num2 = Mathf.Max(1, mineable.def.building.EffectiveMineableYield);
                                if (def.building.mineableYieldWasteable)
                                {
                                    num2 = Mathf.Max(1, GenMath.RoundRandom(num2));
                                }

                                var thing = ThingMaker.MakeThing(mineable.def.building.mineableThing);
                                thing.stackCount = num2;
                                GenPlace.TryPlaceThing(thing, mineable.Position, Map, ThingPlaceMode.Near, null, null,
                                    default(Rot4));
                            }

                            mineable?.Destroy(DestroyMode.KillFinalize);
                        }
                        else
                        {
                            var dinfo2 = new DamageInfo(DamageDefOf.Cut, DamageNum * 5f, DamageArmorPenetration, 0f,
                                this, null, def);
                            list[j].TakeDamage(dinfo2);
                        }
                    }
                }
            }
            else if (!compPowerTrader.PowerOn)
            {
                DestroyAllBeacon();
            }

            FleckMaker.ThrowMicroSparks(Building_DrakkenLaserDrill_Beacon_Mouse.DrawPos, Map);
            FleckMaker.ThrowLightningGlow(Building_DrakkenLaserDrill_Beacon_Mouse.DrawPos, Map, 1.5f);
            for (var k = 0; k < 10; k++)
            {
                var num3 = Rand.Range(-0.3f, 0.3f);
                var loc = Building_DrakkenLaserDrill_Beacon_Mouse.DrawPos + new Vector3(num3, 0f, num3);
                var scale = Rand.Range(0.5f, 1f);
                var dataStatic = FleckMaker.GetDataStatic(loc, Map,
                    MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale);
                var velocityAngle = Rand.Range(-180f, 180f);
                dataStatic.velocityAngle = velocityAngle;
                dataStatic.velocitySpeed = Rand.Range(5f, 10f);
                Map.flecks.CreateFleck(dataStatic);
            }
        }
        else if (Now_Rebuilding)
        {
            DestroyAllBeacon();
        }
    }

    public void DamageTarget_CrossMap()
    {
        if (!Now_Rebuilding)
        {
            var map = TargetThing.Map;
            if (TargetThing is Pawn pawn)
            {
                if (pawn.Dead)
                {
                    Building_DrakkenLaserDrill_Beacon_CrossMap.FindNextTarget(pawn);
                    TargetThing = null;
                    ChangePowerConsumeToZero();
                    return;
                }

                if (pawn.Downed && !IfAttackDown)
                {
                    Building_DrakkenLaserDrill_Beacon_CrossMap.FindNextTarget(TargetThing);
                    TargetThing = null;
                    ChangePowerConsumeToZero();
                    return;
                }
            }
            else if (TargetThing is Building && TargetThing.Destroyed)
            {
                Building_DrakkenLaserDrill_Beacon_CrossMap.FindNextTarget(TargetThing);
                TargetThing = null;
                ChangePowerConsumeToZero();
                return;
            }

            var compPowerTrader = this.TryGetComp<CompPowerTrader>();
            compPowerTrader.PowerOutput = 0f - (Base_ConsumePowerFactor * DamageNum);
            if (compPowerTrader.PowerOn)
            {
                if (TargetThing is Pawn)
                {
                    var dinfo = new DamageInfo(DamageDefOf.Cut, DamageNum, DamageArmorPenetration, 0f, this, null, def);
                    TargetThing.TakeDamage(dinfo);
                }
                else if (TargetThing is Building)
                {
                    if (TargetThing is Mineable mineable)
                    {
                        mineable.HitPoints -= (int)(DamageNum * 5f);
                        if (mineable.HitPoints <= 0)
                        {
                            if (mineable.def.building.mineableThing != null)
                            {
                                var num = Mathf.Max(1, mineable.def.building.EffectiveMineableYield);
                                if (def.building.mineableYieldWasteable)
                                {
                                    num = Mathf.Max(1, GenMath.RoundRandom(num));
                                }

                                var thing = ThingMaker.MakeThing(mineable.def.building.mineableThing);
                                thing.stackCount = num;
                                GenPlace.TryPlaceThing(thing, mineable.Position, map, ThingPlaceMode.Near, null, null,
                                    default(Rot4));
                            }

                            mineable.Destroy(DestroyMode.KillFinalize);
                        }
                    }
                    else
                    {
                        var dinfo2 = new DamageInfo(DamageDefOf.Cut, DamageNum * 5f, DamageArmorPenetration, 0f, this,
                            null, def);
                        TargetThing.TakeDamage(dinfo2);
                    }
                }
            }
            else if (!compPowerTrader.PowerOn)
            {
                DestroyAllBeacon();
            }

            FleckMaker.ThrowMicroSparks(TargetThing.DrawPos, map);
            FleckMaker.ThrowLightningGlow(TargetThing.DrawPos, map, 1.5f);
            var num2 = (TargetThing.DrawPos - DrawPos).AngleFlat();
            for (var i = 0; i < 5; i++)
            {
                var num3 = Rand.Range(-0.3f, 0.3f);
                var loc = TargetThing.DrawPos + new Vector3(num3, 0f, num3);
                var scale = Rand.Range(0.5f, 1f);
                var dataStatic = FleckMaker.GetDataStatic(loc, map,
                    MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale);
                var num4 = num2 + Rand.Range(-30f, 30f);
                if (num4 > 180f)
                {
                    num4 = num4 - 180f + -180f;
                }

                if (num4 < -180f)
                {
                    num4 = num4 + 180f + 180f;
                }

                dataStatic.velocityAngle = num4;
                dataStatic.velocitySpeed = Rand.Range(5f, 10f);
                map.flecks.CreateFleck(dataStatic);
            }
        }
        else if (Now_Rebuilding)
        {
            DestroyAllBeacon();
        }
    }

    public void DamagePos_CrossMap()
    {
        if (!Now_Rebuilding)
        {
            var map = Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.Map;
            var num = 3;
            var compPowerTrader = this.TryGetComp<CompPowerTrader>();
            compPowerTrader.PowerOutput = 0f - (Base_ConsumePowerFactor * DamageNum * num);
            if (compPowerTrader.PowerOn)
            {
                var list = new List<Thing>();
                var pos_Square =
                    MYDE_ModFront.GetPos_Square(Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.Position, 1, 1);
                for (var i = 0; i < pos_Square.Count; i++)
                {
                    list.AddRange(pos_Square[i].GetThingList(map));
                }

                // ReSharper disable once ForCanBeConvertedToForeach
                for (var j = 0; j < list.Count; j++)
                {
                    if (list[j] is not Building)
                    {
                        var dinfo = new DamageInfo(DamageDefOf.Cut, DamageNum, DamageArmorPenetration, 0f, this, null,
                            def);
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
                            list[j].HitPoints -= (int)(DamageNum * 5f);
                            if (list[j].HitPoints > 0)
                            {
                                continue;
                            }

                            var mineable = list[j] as Mineable;
                            if (mineable?.def.building.mineableThing != null)
                            {
                                var num2 = Mathf.Max(1, mineable.def.building.EffectiveMineableYield);
                                if (def.building.mineableYieldWasteable)
                                {
                                    num2 = Mathf.Max(1, GenMath.RoundRandom(num2));
                                }

                                var thing = ThingMaker.MakeThing(mineable.def.building.mineableThing);
                                thing.stackCount = num2;
                                GenPlace.TryPlaceThing(thing, mineable.Position, Map, ThingPlaceMode.Near, null, null,
                                    default(Rot4));
                            }

                            mineable?.Destroy(DestroyMode.KillFinalize);
                        }
                        else
                        {
                            var dinfo2 = new DamageInfo(DamageDefOf.Cut, DamageNum * 5f, DamageArmorPenetration, 0f,
                                this, null, def);
                            list[j].TakeDamage(dinfo2);
                        }
                    }
                }
            }
            else if (!compPowerTrader.PowerOn)
            {
                DestroyAllBeacon();
            }

            FleckMaker.ThrowMicroSparks(Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.DrawPos, map);
            FleckMaker.ThrowLightningGlow(Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.DrawPos, map, 1.5f);
            for (var k = 0; k < 10; k++)
            {
                var num3 = Rand.Range(-0.3f, 0.3f);
                var loc = Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.DrawPos + new Vector3(num3, 0f, num3);
                var scale = Rand.Range(0.5f, 1f);
                var dataStatic = FleckMaker.GetDataStatic(loc, map,
                    MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_Spark, scale);
                var velocityAngle = Rand.Range(-180f, 180f);
                dataStatic.velocityAngle = velocityAngle;
                dataStatic.velocitySpeed = Rand.Range(5f, 10f);
                map.flecks.CreateFleck(dataStatic);
            }
        }
        else if (Now_Rebuilding)
        {
            DestroyAllBeacon();
        }
    }

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        if (HitPoints <= 1 && IfImmunity)
        {
            HitPoints = 1;
        }
        else if (HitPoints <= 1)
        {
            Now_Rebuilding = true;
            HitPoints = 1;
        }
        else
        {
            base.Destroy(mode);
        }
    }

    public void DestroyAllBeacon()
    {
        ChangePowerConsumeToZero();
        if (this.TryGetComp<Comp_DrakkenLaserDrill_AutoAttack>().IfEnemyCome)
        {
            this.TryGetComp<Comp_DrakkenLaserDrill_AutoAttack>().IfEnemyCome = false;
        }

        if (Building_DrakkenLaserDrill_Beacon != null)
        {
            Building_DrakkenLaserDrill_Beacon.Destroy();
            Building_DrakkenLaserDrill_Beacon = null;
        }

        if (Building_DrakkenLaserDrill_Beacon_CrossMap != null)
        {
            Building_DrakkenLaserDrill_Beacon_CrossMap.Destroy();
            Building_DrakkenLaserDrill_Beacon_CrossMap = null;
        }

        if (Building_DrakkenLaserDrill_Beacon_Mouse != null)
        {
            Building_DrakkenLaserDrill_Beacon_Mouse.Destroy();
            Building_DrakkenLaserDrill_Beacon_Mouse = null;
        }

        if (Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap == null)
        {
            return;
        }

        Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap.Destroy();
        Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap = null;
    }

    public void ChangePowerConsumeToZero()
    {
        var compPowerTrader = this.TryGetComp<CompPowerTrader>();
        compPowerTrader.PowerOutput = 0f;
    }

    public void SetBlueBarPos()
    {
        Power_Center_BlueBar = MYDE_ModFront.GetVector3_By_AngleFlat(DrawPos, -0.68f, GunAngle);
    }

    public void Set_StoredEnergyMax()
    {
        var list = Map.listerThings.ThingsOfDef(MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill);
        if (list.Count <= 0)
        {
            return;
        }

        foreach (var thing in list)
        {
            if (thing is Building_DrakkenLaserDrill building_DrakkenLaserDrill &&
                building_DrakkenLaserDrill.StoredEnergyMax >= StoredEnergyMax)
            {
                StoredEnergyMax = building_DrakkenLaserDrill.StoredEnergyMax;
            }
        }

        var compPowerBattery = this.TryGetComp<CompPowerBattery>();
        compPowerBattery.Props.storedEnergyMax = StoredEnergyMax;
    }

    public void Check_Highest_DamageNumMax()
    {
        var list = Map.listerThings.ThingsOfDef(MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill);
        if (list.Count <= 0)
        {
            return;
        }

        foreach (var thing in list)
        {
            if (thing is Building_DrakkenLaserDrill building_DrakkenLaserDrill &&
                building_DrakkenLaserDrill.DamageNumMax >= DamageNumMax)
            {
                DamageNumMax = building_DrakkenLaserDrill.DamageNumMax;
            }
        }
    }

    public void Check_Highest_DamageArmorPenetrationMax()
    {
        var list = Map.listerThings.ThingsOfDef(MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill);
        if (list.Count > 0)
        {
            foreach (var thing in list)
            {
                if (thing is Building_DrakkenLaserDrill building_DrakkenLaserDrill &&
                    building_DrakkenLaserDrill.DamageArmorPenetrationMax >= DamageArmorPenetrationMax)
                {
                    DamageArmorPenetrationMax = building_DrakkenLaserDrill.DamageArmorPenetrationMax;
                }
            }
        }

        DamageArmorPenetration = DamageArmorPenetrationMax;
    }

    private void Add_PulseCannon_EnergyAccumulation()
    {
        var compPowerTrader = this.TryGetComp<CompPowerTrader>();
        if (!(compPowerTrader.PowerOutput < 0f) || IfImmunity)
        {
            return;
        }

        PulseCannon_EnergyAccumulation +=
            0f - (compPowerTrader.PowerOutput * CompPower.WattsToWattDaysPerTick / 1f);
        if (PulseCannon_EnergyAccumulation > PulseCannon_EnergyAccumulationMax)
        {
            PulseCannon_EnergyAccumulation = PulseCannon_EnergyAccumulationMax;
        }
    }

    private void Add_ConcentratedBeam_EnergyAccumulation()
    {
        var compPowerTrader = this.TryGetComp<CompPowerTrader>();
        if (!(compPowerTrader.PowerOutput < 0f) || IfImmunity)
        {
            return;
        }

        ConcentratedBeam_EnergyAccumulation +=
            0f - (compPowerTrader.PowerOutput * CompPower.WattsToWattDaysPerTick / 1f);
        if (ConcentratedBeam_EnergyAccumulation > ConcentratedBeam_EnergyAccumulationMax)
        {
            ConcentratedBeam_EnergyAccumulation = ConcentratedBeam_EnergyAccumulationMax;
        }
    }
}