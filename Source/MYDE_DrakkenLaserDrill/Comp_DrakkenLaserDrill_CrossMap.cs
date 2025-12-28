using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill
{
    [StaticConstructorOnStartup]
    public class Comp_DrakkenLaserDrill_CrossMap : ThingComp
    {
        // [수정] 텍스처 캐싱 (성능 최적화)
        private static readonly Texture2D Icon_True = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/CrossMap_True");
        private static readonly Texture2D Icon_False = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/CrossMap_False");

        public CompProperties_DrakkenLaserDrill_CrossMap Props => props as CompProperties_DrakkenLaserDrill_CrossMap;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            var building = parent as Building_DrakkenLaserDrill;
            if (building == null) yield break;

            // [수정] UI를 그릴 때 상태를 확인하여 라벨과 아이콘 결정 (CompTick 대체)
            string label;
            Texture2D icon;

            if (building.IfCrossMap)
            {
                label = "DrakkenLaserDrill_CrossMap_True_Label".Translate();
                icon = Icon_True;
            }
            else
            {
                label = "DrakkenLaserDrill_CrossMap_False_Label".Translate();
                icon = Icon_False;
            }

            yield return new Command_Action
            {
                action = DoSomething_CrossMapSwitch,
                defaultLabel = label,
                icon = icon,
                defaultDesc = "DrakkenLaserDrill_CrossMap_Desc".Translate()
            };
        }

        private void DoSomething_CrossMapSwitch()
        {
            var building_DrakkenLaserDrill = parent as Building_DrakkenLaserDrill;
            if (building_DrakkenLaserDrill == null) return;

            // 단순 토글 로직으로 간소화 가능하지만, 원본 로직 유지
            if (building_DrakkenLaserDrill.IfCrossMap)
            {
                building_DrakkenLaserDrill.IfCrossMap = false;
            }
            else
            {
                building_DrakkenLaserDrill.IfCrossMap = true;
            }
        }

        // [수정] CompTick 삭제됨 (더 이상 필요 없음)
    }
}