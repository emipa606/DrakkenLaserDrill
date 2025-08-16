using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Building_UpdateAP : Building
{
    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        var c = Position + new IntVec3(0, 0, 1);
        var thingList = c.GetThingList(map);
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < thingList.Count; i++)
        {
            if (thingList[i] is not Building_DrakkenLaserDrill)
            {
                continue;
            }

            if (thingList[i] is Building_DrakkenLaserDrill building_DrakkenLaserDrill)
            {
                building_DrakkenLaserDrill.DamageArmorPenetrationMax +=
                    0.1f + MYDE_DrakkenLaserDrill_Setting.Extra_DamageArmorPenetrationMax;
                building_DrakkenLaserDrill.DamageArmorPenetration =
                    building_DrakkenLaserDrill.DamageArmorPenetrationMax;
            }

            Destroy();
            break;
        }

        if (!Destroyed)
        {
            Destroy();
        }
    }
}