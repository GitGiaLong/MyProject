namespace GSMFWPF.FontsIcon
{
    public interface ISpinable
    {
        /// <summary>
        /// Gets or sets the current spin (angle) animation of the icon.
        /// </summary>
        bool Spin { get; set; }

        /// <summary>
        /// Gets or sets the duration of the spinning animation (in seconds). This will stop and start the spin animation.
        /// </summary>
        double SpinDuration { get; set; }
    }
}
