using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill
{
    [StaticConstructorOnStartup]
    public class Comp_DrakkenLaserDrill_PulseCannon : ThingComp
    {
        // [수정] 텍스처 캐싱 (성능 최적화)
        private static readonly Texture2D Icon_PulseCannon = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/PulseCannon");
        private static readonly Texture2D Icon_Nothing = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Nothing/Nothing");

        public CompProperties_DrakkenLaserDrill_PulseCannon Props => props as CompProperties_DrakkenLaserDrill_PulseCannon;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            // 연구 프로젝트 확인 (매 틱 검사하지 않음)
            var ResearchProject = DefDatabase<ResearchProjectDef>.GetNamed("MYDE_DrakkenLaserDrill_Research_PulseCannon", false);

            var Building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
            if (Building_DrakkenLaserDrill is { IfCrossMap: true })
            {
                yield break;
            }

            if (ResearchProject != null && ResearchProject.IsFinished)
            {
                // [수정] 라벨 및 아이콘 결정 로직을 이곳으로 이동
                string label;
                Texture2D icon;

                if (Building_DrakkenLaserDrill != null && Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulation >=
                    Building_DrakkenLaserDrill.PulseCannon_EnergyAccumulationMax)
                {
                    // 충전 완료 상태
                    var num = Building_DrakkenLaserDrill.Base_ConsumePowerFactor *
                              Building_DrakkenLaserDrill.DamageNum *
                              Building_DrakkenLaserDrill.PowerConsumeFactor_PulseCannon;
                    var num2 = num * CompPower.WattsToWattDaysPerTick * 180f;

                    label = "DrakkenLaserDrill_PulseCannon_Label".Translate() + "：" + num2.ToString("F0");
                    icon = Icon_PulseCannon;
                }
                else
                {
                    // 충전 중 상태
                    float current = Building_DrakkenLaserDrill?.PulseCannon_EnergyAccumulation ?? 0f;
                    float max = Building_DrakkenLaserDrill?.PulseCannon_EnergyAccumulationMax ?? 1f;

                    label = $"{(int)current} / {max}";
                    icon = Icon_Nothing;
                }

                yield return new Command_Action
                {
                    action = DoSomething,
                    defaultLabel = label,
                    icon = icon,
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
            // [수정] TryGetComp 대신, Building_DrakkenLaserDrill.cs에서 최적화했다면 캐싱된 변수를 쓰는 게 좋지만 여기서는 안전하게 유지
            var compPower = Building_DrakkenLaserDrill?.TryGetComp<CompPower>();

            if (Building_DrakkenLaserDrill != null && compPower != null)
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
            Find.Targeter.BeginTargeting(targetingParameters, delegate (LocalTargetInfo Target)
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
            }, delegate (LocalTargetInfo target)
            {
                GenDraw.DrawRadiusRing(target.Cell, 12.9f, Color.red);
                GenDraw.DrawRadiusRing(target.Cell, 2.9f, Color.white);
            }, null);
        }

        // [수정] CompTick 완전히 삭제됨
    }
}