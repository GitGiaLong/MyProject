using System.Windows.Controls;

namespace Core.Libraries.WPF.Controls.Texts
{
    /// <summary>
    /// text block used as scale
    /// </summary>
    internal class SpeTextBlock : TextBlock
    {
        public double X { get; set; }

        public SpeTextBlock() => Width = 60;

        public SpeTextBlock(double x) : this()
        {
            X = x;
            Canvas.SetLeft(this, X);
        }

        private DateTime _time;

        /// <summary>
        /// Time
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                Text = $"{value.ToString(TimeFormat)}\r\n|";
            }
        }

        /// <summary>
        /// Time format
        /// </summary>
        public string TimeFormat { get; set; } = "HH:mm";

        /// <summary>
        /// Lateral movement
        /// </summary>
        /// <param name="offsetX"></param>
        public void MoveX(double offsetX) => Canvas.SetLeft(this, X + offsetX);
    }
}
