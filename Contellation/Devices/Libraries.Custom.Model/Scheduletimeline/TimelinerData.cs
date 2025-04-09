namespace Libraries.Custom.Model.Scheduletimeline
{
    public class TimelinerData
    {
        public bool IsNeedSidePanel => Items?.Count() > 0 && Items.Any(x => !string.IsNullOrEmpty(x.Name));
        public List<TimelinerItem> Items { get; set; } = new List<TimelinerItem>();
    }
}
