using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Building_DrakkenLaserDrill_Beacon_Mouse : ThingWithComps
{
    private readonly float MinLaser_Alpha = 0.6f;

    private readonly float MinLaser_Width = 0.3f;

    private readonly float MinLaserPos_A_Range_Limit = 0.4f;

    private readonly float MinLaserPos_B_Range_Limit = 0.4f;

    private readonly float MinLaserPos_C_Range_Limit = 0.4f;

    private readonly float MinLaserPos_D_Range_Limit = 0.4f;

    private readonly float MinLaserPos_E_Range_Limit = 0.4f;

    private readonly float MinLaserPos_F_Range_Limit = 0.4f;
    private float Angle;

    private Building_DrakkenLaserDrill Building_DrakkenLaserDrill;

    private int LaserScaleTick;

    private Vector3 MinLaserPos_A_End;

    private float MinLaserPos_A_Range = 0.5f;
    private Vector3 MinLaserPos_A_Start;

    private bool MinLaserPos_A_UpOrDown = true;

    private Vector3 MinLaserPos_B_End;

    private float MinLaserPos_B_Range = -0.5f;

    private Vector3 MinLaserPos_B_Start;

    private bool MinLaserPos_B_UpOrDown;

    private Vector3 MinLaserPos_C_End;

    private float MinLaserPos_C_Range = 0.2f;

    private Vector3 MinLaserPos_C_Start;

    private bool MinLaserPos_C_UpOrDown = true;

    private Vector3 MinLaserPos_D_End;

    private float MinLaserPos_D_Range = -0.2f;

    private Vector3 MinLaserPos_D_Start;

    private bool MinLaserPos_D_UpOrDown;

    private Vector3 MinLaserPos_E_End;

    private float MinLaserPos_E_Range = -0.1f;

    private Vector3 MinLaserPos_E_Start;

    private bool MinLaserPos_E_UpOrDown = true;

    private Vector3 MinLaserPos_F_End;

    private float MinLaserPos_F_Range = 0.1f;

    private Vector3 MinLaserPos_F_Start;

    private bool MinLaserPos_F_UpOrDown;

    private Vector2 RealPos;

    private bool TakeDamageBool;

    private int TakeDamageTick;

    private int TakeDamageTickMax = 10;

    public Vector3 TargetPos;

    public void CheckSpawn(Building_DrakkenLaserDrill buildingDrakkenLaserDrill, Vector2 realPos, Vector3 targetPos)
    {
        Building_DrakkenLaserDrill = buildingDrakkenLaserDrill;
        TargetPos = targetPos;
        RealPos = realPos;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref MinLaserPos_A_Range, "MinLaserPos_A_Range");
        Scribe_Values.Look(ref MinLaserPos_A_UpOrDown, "MinLaserPos_A_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_B_Range, "MinLaserPos_B_Range");
        Scribe_Values.Look(ref MinLaserPos_B_UpOrDown, "MinLaserPos_B_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_C_Range, "MinLaserPos_C_Range");
        Scribe_Values.Look(ref MinLaserPos_C_UpOrDown, "MinLaserPos_C_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_D_Range, "MinLaserPos_D_Range");
        Scribe_Values.Look(ref MinLaserPos_D_UpOrDown, "MinLaserPos_D_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_E_Range, "MinLaserPos_E_Range");
        Scribe_Values.Look(ref MinLaserPos_E_UpOrDown, "MinLaserPos_E_UpOrDown");
        Scribe_Values.Look(ref MinLaserPos_F_Range, "MinLaserPos_F_Range");
        Scribe_Values.Look(ref MinLaserPos_F_UpOrDown, "MinLaserPos_F_UpOrDown");
        Scribe_References.Look(ref Building_DrakkenLaserDrill, "Building_DrakkenLaserDrill");
        Scribe_Values.Look(ref LaserScaleTick, "LaserScaleTick");
        Scribe_Values.Look(ref TakeDamageBool, "TakeDamageBool");
        Scribe_Values.Look(ref TakeDamageTick, "TakeDamageTick");
        Scribe_Values.Look(ref TakeDamageTickMax, "TakeDamageTickMax");
        Scribe_Values.Look(ref TargetPos, "TargetPos");
        Scribe_Values.Look(ref RealPos, "RealPos");
        Scribe_Values.Look(ref Angle, "Angle");
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        base.DrawAt(drawLoc, flip);
        if (Building_DrakkenLaserDrill.Now_Rebuilding)
        {
            return;
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 6)
        {
            var drawPos = Building_DrakkenLaserDrill.DrawPos;
            var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
            var pos = (drawPos + vector) / 2f;
            pos.y = AltitudeLayer.PawnRope.AltitudeFor(3f);
            var lengthHorizontal = (drawPos.ToIntVec3() - vector.ToIntVec3()).LengthHorizontal;
            var x = 1.2f;
            if (LaserScaleTick < 30)
            {
                x = LaserScaleTick / 25f;
            }

            var num = (drawPos - vector).AngleFlat();
            var a = 0.8f;
            var color = new Color(Building_DrakkenLaserDrill.Color_Red / 255f,
                Building_DrakkenLaserDrill.Color_Green / 255f, Building_DrakkenLaserDrill.Color_Blue / 255f)
            {
                a = a
            };
            var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser"),
                ShaderDatabase.Transparent, color);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.AngleAxis(num, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
            Building_DrakkenLaserDrill.GunAngle = num;
        }

        Draw_MinLaserPos_Prepare();
    }

    protected override void Tick()
    {
        base.Tick();
        var map = Map;
        var targetPos = TargetPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var angle = (targetPos - vector).ToAngleFlat();
        Angle = angle;
        var num = RealPos.x - targetPos.x;
        var num2 = RealPos.y - targetPos.z;
        var distance = Math.Min(0.5f, Math.Abs(((num * num) + (num2 * num2)) / 5f));
        RealPos = RealPos.Moved(Angle, distance);
        Position = new IntVec3((int)RealPos.x, Position.y, (int)RealPos.y);
        var dataStatic = FleckMaker.GetDataStatic(vector, map,
            MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_HeatGlow_Intense, 2f);
        map.flecks.CreateFleck(dataStatic);
        TakeDamageTick++;
        if (TakeDamageTick >= TakeDamageTickMax && LaserScaleTick >= 30)
        {
            TakeDamageTick = 0;
            Building_DrakkenLaserDrill.DamagePos();
        }

        LaserScaleTick++;
        Get_MinLaserPos_A_Pos();
        Get_MinLaserPos_B_Pos();
        Get_MinLaserPos_C_Pos();
        Get_MinLaserPos_D_Pos();
        Get_MinLaserPos_E_Pos();
        Get_MinLaserPos_F_Pos();
        Building_DrakkenLaserDrill.SetBlueBarPos();
    }

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_Mouse = null;
        Building_DrakkenLaserDrill.ChangePowerConsumeToZero();
        base.Destroy(mode);
    }

    private void Get_MinLaserPos_A_Pos()
    {
        if (MinLaserPos_A_UpOrDown)
        {
            MinLaserPos_A_Range -= 0.01f;
            if (MinLaserPos_A_Range <= 0f - MinLaserPos_A_Range_Limit)
            {
                MinLaserPos_A_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_A_UpOrDown)
        {
            MinLaserPos_A_Range += 0.01f;
            if (MinLaserPos_A_Range >= MinLaserPos_A_Range_Limit)
            {
                MinLaserPos_A_UpOrDown = true;
            }
        }

        var minLaserPos_A_Range = MinLaserPos_A_Range;
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_A_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_A_Range, num2);
        MinLaserPos_A_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, minLaserPos_A_Range, num2);
    }

    private void Get_MinLaserPos_B_Pos()
    {
        if (MinLaserPos_B_UpOrDown)
        {
            MinLaserPos_B_Range -= 0.01f;
            if (MinLaserPos_B_Range <= 0f - MinLaserPos_B_Range_Limit)
            {
                MinLaserPos_B_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_B_UpOrDown)
        {
            MinLaserPos_B_Range += 0.01f;
            if (MinLaserPos_B_Range >= MinLaserPos_B_Range_Limit)
            {
                MinLaserPos_B_UpOrDown = true;
            }
        }

        var minLaserPos_B_Range = MinLaserPos_B_Range;
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_B_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_B_Range, num2);
        MinLaserPos_B_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, minLaserPos_B_Range, num2);
    }

    private void Get_MinLaserPos_C_Pos()
    {
        if (MinLaserPos_C_UpOrDown)
        {
            MinLaserPos_C_Range -= 0.01f;
            if (MinLaserPos_C_Range <= 0f - MinLaserPos_C_Range_Limit)
            {
                MinLaserPos_C_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_C_UpOrDown)
        {
            MinLaserPos_C_Range += 0.01f;
            if (MinLaserPos_C_Range >= MinLaserPos_C_Range_Limit)
            {
                MinLaserPos_C_UpOrDown = true;
            }
        }

        var minLaserPos_C_Range = MinLaserPos_C_Range;
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_C_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_C_Range, num2);
        MinLaserPos_C_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, minLaserPos_C_Range, num2);
    }

    private void Get_MinLaserPos_D_Pos()
    {
        if (MinLaserPos_D_UpOrDown)
        {
            MinLaserPos_D_Range -= 0.01f;
            if (MinLaserPos_D_Range <= 0f - MinLaserPos_D_Range_Limit)
            {
                MinLaserPos_D_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_D_UpOrDown)
        {
            MinLaserPos_D_Range += 0.01f;
            if (MinLaserPos_D_Range >= MinLaserPos_D_Range_Limit)
            {
                MinLaserPos_D_UpOrDown = true;
            }
        }

        var minLaserPos_D_Range = MinLaserPos_D_Range;
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_D_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_D_Range, num2);
        MinLaserPos_D_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, minLaserPos_D_Range, num2);
    }

    private void Get_MinLaserPos_E_Pos()
    {
        if (MinLaserPos_E_UpOrDown)
        {
            MinLaserPos_E_Range -= 0.01f;
            if (MinLaserPos_E_Range <= 0f - MinLaserPos_E_Range_Limit)
            {
                MinLaserPos_E_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_E_UpOrDown)
        {
            MinLaserPos_E_Range += 0.01f;
            if (MinLaserPos_E_Range >= MinLaserPos_E_Range_Limit)
            {
                MinLaserPos_E_UpOrDown = true;
            }
        }

        var minLaserPos_E_Range = MinLaserPos_E_Range;
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_E_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_E_Range, num2);
        MinLaserPos_E_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, minLaserPos_E_Range, num2);
    }

    private void Get_MinLaserPos_F_Pos()
    {
        if (MinLaserPos_F_UpOrDown)
        {
            MinLaserPos_F_Range -= 0.01f;
            if (MinLaserPos_F_Range <= 0f - MinLaserPos_F_Range_Limit)
            {
                MinLaserPos_F_UpOrDown = false;
            }
        }
        else if (!MinLaserPos_F_UpOrDown)
        {
            MinLaserPos_F_Range += 0.01f;
            if (MinLaserPos_F_Range >= MinLaserPos_F_Range_Limit)
            {
                MinLaserPos_F_UpOrDown = true;
            }
        }

        var minLaserPos_F_Range = MinLaserPos_F_Range;
        var drawPos = Building_DrakkenLaserDrill.DrawPos;
        var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
        var num = (drawPos - vector).AngleFlat();
        var num2 = num - 90f;
        if (num2 < 0f)
        {
            num2 = 360f + num2;
        }

        MinLaserPos_F_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, minLaserPos_F_Range, num2);
        MinLaserPos_F_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, minLaserPos_F_Range, num2);
    }

    private void Draw_MinLaserPos_Prepare()
    {
        if (Building_DrakkenLaserDrill.DamageNum >= 7)
        {
            Draw_MinLaserPos(MinLaserPos_A_UpOrDown, MinLaserPos_A_Start, MinLaserPos_A_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 8)
        {
            Draw_MinLaserPos(MinLaserPos_B_UpOrDown, MinLaserPos_B_Start, MinLaserPos_B_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 9)
        {
            Draw_MinLaserPos(MinLaserPos_C_UpOrDown, MinLaserPos_C_Start, MinLaserPos_C_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 10)
        {
            Draw_MinLaserPos(MinLaserPos_D_UpOrDown, MinLaserPos_D_Start, MinLaserPos_D_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 11)
        {
            Draw_MinLaserPos(MinLaserPos_E_UpOrDown, MinLaserPos_E_Start, MinLaserPos_E_End);
        }

        if (Building_DrakkenLaserDrill.DamageNum >= 12)
        {
            Draw_MinLaserPos(MinLaserPos_F_UpOrDown, MinLaserPos_F_Start, MinLaserPos_F_End);
        }
    }

    private void Draw_MinLaserPos(bool UpOrDown, Vector3 Start, Vector3 End)
    {
        var incOffset = 4f;
        if (!UpOrDown)
        {
            incOffset = 2f;
        }

        var pos = (Start + End) / 2f;
        pos.y = AltitudeLayer.PawnRope.AltitudeFor(incOffset);
        var lengthHorizontal = (Start.ToIntVec3() - End.ToIntVec3()).LengthHorizontal;
        var x = MinLaser_Width;
        if (LaserScaleTick < 30)
        {
            x = LaserScaleTick / 100f;
        }

        var angle = (Start - End).AngleFlat();
        var color = new Color(Building_DrakkenLaserDrill.Color_Min_Red / 255f,
            Building_DrakkenLaserDrill.Color_Min_Green / 255f, Building_DrakkenLaserDrill.Color_Min_Blue / 255f)
        {
            a = MinLaser_Alpha
        };
        var material = MaterialPool.MatFrom(ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser"),
            ShaderDatabase.Transparent, color);
        var matrix = default(Matrix4x4);
        matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
        Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
    }
}