namespace Core.Libraries.WPF.Controls.Datetimes.Scheduletimelines.Model
{
    public class TimelinerItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public System.Windows.Controls.Viewbox Icon { get; set; }
        public bool IsEnabled { get; set; }
        public List<TimelinerJob> Jobs { get; set; } = new List<TimelinerJob>();
    }
}
