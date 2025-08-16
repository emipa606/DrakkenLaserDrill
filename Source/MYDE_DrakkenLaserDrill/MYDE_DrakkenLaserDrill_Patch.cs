using System.Reflection;
using HarmonyLib;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class MYDE_DrakkenLaserDrill_Patch
{
    static MYDE_DrakkenLaserDrill_Patch()
    {
        new Harmony("KongYao.MYDE_DrakkenLaserDrill").PatchAll(Assembly.GetExecutingAssembly());
    }
}