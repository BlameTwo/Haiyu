namespace Haiyu.Models.Wrapper
{
    public partial class SideEventDataWrapper:ObservableObject
    {
        [ObservableProperty]
        public partial string ImageUrl { get; set; }
        [ObservableProperty]
        public partial string Title { get; set; }
        [ObservableProperty]
        public partial string StartTime { get; set; }
        [ObservableProperty]
        public partial string EndTime { get; set; }
        [ObservableProperty]
        public partial string TotalSpan { get; set; }

        [ObservableProperty]
        public partial double MaxProgress { get; set; }

        [ObservableProperty]
        public partial double CurrentProgress { get; set; }

        public void Cali()
        {
            this.MaxProgress = DateTime.Parse(EndTime).Second;
            this.CurrentProgress = (DateTime.Parse(EndTime) - DateTime.Now).TotalSeconds;
        }
    }
}
