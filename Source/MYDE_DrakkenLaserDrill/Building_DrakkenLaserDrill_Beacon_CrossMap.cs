using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill
{
    [StaticConstructorOnStartup]
    public class Building_DrakkenLaserDrill_Beacon_CrossMap : ThingWithComps
    {
        // [수정] 텍스처 캐싱
        private static readonly Texture2D Tex_Laser = ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser");

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

        // [오류 수정] private -> public 변경
        public int LaserScaleTick;

        public List<Thing> ListThing;

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

        private float Range;

        // [오류 수정] 누락된 변수 추가
        public float True_Map_Range;

        private Vector2 RealPos;
        private Vector3 SourcePos;
        private bool TakeDamageBool;
        private int TakeDamageTick;
        private int TakeDamageTickMax = 10;
        public Thing TargetThing;

        // [오류 수정] CheckSpawn 파라미터에 true_Map_Range 추가
        public void CheckSpawn(Building_DrakkenLaserDrill buildingDrakkenLaserDrill, Vector2 realPos,
            List<Thing> listThing, Thing targetThing, float angle, Vector3 sourcePos, float range, float true_Map_Range)
        {
            Building_DrakkenLaserDrill = buildingDrakkenLaserDrill;
            RealPos = realPos;
            ListThing = listThing;
            TargetThing = targetThing;
            Angle = angle;
            SourcePos = sourcePos;
            Range = range;
            True_Map_Range = true_Map_Range; // 값 할당
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
            Scribe_References.Look(ref TargetThing, "TargetThing");
            Scribe_Values.Look(ref TakeDamageBool, "TakeDamageBool");
            Scribe_Values.Look(ref TakeDamageTick, "TakeDamageTick");
            Scribe_Values.Look(ref TakeDamageTickMax, "TakeDamageTickMax");
            Scribe_Values.Look(ref RealPos, "RealPos");
            Scribe_Values.Look(ref Angle, "Angle");
            Scribe_Values.Look(ref SourcePos, "SourcePos");
            Scribe_Values.Look(ref Range, "Range");
            // [오류 수정] 저장 데이터 추가
            Scribe_Values.Look(ref True_Map_Range, "True_Map_Range");

            DeepProfiler.Start("Load All ListThing");
            Scribe_Collections.Look(ref ListThing, "ListThing", LookMode.Reference);
            DeepProfiler.End();
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
                var drawPos = DrawPos;
                var vector = SourcePos;
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

                var material = MaterialPool.MatFrom(Tex_Laser, ShaderDatabase.Transparent, color);
                var matrix = default(Matrix4x4);
                matrix.SetTRS(pos, Quaternion.AngleAxis(num, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
                Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
            }

            Draw_MinLaserPos_Prepare();
        }

        protected override void Tick()
        {
            base.Tick();
            var map = Map;
            var drawPos = TargetThing.DrawPos;

            var vector = new Vector3(RealPos.x, DrawPos.y, RealPos.y);
            var angle = (drawPos - vector).ToAngleFlat();

            var num = RealPos.x - drawPos.x;
            var num2 = RealPos.y - drawPos.z;
            var distance = System.Math.Min(0.5f, System.Math.Abs(((num * num) + (num2 * num2)) / 5f));
            RealPos = RealPos.Moved(angle, distance);
            Position = new IntVec3((int)RealPos.x, Position.y, (int)RealPos.y);

            var dataStatic = FleckMaker.GetDataStatic(vector, map,
                MYDE_FleckDefOf.MYDE_Building_DrakkenLaserDrill_Fleck_HeatGlow_Intense, 2f);
            map.flecks.CreateFleck(dataStatic);

            TakeDamageTick++;
            if (TakeDamageTick >= TakeDamageTickMax && LaserScaleTick >= 30)
            {
                TakeDamageTick = 0;
                var num3 = (num * num) + (num2 * num2);
                var num4 = 2f;
                TakeDamageBool = num3 <= num4;

                if (TakeDamageBool)
                {
                    Building_DrakkenLaserDrill.TargetThing = TargetThing;
                    Building_DrakkenLaserDrill.DamageTarget_CrossMap();
                }
            }

            LaserScaleTick++;
            Get_MinLaserPos_A_Pos();
            Get_MinLaserPos_B_Pos();
            Get_MinLaserPos_C_Pos();
            Get_MinLaserPos_D_Pos();
            Get_MinLaserPos_E_Pos();
            Get_MinLaserPos_F_Pos();
        }

        public void FindNextTarget(Thing OldTarget)
        {
            ListThing.Remove(OldTarget);
            if (ListThing.Count > 0)
            {
                var list = new List<Pawn>();
                var list2 = new List<Thing>();
                var num = 500f;
                for (var i = 0; i < ListThing.Count; i++)
                {
                    if (ListThing[i] is Pawn pawn)
                    {
                        if (!pawn.Dead)
                        {
                            if (pawn.Map == Map)
                            {
                                if (!pawn.Downed || Building_DrakkenLaserDrill.IfAttackDown)
                                {
                                    list.Add(pawn);
                                }
                                else if (pawn.Downed && !Building_DrakkenLaserDrill.IfAttackDown)
                                {
                                    ListThing.Remove(pawn);
                                }
                            }
                            else
                            {
                                ListThing.Remove(pawn);
                            }
                        }
                        else
                        {
                            ListThing.Remove(pawn);
                        }
                    }
                    else if (ListThing[i] is Building building)
                    {
                        if (!building.Destroyed)
                        {
                            list2.Add(building);
                        }
                        else
                        {
                            ListThing.Remove(building);
                        }
                    }
                }

                if (list.Count > 0)
                {
                    foreach (var target in list)
                    {
                        if (target != null && (Position - target.Position).LengthHorizontal < num)
                        {
                            TargetThing = target;
                            num = (Position - target.Position).LengthHorizontal;
                        }
                    }
                }
                else if (list2.Count > 0)
                {
                    foreach (var target in list2)
                    {
                        if ((Position - target.Position).LengthHorizontal < num)
                        {
                            TargetThing = target;
                            num = (Position - target.Position).LengthHorizontal;
                        }
                    }
                }
            }
            else
            {
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap = null;
                Destroy();
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (Building_DrakkenLaserDrill != null)
            {
                Building_DrakkenLaserDrill.Building_DrakkenLaserDrill_Beacon_CrossMap = null;
                Building_DrakkenLaserDrill.ChangePowerConsumeToZero();
            }
            base.Destroy(mode);
        }

        private void Get_MinLaserPos_A_Pos()
        {
            if (MinLaserPos_A_UpOrDown)
            {
                MinLaserPos_A_Range -= 0.01f;
                if (MinLaserPos_A_Range <= 0f - MinLaserPos_A_Range_Limit) MinLaserPos_A_UpOrDown = false;
            }
            else
            {
                MinLaserPos_A_Range += 0.01f;
                if (MinLaserPos_A_Range >= MinLaserPos_A_Range_Limit) MinLaserPos_A_UpOrDown = true;
            }

            var drawPos = DrawPos;
            var vector = SourcePos;
            var num = (drawPos - vector).AngleFlat();
            var num2 = num - 90f;
            if (num2 < 0f) num2 += 360f;

            MinLaserPos_A_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, MinLaserPos_A_Range, num2);
            MinLaserPos_A_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, MinLaserPos_A_Range, num2);
        }

        private void Get_MinLaserPos_B_Pos()
        {
            if (MinLaserPos_B_UpOrDown) { MinLaserPos_B_Range -= 0.01f; if (MinLaserPos_B_Range <= -MinLaserPos_B_Range_Limit) MinLaserPos_B_UpOrDown = false; }
            else { MinLaserPos_B_Range += 0.01f; if (MinLaserPos_B_Range >= MinLaserPos_B_Range_Limit) MinLaserPos_B_UpOrDown = true; }
            var drawPos = DrawPos; var vector = SourcePos; var num = (drawPos - vector).AngleFlat(); var num2 = num - 90f; if (num2 < 0f) num2 += 360f;
            MinLaserPos_B_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, MinLaserPos_B_Range, num2);
            MinLaserPos_B_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, MinLaserPos_B_Range, num2);
        }
        private void Get_MinLaserPos_C_Pos()
        {
            if (MinLaserPos_C_UpOrDown) { MinLaserPos_C_Range -= 0.01f; if (MinLaserPos_C_Range <= -MinLaserPos_C_Range_Limit) MinLaserPos_C_UpOrDown = false; }
            else { MinLaserPos_C_Range += 0.01f; if (MinLaserPos_C_Range >= MinLaserPos_C_Range_Limit) MinLaserPos_C_UpOrDown = true; }
            var drawPos = DrawPos; var vector = SourcePos; var num = (drawPos - vector).AngleFlat(); var num2 = num - 90f; if (num2 < 0f) num2 += 360f;
            MinLaserPos_C_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, MinLaserPos_C_Range, num2);
            MinLaserPos_C_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, MinLaserPos_C_Range, num2);
        }
        private void Get_MinLaserPos_D_Pos()
        {
            if (MinLaserPos_D_UpOrDown) { MinLaserPos_D_Range -= 0.01f; if (MinLaserPos_D_Range <= -MinLaserPos_D_Range_Limit) MinLaserPos_D_UpOrDown = false; }
            else { MinLaserPos_D_Range += 0.01f; if (MinLaserPos_D_Range >= MinLaserPos_D_Range_Limit) MinLaserPos_D_UpOrDown = true; }
            var drawPos = DrawPos; var vector = SourcePos; var num = (drawPos - vector).AngleFlat(); var num2 = num - 90f; if (num2 < 0f) num2 += 360f;
            MinLaserPos_D_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, MinLaserPos_D_Range, num2);
            MinLaserPos_D_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, MinLaserPos_D_Range, num2);
        }
        private void Get_MinLaserPos_E_Pos()
        {
            if (MinLaserPos_E_UpOrDown) { MinLaserPos_E_Range -= 0.01f; if (MinLaserPos_E_Range <= -MinLaserPos_E_Range_Limit) MinLaserPos_E_UpOrDown = false; }
            else { MinLaserPos_E_Range += 0.01f; if (MinLaserPos_E_Range >= MinLaserPos_E_Range_Limit) MinLaserPos_E_UpOrDown = true; }
            var drawPos = DrawPos; var vector = SourcePos; var num = (drawPos - vector).AngleFlat(); var num2 = num - 90f; if (num2 < 0f) num2 += 360f;
            MinLaserPos_E_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, MinLaserPos_E_Range, num2);
            MinLaserPos_E_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, MinLaserPos_E_Range, num2);
        }
        private void Get_MinLaserPos_F_Pos()
        {
            if (MinLaserPos_F_UpOrDown) { MinLaserPos_F_Range -= 0.01f; if (MinLaserPos_F_Range <= -MinLaserPos_F_Range_Limit) MinLaserPos_F_UpOrDown = false; }
            else { MinLaserPos_F_Range += 0.01f; if (MinLaserPos_F_Range >= MinLaserPos_F_Range_Limit) MinLaserPos_F_UpOrDown = true; }
            var drawPos = DrawPos; var vector = SourcePos; var num = (drawPos - vector).AngleFlat(); var num2 = num - 90f; if (num2 < 0f) num2 += 360f;
            MinLaserPos_F_Start = MYDE_ModFront.GetVector3_By_AngleFlat(drawPos, MinLaserPos_F_Range, num2);
            MinLaserPos_F_End = MYDE_ModFront.GetVector3_By_AngleFlat(vector, MinLaserPos_F_Range, num2);
        }

        private void Draw_MinLaserPos_Prepare()
        {
            if (Building_DrakkenLaserDrill.DamageNum >= 7) Draw_MinLaserPos(MinLaserPos_A_UpOrDown, MinLaserPos_A_Start, MinLaserPos_A_End);
            if (Building_DrakkenLaserDrill.DamageNum >= 8) Draw_MinLaserPos(MinLaserPos_B_UpOrDown, MinLaserPos_B_Start, MinLaserPos_B_End);
            if (Building_DrakkenLaserDrill.DamageNum >= 9) Draw_MinLaserPos(MinLaserPos_C_UpOrDown, MinLaserPos_C_Start, MinLaserPos_C_End);
            if (Building_DrakkenLaserDrill.DamageNum >= 10) Draw_MinLaserPos(MinLaserPos_D_UpOrDown, MinLaserPos_D_Start, MinLaserPos_D_End);
            if (Building_DrakkenLaserDrill.DamageNum >= 11) Draw_MinLaserPos(MinLaserPos_E_UpOrDown, MinLaserPos_E_Start, MinLaserPos_E_End);
            if (Building_DrakkenLaserDrill.DamageNum >= 12) Draw_MinLaserPos(MinLaserPos_F_UpOrDown, MinLaserPos_F_Start, MinLaserPos_F_End);
        }

        private void Draw_MinLaserPos(bool UpOrDown, Vector3 Start, Vector3 End)
        {
            var incOffset = 4f;
            if (!UpOrDown) incOffset = 2f;

            var pos = (Start + End) / 2f;
            pos.y = AltitudeLayer.PawnRope.AltitudeFor(incOffset);
            var lengthHorizontal = (Start.ToIntVec3() - End.ToIntVec3()).LengthHorizontal;
            var x = MinLaser_Width;
            if (LaserScaleTick < 30) x = LaserScaleTick / 100f;

            var angle = (Start - End).AngleFlat();
            var color = new Color(Building_DrakkenLaserDrill.Color_Min_Red / 255f,
                Building_DrakkenLaserDrill.Color_Min_Green / 255f, Building_DrakkenLaserDrill.Color_Min_Blue / 255f)
            {
                a = MinLaser_Alpha
            };

            var material = MaterialPool.MatFrom(Tex_Laser, ShaderDatabase.Transparent, color);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), new Vector3(x, 1f, lengthHorizontal));
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
        }
    }
}