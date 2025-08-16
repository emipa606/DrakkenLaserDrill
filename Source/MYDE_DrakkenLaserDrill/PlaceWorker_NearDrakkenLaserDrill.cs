using System.Collections.Generic;
using Verse;

namespace MYDE_DrakkenLaserDrill;

public class PlaceWorker_NearDrakkenLaserDrill : PlaceWorker
{
    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map,
        Thing thingToIgnore = null, Thing thing = null)
    {
        var allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
        var list = new List<Building>();
        foreach (var building in allBuildingsColonist)
        {
            if (building is Building_DrakkenLaserDrill)
            {
                list.Add(building);
            }
        }

        var list2 = new List<IntVec3>();
        foreach (var building in list)
        {
            var num = 3;
            for (var k = -num; k <= num; k++)
            {
                var item = building.Position + new IntVec3(k, 0, -4);
                list2.Add(item);
            }
        }

        GenDraw.DrawFieldEdges(list2);
        return list2.Contains(loc);
    }
}