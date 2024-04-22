namespace GSMFWPF.FontsIcon
{
    public interface IPulsable
    {
        /// <summary>
        /// Gets or sets the state of the pulse animation
        /// </summary>
        bool Pulse { get; set; }

        /// <summary>
        /// Gets or sets the duration of the pulse animation
        /// </summary>
        double PulseDuration { get; set; }
    }
}
