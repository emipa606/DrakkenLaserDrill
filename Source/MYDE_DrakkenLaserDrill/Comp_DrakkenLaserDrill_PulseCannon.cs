using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Comp_DrakkenLaserDrill_PulseCannon : ThingComp
{
    private Texture2D Building_DrakkenLaserDrill_PulseCannon_Icon;
    private string Building_DrakkenLaserDrill_PulseCannon_Label;

    public CompProperties_DrakkenLaserDrill_PulseCannon Props => props as CompProperties_DrakkenLaserDrill_PulseCannon;

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        var ResearchProject =
            DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find(x =>
                x.defName == "MYDE_DrakkenLaserDrill_Research_PulseCannon");
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
                                      Building_DrakkenLaserDrill.PowerConsumeFactor_PulseCannon;
                _ = PowerConsumeNum * CompPower.WattsToWattDaysPerTick * 180f;
            }

            yield return new Command_Action
            {
                action = DoSomething,
                defaultLabel = Building_DrakkenLaserDrill_PulseCannon_Label,
                icon = Building_DrakkenLaserDrill_PulseCannon_Icon,
                defaultDesc = "DrakkenLaserDrill_PulseCannon_Desc".Translate()
            };
        }

        if (DebugSettings.ShowDevGizmos)
        {
            yield return new Command_Action
            {
                defaultLabel = "Max：PulseCannon",
                action = delegate
                {
                    if (Building_DrakkenLaserDrill != null)
                    {
                        Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation =
                            Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax;
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
            var cell = Target.Cell;
            var realPos = new Vector2(Target.CenterVector3.x, Target.CenterVector3.z);
            var building_DrakkenLaserDrill_Beacon_PulseCannon =
                (Building_DrakkenLaserDrill_Beacon_PulseCannon)ThingMaker.MakeThing(MYDE_ThingDefOf
                    .MYDE_Building_DrakkenLaserDrill_Beacon_PulseCannon);
            ((Building_DrakkenLaserDrill_Beacon_PulseCannon)GenSpawn.Spawn(
                    building_DrakkenLaserDrill_Beacon_PulseCannon, cell, Map))
                .CheckSpawn(Building_DrakkenLaserDrill, realPos);
            Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_PulseCannon =
                building_DrakkenLaserDrill_Beacon_PulseCannon;
            var mYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave =
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave;
            var effecter = new Effecter(mYDE_Building_DrakkenLaserDrill_Effecter_Vaporize_Heatwave)
            {
                scale = 1f
            };
            effecter.Trigger(new TargetInfo(Target.Cell, Map), TargetInfo.Invalid);
            effecter.Cleanup();
            Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation = 0f;
        }, delegate(LocalTargetInfo target)
        {
            GenDraw.DrawRadiusRing(target.Cell, 12.9f, Color.red);
            GenDraw.DrawRadiusRing(target.Cell, 2.9f, Color.white);
        }, null);
    }

    public override void CompTick()
    {
        var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
        if (building_DrakkenLaserDrill != null && building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation <
            building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax)
        {
            Building_DrakkenLaserDrill_PulseCannon_Label =
                (int)building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation + " / " +
                building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax;
            Building_DrakkenLaserDrill_PulseCannon_Icon =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Nothing/Nothing");
        }
        else if (building_DrakkenLaserDrill != null && building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation >=
                 building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax)
        {
            var num = building_DrakkenLaserDrill.Base_ConsumePowerFactor * building_DrakkenLaserDrill.DamageNum *
                      building_DrakkenLaserDrill.PowerConsumeFactor_PulseCannon;
            var num2 = num * CompPower.WattsToWattDaysPerTick * 180f;
            Building_DrakkenLaserDrill_PulseCannon_Label =
                "DrakkenLaserDrill_PulseCannon_Label".Translate() + "：" + num2.ToString();
            Building_DrakkenLaserDrill_PulseCannon_Icon =
                ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/PulseCannon");
        }
    }
}