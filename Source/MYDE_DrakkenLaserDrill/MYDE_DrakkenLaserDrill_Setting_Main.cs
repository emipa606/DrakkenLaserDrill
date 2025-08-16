using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class MYDE_DrakkenLaserDrill_Setting_Main : Mod
{
    public static MYDE_DrakkenLaserDrill_Setting Settings;
    private static string currentVersion;

    public MYDE_DrakkenLaserDrill_Setting_Main(ModContentPack content)
        : base(content)
    {
        Settings = GetSettings<MYDE_DrakkenLaserDrill_Setting>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override string SettingsCategory()
    {
        return "DrakkenLaserDrill_Setting_Label".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(new Rect(inRect.x, inRect.y, inRect.width, inRect.height));
        if (listing_Standard.ButtonText("DrakkenLaserDrill_Setting_Initialization".Translate()))
        {
            MYDE_DrakkenLaserDrill_Setting.Initialization();
        }

        listing_Standard.GapLine(20f);
        Text.Font = GameFont.Medium;
        string text = "（" + "DrakkenLaserDrill_TranslateConsumePowerPreSecond".Translate() + "：" +
                      (MYDE_DrakkenLaserDrill_Setting.Base_ConsumePowerFactor * CompPower.WattsToWattDaysPerTick * 60f)
                      .ToString() + " * " + "DrakkenLaserDrill_DamageNumNow".Translate() + "）";
        listing_Standard.Label("DrakkenLaserDrill_Base_ConsumePowerFactor".Translate() + " : " +
                               ((int)MYDE_DrakkenLaserDrill_Setting.Base_ConsumePowerFactor).ToString() + text);
        listing_Standard.Gap(10f);
        MYDE_DrakkenLaserDrill_Setting.Base_ConsumePowerFactor =
            (int)listing_Standard.Slider(MYDE_DrakkenLaserDrill_Setting.Base_ConsumePowerFactor, 0f, 12000f);
        listing_Standard.GapLine(20f);
        Text.Font = GameFont.Medium;
        listing_Standard.Label("DrakkenLaserDrill_Extra_StoredEnergyMax".Translate() + " : " +
                               MYDE_DrakkenLaserDrill_Setting.Extra_StoredEnergyMax.ToString());
        listing_Standard.Gap(10f);
        MYDE_DrakkenLaserDrill_Setting.Extra_StoredEnergyMax =
            (int)listing_Standard.Slider(MYDE_DrakkenLaserDrill_Setting.Extra_StoredEnergyMax, 0f, 3000f);
        listing_Standard.GapLine(20f);
        Text.Font = GameFont.Medium;
        listing_Standard.Label("DrakkenLaserDrill_Extra_DamageNumMax".Translate() + " : " +
                               MYDE_DrakkenLaserDrill_Setting.Extra_DamageNumMax.ToString());
        listing_Standard.Gap(10f);
        MYDE_DrakkenLaserDrill_Setting.Extra_DamageNumMax =
            (int)listing_Standard.Slider(MYDE_DrakkenLaserDrill_Setting.Extra_DamageNumMax, 0f, 20f);
        listing_Standard.GapLine(20f);
        Text.Font = GameFont.Medium;
        listing_Standard.Label("DrakkenLaserDrill_Extra_DamageArmorPenetrationMax".Translate() + " : " +
                               (MYDE_DrakkenLaserDrill_Setting.Extra_DamageArmorPenetrationMax * 100f).ToString() +
                               "%");
        listing_Standard.Gap(10f);
        MYDE_DrakkenLaserDrill_Setting.Extra_DamageArmorPenetrationMax =
            listing_Standard.Slider(MYDE_DrakkenLaserDrill_Setting.Extra_DamageArmorPenetrationMax, 0f, 10f);
        listing_Standard.End();
        var butRect = inRect;
        butRect.x += 0f;
        butRect.y += 400f;
        butRect.width = 40f;
        butRect.height = 40f;
        var tex = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/True");
        if (!MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
        {
            tex = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/False");
        }

        if (Widgets.ButtonImage(butRect, tex))
        {
            if (MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
            {
                MYDE_DrakkenLaserDrill_Setting.IfShowMessage = false;
            }
            else if (!MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
            {
                MYDE_DrakkenLaserDrill_Setting.IfShowMessage = true;
            }
        }

        var rect2 = inRect;
        rect2.x += 50f;
        rect2.y += 405f;
        rect2.width = 600f;
        rect2.height = 50f;
        string text2 = "DrakkenLaserDrill_Set_IfShowMessage_True".Translate();
        if (!MYDE_DrakkenLaserDrill_Setting.IfShowMessage)
        {
            text2 = "DrakkenLaserDrill_Set_IfShowMessage_False".Translate();
        }

        Widgets.Label(rect2, "DrakkenLaserDrill_Set_IfShowMessage".Translate() + "：" + text2);
        var butRect2 = inRect;
        butRect2.x += 0f;
        butRect2.y += 460f;
        butRect2.width = 40f;
        butRect2.height = 40f;
        var tex2 = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/True");
        if (!MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange)
        {
            tex2 = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/False");
        }

        if (Widgets.ButtonImage(butRect2, tex2))
        {
            if (MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange)
            {
                MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange = false;
            }
            else if (!MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange)
            {
                MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange = true;
            }
        }

        var rect4 = inRect;
        rect4.x += 50f;
        rect4.y += 465f;
        rect4.width = 600f;
        rect4.height = 50f;
        string text3 = "DrakkenLaserDrill_Set_IfIgnoreMapRange_True".Translate();
        if (!MYDE_DrakkenLaserDrill_Setting.IfIgnoreMapRange)
        {
            text3 = "DrakkenLaserDrill_Set_IfIgnoreMapRange_False".Translate();
        }

        Widgets.Label(rect4, "DrakkenLaserDrill_Set_IfIgnoreMapRange".Translate() + "：" + text3);

        if (currentVersion == null)
        {
            return;
        }

        var rect5 = inRect;
        rect5.x += 50f;
        rect5.y += 515f;
        rect5.width = 600f;
        rect5.height = 25f;
        GUI.contentColor = Color.gray;
        Widgets.Label(rect5, "DrakkenLaserDrill_CurrentModVersion".Translate(currentVersion));
        GUI.contentColor = Color.white;
    }
}