using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class Building_UpdateDamage : Building
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
                building_DrakkenLaserDrill.DamageNumMax += 1 + MYDE_DrakkenLaserDrill_Setting.Extra_DamageNumMax;
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