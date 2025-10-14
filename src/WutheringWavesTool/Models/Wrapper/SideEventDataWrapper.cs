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
        public partial string Message { get; set; }

        [ObservableProperty]
        public partial double CurrentProgress { get; set; }

        public void Cali()
        {
            DateTime start = DateTime.Parse(StartTime);
            DateTime end = DateTime.Parse(EndTime);
            TimeSpan totalSpan = end - start;
            this.MaxProgress = totalSpan.TotalSeconds > 0 ? totalSpan.TotalSeconds : 0;

            TimeSpan elapsedSpan = DateTime.Now - start;
            double elapsed = elapsedSpan.TotalSeconds;
            if (elapsed < 0)
            {
                Message = "未开始";
                this.CurrentProgress = 0;
            }
            else if (elapsed >= this.MaxProgress)
            {
                Message = "已结束";
                this.CurrentProgress = this.MaxProgress;
            }
            else
            {
                Message = "进行中";
                this.CurrentProgress = elapsed;
            }
            TotalSpan = $"{elapsedSpan.Days}天{elapsedSpan.Hours}小时{elapsedSpan.Minutes}分";
        }
    }
}
