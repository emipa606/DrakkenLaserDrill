using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_ConcentratedBeam : ThingComp
{
    private Texture2D Building_DrakkenLaserDrill_ConcentratedBeam_Icon;
    private string Building_DrakkenLaserDrill_ConcentratedBeam_Label;

    public CompProperties_DrakkenLaserDrill_ConcentratedBeam Props =>
        props as CompProperties_DrakkenLaserDrill_ConcentratedBeam;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        var ResearchProject =
            DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find(x =>
                x.defName == "MYDE_DrakkenLaserDrill_Research_ConcentratedBeam");
        var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (Building_DrakkenLaserDrill is { IfCrossMap: true })
        {
            yield break;
        }

        if (ResearchProject.IsFinished)
        {
            if (Building_DrakkenLaserDrill != null)
            {
                var PowerConsumeNum = Building_DrakkenLaserDrill.Base_ConsumePowerFactor *
                                      Building_DrakkenLaserDrill.DamageNum *
                                      Building_DrakkenLaserDrill.PowerConsumeFactor_ConcentratedBeam;
                _ = PowerConsumeNum * CompPower.WattsToWattDaysPerTick * 180f;
            }

            yield return new Command_Action
            {
                action = DoSomething,
                defaultLabel = Building_DrakkenLaserDrill_ConcentratedBeam_Label,
                icon = Building_DrakkenLaserDrill_ConcentratedBeam_Icon,
                defaultDesc = "DrakkenLaserDrill_ConcentratedBeam_Desc".Translate()
            };
        }

        if (DebugSettings.ShowDevGizmos)
        {
            yield return new Command_Action
            {
                defaultLabel = "Max：ConcentratedBeam",
                action = delegate
                {
                    if (Building_DrakkenLaserDrill != null)
                    {
                        Building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulation =
                            Building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulationMax;
                    }
                }
            };
        }
    }

    private void DoSomething()
    {
        var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        var compPower = Building_DrakkenLaserDrill.TryGetComp<CompPower>();
        if (Building_DrakkenLaserDrill != null)
        {
            var num = Building_DrakkenLaserDrill.Base_ConsumePowerFactor * Building_DrakkenLaserDrill.DamageNum *
                      Building_DrakkenLaserDrill.PowerConsumeFactor_ConcentratedBeam;
            var num2 = num * CompPower.WattsToWattDaysPerTick * 180f;
            var num3 = compPower.PowerNet.CurrentStoredEnergy();
            if (!(num2 < num3) ||
                !(Building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulation >=
                  Building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulationMax) ||
                Building_DrakkenLaserDrill.IfImmunity)
            {
                return;
            }
        }

        var Map = parent.Map;
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
            validator = target => target.IsValid && target.Cell.InBounds(Map)
        };
        Find.Targeter.BeginTargeting(targetingParameters, delegate(LocalTargetInfo Target)
        {
            var drawPos = Building_DrakkenLaserDrill.DrawPos;
            var centerVector = Target.CenterVector3;
            var angle = (drawPos - centerVector).AngleFlat();
            var list = new List<IntVec3>();
            for (var i = 0; i < 500; i += 2)
            {
                var vector = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, angle);
                if (vector.x > Map.Size.x || vector.x < 0f || vector.z > Map.Size.z || vector.z < 0f)
                {
                    break;
                }

                list.Add(vector.ToIntVec3());
            }

            var list2 = new List<IntVec3>();
            foreach (var targetPos in list)
            {
                var pos_Square = MYDE_ModFront.GetPos_Square(targetPos, 2, 2);
                foreach (var intVec3 in pos_Square)
                {
                    if (!list2.Contains(intVec3))
                    {
                        list2.Add(intVec3);
                    }
                }
            }

            var list3 = new List<IntVec3>();
            foreach (var targetPos in list)
            {
                var pos_Square2 = MYDE_ModFront.GetPos_Square(targetPos, 1, 1);
                foreach (var intVec3 in pos_Square2)
                {
                    if (!list3.Contains(intVec3))
                    {
                        list3.Add(intVec3);
                    }
                }
            }

            var cell = Target.Cell;
            var realPos = new Vector2(Target.CenterVector3.x, Target.CenterVector3.z);
            var building_DrakkenLaserDrill_Beacon_ConcentratedBeam =
                (Building_DrakkenLaserDrill_Beacon_ConcentratedBeam)ThingMaker.MakeThing(MYDE_ThingDefOf
                    .MYDE_Building_DrakkenLaserDrill_Beacon_ConcentratedBeam);
            ((Building_DrakkenLaserDrill_Beacon_ConcentratedBeam)GenSpawn.Spawn(
                    building_DrakkenLaserDrill_Beacon_ConcentratedBeam, cell, Map))
                .CheckSpawn(Building_DrakkenLaserDrill, realPos, list2, list3);
            Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_ConcentratedBeam =
                building_DrakkenLaserDrill_Beacon_ConcentratedBeam;
            Building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulation = 0f;
        }, delegate(LocalTargetInfo Target)
        {
            var drawPos = Building_DrakkenLaserDrill.DrawPos;
            var centerVector = Target.CenterVector3;
            var angle = (drawPos - centerVector).AngleFlat();
            var list = new List<IntVec3>();
            for (var i = 0; i < 500; i += 2)
            {
                var vector = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, i, angle);
                if (vector.x > Map.Size.x || vector.x < 0f || vector.z > Map.Size.z || vector.z < 0f)
                {
                    break;
                }

                list.Add(vector.ToIntVec3());
            }

            var list2 = new List<IntVec3>();
            foreach (var targetPos in list)
            {
                var pos_Square = MYDE_ModFront.GetPos_Square(targetPos, 2, 2);
                list2.AddRange(pos_Square);
            }

            GenDraw.DrawFieldEdges(list2, Color.yellow);
        }, null);
    }

    public override void CompTick()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill != null && building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulation <
            building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulationMax)
        {
            Building_DrakkenLaserDrill_ConcentratedBeam_Label =
                (int)building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulation + " / " +
                building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulationMax;
            Building_DrakkenLaserDrill_ConcentratedBeam_Icon =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Nothing/Nothing");
        }
        else if (building_DrakkenLaserDrill != null && building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulation >=
                 building_DrakkenLaserDrill.ConcentratedBeam_EnergyAccumulationMax)
        {
            var num = building_DrakkenLaserDrill.Base_ConsumePowerFactor * building_DrakkenLaserDrill.DamageNum *
                      building_DrakkenLaserDrill.PowerConsumeFactor_ConcentratedBeam;
            var num2 = num * CompPower.WattsToWattDaysPerTick * 180f;
            Building_DrakkenLaserDrill_ConcentratedBeam_Label =
                "DrakkenLaserDrill_ConcentratedBeam_Label".Translate() + "：" + num2.ToString();
            Building_DrakkenLaserDrill_ConcentratedBeam_Icon =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/ConcentratedBeam");
        }
    }
}