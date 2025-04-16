using System.ComponentModel;

namespace Libraries.Custom.Enums.Control
{

    public enum LoadingMode
    {
        [Description("LoadingWaveStyle")]
        Wave,

        [Description("LoadingArcStyle")]
        Arc,

        [Description("LoadingArcsStyle")]
        Arcs,

        [Description("LoadingArcsRingStyle")]
        ArcsRing,

        [Description("LoadingDoubleBounceStyle")]
        DoubleBounce,

        [Description("LoadingFlipPlaneStyle")]
        FlipPlane,

        [Description("LoadingPulseStyle")]
        Pulse,

        [Description("LoadingRingStyle")]
        Ring,

        [Description("LoadingThreeDotsStyle")]
        ThreeDots
    }
}
