using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class ITab_ColorChange : ITab
{
    private static readonly Vector2 WinSize = new(700f, 630f);

    public ITab_ColorChange()
    {
        size = WinSize;
        labelKey = "DrakkenLaserDrill_Base_Setting";
        tutorTag = "DrakkenLaserDrill_Base_Setting".Translate();
    }

    public override bool IsVisible => true;

    private Building_DrakkenLaserDrill Building_DrakkenLaserDrill => (Building_DrakkenLaserDrill)SelThing;

    protected override void FillTab()
    {
        var rect = new Rect(20f, 30f, 660f, 180f);
        Widgets.FillableBar(rect, 1f, MYDE_TexButton.Background, MYDE_TexButton.Background, false);
        var rect2 = rect;
        rect2.x += 10f;
        rect2.y += 5f;
        rect2.width -= 20f;
        rect2.height = 50f;
        Text.Font = GameFont.Medium;
        Widgets.Label(rect2, "DrakkenLaserDrill_Color_Laser".Translate());
        Text.Font = GameFont.Small;
        var rect3 = rect;
        rect3.x += 10f;
        rect3.y += 35f;
        rect3.width -= 20f;
        rect3.height = 50f;
        Widgets.Label(rect3, "R ：" + Building_DrakkenLaserDrill.Color_Red);
        var rect4 = rect;
        rect4.x += 10f;
        rect4.y += 60f;
        rect4.width -= 20f;
        rect4.height = 50f;
        Building_DrakkenLaserDrill.Color_Red =
            (int)Widgets.HorizontalSlider(rect4, Building_DrakkenLaserDrill.Color_Red, 0f, 255f);
        var rect5 = rect;
        rect5.x += 10f;
        rect5.y += 85f;
        rect5.width -= 20f;
        rect5.height = 50f;
        Widgets.Label(rect5, "G ：" + Building_DrakkenLaserDrill.Color_Green);
        var rect6 = rect;
        rect6.x += 10f;
        rect6.y += 110f;
        rect6.width -= 20f;
        rect6.height = 50f;
        Building_DrakkenLaserDrill.Color_Green =
            (int)Widgets.HorizontalSlider(rect6, Building_DrakkenLaserDrill.Color_Green, 0f, 255f);
        var rect7 = rect;
        rect7.x += 10f;
        rect7.y += 135f;
        rect7.width -= 20f;
        rect7.height = 50f;
        Widgets.Label(rect7, "B ：" + Building_DrakkenLaserDrill.Color_Blue);
        var rect8 = rect;
        rect8.x += 10f;
        rect8.y += 160f;
        rect8.width -= 20f;
        rect8.height = 50f;
        Building_DrakkenLaserDrill.Color_Blue =
            (int)Widgets.HorizontalSlider(rect8, Building_DrakkenLaserDrill.Color_Blue, 0f, 255f);
        var rect9 = new Rect(20f, 230f, 660f, 180f);
        Widgets.FillableBar(rect9, 1f, MYDE_TexButton.Background, MYDE_TexButton.Background, false);
        var rect10 = rect9;
        rect10.x += 10f;
        rect10.y += 5f;
        rect10.width -= 20f;
        rect10.height = 50f;
        Text.Font = GameFont.Medium;
        Widgets.Label(rect10, "DrakkenLaserDrill_Color_MinLaser".Translate());
        Text.Font = GameFont.Small;
        var rect11 = rect9;
        rect11.x += 10f;
        rect11.y += 35f;
        rect11.width -= 20f;
        rect11.height = 50f;
        Widgets.Label(rect11, "R ：" + Building_DrakkenLaserDrill.Color_Min_Red);
        var rect12 = rect9;
        rect12.x += 10f;
        rect12.y += 60f;
        rect12.width -= 20f;
        rect12.height = 50f;
        Building_DrakkenLaserDrill.Color_Min_Red =
            (int)Widgets.HorizontalSlider(rect12, Building_DrakkenLaserDrill.Color_Min_Red, 0f, 255f);
        var rect13 = rect9;
        rect13.x += 10f;
        rect13.y += 85f;
        rect13.width -= 20f;
        rect13.height = 50f;
        Widgets.Label(rect13, "G ：" + Building_DrakkenLaserDrill.Color_Min_Green);
        var rect14 = rect9;
        rect14.x += 10f;
        rect14.y += 110f;
        rect14.width -= 20f;
        rect14.height = 50f;
        Building_DrakkenLaserDrill.Color_Min_Green =
            (int)Widgets.HorizontalSlider(rect14, Building_DrakkenLaserDrill.Color_Min_Green, 0f, 255f);
        var rect15 = rect9;
        rect15.x += 10f;
        rect15.y += 135f;
        rect15.width -= 20f;
        rect15.height = 50f;
        Widgets.Label(rect15, "B ：" + Building_DrakkenLaserDrill.Color_Min_Blue);
        var rect16 = rect9;
        rect16.x += 10f;
        rect16.y += 160f;
        rect16.width -= 20f;
        rect16.height = 50f;
        Building_DrakkenLaserDrill.Color_Min_Blue =
            (int)Widgets.HorizontalSlider(rect16, Building_DrakkenLaserDrill.Color_Min_Blue, 0f, 255f);
        var rect17 = new Rect(20f, 430f, 660f, 60f);
        Widgets.FillableBar(rect17, 1f, MYDE_TexButton.Background, MYDE_TexButton.Background, false);
        var rect18 = rect17;
        rect18.x += 10f;
        rect18.y += 5f;
        rect18.width -= 20f;
        rect18.height = 50f;
        Text.Font = GameFont.Medium;
        Widgets.Label(rect18,
            "DrakkenLaserDrill_Set_DamageArmorPenetration".Translate() + "：" +
            ((int)(Building_DrakkenLaserDrill.DamageArmorPenetration * 100f)).ToString() + "%" + " / " +
            (Building_DrakkenLaserDrill.DamageArmorPenetrationMax * 100f).ToString() + "%");
        Text.Font = GameFont.Small;
        var rect19 = rect17;
        rect19.x += 15f;
        rect19.y += 35f;
        rect19.width -= 20f;
        rect19.height = 50f;
        Building_DrakkenLaserDrill.DamageArmorPenetration = Widgets.HorizontalSlider(rect19,
            Building_DrakkenLaserDrill.DamageArmorPenetration, 0f,
            Building_DrakkenLaserDrill.DamageArmorPenetrationMax);
        var rect20 = new Rect(20f, 500f, 660f, 60f);
        Widgets.FillableBar(rect20, 1f, MYDE_TexButton.Background, MYDE_TexButton.Background, false);
        var rect21 = rect20;
        rect21.x += 10f;
        rect21.y += 5f;
        rect21.width -= 20f;
        rect21.height = 50f;
        Text.Font = GameFont.Medium;
        Widgets.Label(rect21,
            "DrakkenLaserDrill_Set_DamageNum".Translate() + "：" + Building_DrakkenLaserDrill.DamageNum.ToString() +
            " / " + Building_DrakkenLaserDrill.DamageNumMax.ToString());
        Text.Font = GameFont.Small;
        var rect22 = rect20;
        rect22.x += 15f;
        rect22.y += 35f;
        rect22.width -= 20f;
        rect22.height = 50f;
        Building_DrakkenLaserDrill.DamageNum = (int)Widgets.HorizontalSlider(rect22,
            Building_DrakkenLaserDrill.DamageNum, 0f, Building_DrakkenLaserDrill.DamageNumMax);
        var rect23 = new Rect(20f, 570f, 40f, 40f);
        var tex = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/True");
        if (!Building_DrakkenLaserDrill.IfAttackDown)
        {
            tex = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/False");
        }

        var butRect = rect23;
        butRect.x += 0f;
        butRect.y += 0f;
        if (Widgets.ButtonImage(butRect, tex))
        {
            if (Building_DrakkenLaserDrill.IfAttackDown)
            {
                Building_DrakkenLaserDrill.IfAttackDown = false;
            }
            else if (!Building_DrakkenLaserDrill.IfAttackDown)
            {
                Building_DrakkenLaserDrill.IfAttackDown = true;
            }
        }

        var rect24 = rect23;
        rect24.x += 50f;
        rect24.y += 5f;
        rect24.width = 600f;
        rect24.height = 50f;
        Text.Font = GameFont.Medium;
        string text = "DrakkenLaserDrill_Set_IfAttackDown_True".Translate();
        if (!Building_DrakkenLaserDrill.IfAttackDown)
        {
            text = "DrakkenLaserDrill_Set_IfAttackDown_False".Translate();
        }

        Widgets.Label(rect24, "DrakkenLaserDrill_Set_IfAttackDown".Translate() + "：" + text);
        var a = 0.9f;
        GUI.DrawTexture(new Rect(420f, 37f, 250f, 30f),
            color: new Color(Building_DrakkenLaserDrill.Color_Red / 255f, Building_DrakkenLaserDrill.Color_Green / 255f,
                Building_DrakkenLaserDrill.Color_Blue / 255f, a), image: MYDE_TexButton.DrakkenLaserDrill_Laser,
            scaleMode: ScaleMode.ScaleAndCrop, alphaBlend: true, imageAspect: 1f, borderWidth: 300f, borderRadius: 20f);
        GUI.DrawTexture(new Rect(420f, 237f, 250f, 30f),
            color: new Color(Building_DrakkenLaserDrill.Color_Min_Red / 255f,
                Building_DrakkenLaserDrill.Color_Min_Green / 255f, Building_DrakkenLaserDrill.Color_Min_Blue / 255f, a),
            image: MYDE_TexButton.DrakkenLaserDrill_Laser, scaleMode: ScaleMode.ScaleAndCrop, alphaBlend: true,
            imageAspect: 1f, borderWidth: 300f, borderRadius: 20f);
        var tex2 = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Square");
        var rect25 = rect;
        rect25.y += 7f;
        rect25.width = 30f;
        rect25.height = 30f;
        var butRect2 = rect25;
        butRect2.x += 350f;
        var num = 255f;
        var num2 = 234f;
        var num3 = 3f;
        if (Widgets.ButtonImage(baseColor: new Color(num / 255f, num2 / 255f, num3 / 255f), butRect: butRect2,
                tex: tex2))
        {
            Building_DrakkenLaserDrill.Color_Red = num;
            Building_DrakkenLaserDrill.Color_Green = num2;
            Building_DrakkenLaserDrill.Color_Blue = num3;
        }

        var rect26 = rect9;
        rect26.y += 7f;
        rect26.width = 30f;
        rect26.height = 30f;
        var butRect3 = rect26;
        butRect3.x += 350f;
        var num4 = 255f;
        var num5 = 128f;
        var num6 = 3f;
        if (!Widgets.ButtonImage(baseColor: new Color(num4 / 255f, num5 / 255f, num6 / 255f), butRect: butRect3,
                tex: tex2))
        {
            return;
        }

        Building_DrakkenLaserDrill.Color_Min_Red = num4;
        Building_DrakkenLaserDrill.Color_Min_Green = num5;
        Building_DrakkenLaserDrill.Color_Min_Blue = num6;
    }
}