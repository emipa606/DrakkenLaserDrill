using RimWorld;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[DefOf]
public static class MYDE_ThingDefOf
{
    public static ThingDef MYDE_Building_DrakkenLaserDrill;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_UpdateEnergy;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_UpdateDamage;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_UpdateAP;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon_Mouse;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon_ConcentratedBeam;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon_PulseCannon;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon_CrossMap;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon_Mouse_CrossMap;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon_ConcentratedBeam_CrossMap;

    public static ThingDef MYDE_Building_DrakkenLaserDrill_Beacon_PulseCannon_CrossMap;

    static MYDE_ThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(MYDE_ThingDefOf));
    }
}