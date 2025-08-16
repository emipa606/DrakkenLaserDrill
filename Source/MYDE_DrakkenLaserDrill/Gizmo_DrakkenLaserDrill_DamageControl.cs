using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Gizmo_DrakkenLaserDrill_DamageControl : Gizmo
{
    public static readonly Texture2D WeaponControl_Attack_Building_Icon =
        ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Icon/Beacon");

    public Comp_DrakkenLaserDrill_DamageControl Comp;

    public Gizmo_DrakkenLaserDrill_DamageControl()
    {
        Order = 2f;
    }

    public override float GetWidth(float maxWidth)
    {
        return 217f;
    }

    public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
    {
        var building_DrakkenLaserDrill = Comp.parent as Building_DrakkenLaserDrill;
        var rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
        var rect2 = rect.ContractedBy(6f);
        Widgets.DrawWindowBackground(rect);
        var rect3 = rect2;
        rect3.width = 63f;
        rect3.height = 63f;
        var rect4 = rect3;
        rect4.x += 5f;
        rect4.y += 10f;
        rect4.width = 200f;
        rect4.height = 50f;
        var rect5 = rect4;
        rect5.y -= 10f;
        Widgets.Label(rect5,
            "Building_DrakkenLaserDrill_Damage_Num".Translate() + "：" +
            building_DrakkenLaserDrill?.DamageNum.ToString());
        var rect6 = rect4;
        rect6.y += 30f;
        if (building_DrakkenLaserDrill == null)
        {
            return new GizmoResult(GizmoState.Clear);
        }

        building_DrakkenLaserDrill.DamageNum = (int)Widgets.HorizontalSlider(rect6,
            building_DrakkenLaserDrill.DamageNum, 6f, building_DrakkenLaserDrill.DamageNumMax);
        var rect7 = rect4;
        rect7.x += 132f;
        rect7.y -= 10f;
        rect7.width = 30f;
        rect7.height = 30f;
        if (Widgets.ButtonText(rect7, "+") &&
            building_DrakkenLaserDrill.DamageNum < building_DrakkenLaserDrill.DamageNumMax)
        {
            building_DrakkenLaserDrill.DamageNum++;
        }

        var rect8 = rect4;
        rect8.x += 167f;
        rect8.y -= 10f;
        rect8.width = 30f;
        rect8.height = 30f;
        if (Widgets.ButtonText(rect8, "-") && building_DrakkenLaserDrill.DamageNum > 6)
        {
            building_DrakkenLaserDrill.DamageNum--;
        }

        return new GizmoResult(GizmoState.Clear);
    }
}