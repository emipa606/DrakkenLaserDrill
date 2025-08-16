using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

public static class MYDE_ModFront
{
    public static Vector3 GetVector3_By_AngleFlat(Vector3 Center, float Range, float Angle)
    {
        var x = Center.x;
        var z = Center.z;
        var x2 = x - (Range * (float)Math.Sin(Angle * Math.PI / 180.0));
        var z2 = z - (Range * (float)Math.Cos(Angle * Math.PI / 180.0));
        return new Vector3(x2, Center.y, z2);
    }

    public static List<IntVec3> GetPos_Square(IntVec3 TargetPos, int CX, int CY)
    {
        var list = new List<IntVec3>();
        for (var i = -CX; i <= CX; i++)
        {
            for (var j = -CY; j <= CY; j++)
            {
                var item = TargetPos + new IntVec3(i, 0, j);
                list.Add(item);
            }
        }

        return list;
    }
}