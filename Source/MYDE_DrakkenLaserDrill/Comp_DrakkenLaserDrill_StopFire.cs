using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill
{
    [StaticConstructorOnStartup]
    public class Comp_DrakkenLaserDrill_StopFire : ThingComp
    {
        // [수정] 텍스처 캐싱 (성능 최적화)
        private static readonly Texture2D Icon_Destroy = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Destroy");

        public CompProperties_DrakkenLaserDrill_StopFire Props => props as CompProperties_DrakkenLaserDrill_StopFire;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Action
            {
                action = DoSomething,
                defaultLabel = "DrakkenLaserDrill_StopFire_Label".Translate(),
                defaultDesc = "DrakkenLaserDrill_StopFire_Desc".Translate(),
                icon = Icon_Destroy // [수정] 캐시된 아이콘 사용
            };
        }

        private void DoSomething()
        {
            var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
            building_DrakkenLaserDrill?.DestroyAllBeacon();
        }

        // [수정] CompTick 삭제됨
    }
}