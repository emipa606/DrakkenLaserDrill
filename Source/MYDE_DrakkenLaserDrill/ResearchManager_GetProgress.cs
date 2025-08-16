using HarmonyLib;
using RimWorld;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[HarmonyPatch(typeof(ResearchManager), nameof(ResearchManager.GetProgress))]
internal class ResearchManager_GetProgress
{
    private static float Postfix(float __result, ResearchProjectDef proj)
    {
        var num = __result;
        if (proj.defName == "MYDE_DrakkenLaserDrill_Research_StoredEnergyMax" && num >= proj.CostApparent)
        {
            num = 0f;
            var list = Find.CurrentMap.listerThings.ThingsOfDef(MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill);
            if (list.Count > 0)
            {
                foreach (var thing in list)
                {
                    var building_DrakkenLaserDrill = (Building_DrakkenLaserDrill)thing;
                    if (building_DrakkenLaserDrill.AddColdDownBool)
                    {
                        continue;
                    }

                    if (MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
                    {
                        Find.LetterStack.ReceiveLetter("DrakkenLaserDrill_UpDate_Label".Translate(),
                            proj.description, LetterDefOf.NeutralEvent);
                    }

                    building_DrakkenLaserDrill.StoredEnergyMax +=
                        500f + MYDE_DrakkenLaserDrill_Setting.Extra_StoredEnergyMax;
                    building_DrakkenLaserDrill.AddColdDownBool = true;
                    building_DrakkenLaserDrill.Set_StoredEnergyMax();
                }
            }
            else if (list.Count <= 0 && MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
            {
                Find.LetterStack.ReceiveLetter("DrakkenLaserDrill_FailUpDate_Label".Translate(),
                    "DrakkenLaserDrill_FailUpDate_Desc".Translate(), LetterDefOf.NegativeEvent);
            }
        }

        if (proj.defName == "MYDE_DrakkenLaserDrill_Research_Damage" && num >= proj.CostApparent)
        {
            num = 0f;
            var list2 = Find.CurrentMap.listerThings.ThingsOfDef(MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill);
            if (list2.Count > 0)
            {
                foreach (var thing in list2)
                {
                    var building_DrakkenLaserDrill2 = (Building_DrakkenLaserDrill)thing;
                    if (building_DrakkenLaserDrill2.AddColdDownBool)
                    {
                        continue;
                    }

                    if (MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
                    {
                        Find.LetterStack.ReceiveLetter("DrakkenLaserDrill_UpDate_Label".Translate(),
                            proj.description, LetterDefOf.NeutralEvent);
                    }

                    building_DrakkenLaserDrill2.DamageNumMax +=
                        1 + MYDE_DrakkenLaserDrill_Setting.Extra_DamageNumMax;
                    building_DrakkenLaserDrill2.AddColdDownBool = true;
                }

                foreach (var thing in list2)
                {
                    var building_DrakkenLaserDrill3 = (Building_DrakkenLaserDrill)thing;
                    building_DrakkenLaserDrill3.Check_Highest_DamageNumMax();
                }
            }
            else if (list2.Count <= 0 && MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
            {
                Find.LetterStack.ReceiveLetter("DrakkenLaserDrill_FailUpDate_Label".Translate(),
                    "DrakkenLaserDrill_FailUpDate_Desc".Translate(), LetterDefOf.NegativeEvent);
            }
        }

        if (proj.defName != "MYDE_DrakkenLaserDrill_Research_ArmorPenetration" || !(num >= proj.CostApparent))
        {
            return num;
        }

        num = 0f;
        var list3 = Find.CurrentMap.listerThings.ThingsOfDef(MYDE_ThingDefOf.MYDE_Building_DrakkenLaserDrill);
        if (list3.Count > 0)
        {
            foreach (var thing in list3)
            {
                var building_DrakkenLaserDrill4 = (Building_DrakkenLaserDrill)thing;
                if (building_DrakkenLaserDrill4.AddColdDownBool)
                {
                    continue;
                }

                if (MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
                {
                    Find.LetterStack.ReceiveLetter("DrakkenLaserDrill_UpDate_Label".Translate(),
                        proj.description, LetterDefOf.NeutralEvent);
                }

                building_DrakkenLaserDrill4.DamageArmorPenetrationMax +=
                    0.1f + MYDE_DrakkenLaserDrill_Setting.Extra_DamageArmorPenetrationMax;
                building_DrakkenLaserDrill4.AddColdDownBool = true;
            }

            foreach (var thing in list3)
            {
                var building_DrakkenLaserDrill5 = (Building_DrakkenLaserDrill)thing;
                building_DrakkenLaserDrill5.Check_Highest_DamageArmorPenetrationMax();
            }
        }
        else if (list3.Count <= 0 && MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
        {
            Find.LetterStack.ReceiveLetter("DrakkenLaserDrill_FailUpDate_Label".Translate(),
                "DrakkenLaserDrill_FailUpDate_Desc".Translate(), LetterDefOf.NegativeEvent);
        }

        return num;
    }
}