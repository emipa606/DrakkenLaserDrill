using UnityEngine;
using Verse;

namespace MYDE_DrakkenLaserDrill;

[StaticConstructorOnStartup]
public class MYDE_TexButton
{
    public static readonly Texture2D Background = SolidColorMaterials.NewSolidColorTexture(Color.gray);

    public static readonly Texture2D DrakkenLaserDrill_Laser =
        ContentFinder<Texture2D>.Get("DrakkenLaserDrill_Laser/Laser_Setting");
}