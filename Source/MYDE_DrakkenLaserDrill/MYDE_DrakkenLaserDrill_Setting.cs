using Verse;

namespace MYDE_DrakkenLaserDrill;

public class MYDE_DrakkenLaserDrill_Setting : ModSettings
{
    public static float Base_ConsumePowerFactor = 6000f;

    public static int Extra_StoredEnergyMax;

    public static int Extra_DamageNumMax;

    public static float Extra_DamageArmorPenetrationMax;

    public static bool IfShowMessage;

    public static bool IfIgnoreMapRange;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Base_ConsumePowerFactor, "Base_ConsumePowerFactor");
        Scribe_Values.Look(ref Extra_StoredEnergyMax, "Extra_StoredEnergyMax");
        Scribe_Values.Look(ref Extra_DamageNumMax, "Extra_DamageNumMax");
        Scribe_Values.Look(ref Extra_DamageArmorPenetrationMax, "Extra_DamageArmorPenetrationMax");
        Scribe_Values.Look(ref IfShowMessage, "IfShowMessage");
        Scribe_Values.Look(ref IfIgnoreMapRange, "IfIgnoreMapRange");
    }

    public static void Initialization()
    {
        Base_ConsumePowerFactor = 6000f;
        Extra_StoredEnergyMax = 0;
        Extra_DamageNumMax = 0;
        Extra_DamageArmorPenetrationMax = 0f;
        IfShowMessage = false;
        IfIgnoreMapRange = false;
    }
}