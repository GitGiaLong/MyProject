using System.ComponentModel;

namespace Core.Libraries.WPF.Controls.Loadings
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
