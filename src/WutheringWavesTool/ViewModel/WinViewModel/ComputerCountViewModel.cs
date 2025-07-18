using CounterMonitor.Models;
using WutheringWavesTool.WindowsView;

namespace WutheringWavesTool.ViewModel.WinViewModel;

public partial class ComputerCountViewModel : ObservableRecipient
{
    public ICounterService CounterService { get; }
    public ComputerCountWindow Window { get; set; }

    public ComputerCountViewModel(ICounterService counterService)
    {
        CounterService = counterService;
    }

    [ObservableProperty]
    public partial ObservableCollection<ComputerItem> ComputerItems { get; set; }

    [ObservableProperty]
    public partial int FpsCounter { get; set; }

    [ObservableProperty]
    public partial string CurrentApplication { get; set; }

    [RelayCommand]
    void Loaded()
    {
        if (string.IsNullOrWhiteSpace(AppSettings.AreaCounterPostion))
        {
            Window.SetPostion(Models.Enums.PostionType.LeftTop);
        }
        CounterService.Start();
        CounterService.FpsCounter.FpsOutputChanged += FpsCounter_FpsOutputChanged;
        CounterService.ComputerCounter.ComputerOutput += ComputerCounter_ComputerOutput;
    }

    private void ComputerCounter_ComputerOutput(
        object sender,
        IEnumerable<CounterMonitor.Models.ComputerItem> computers
    )
    {
        this.Window.DispatcherQueue.TryEnqueue(() =>
        {
            this.ComputerItems = computers.ToObservableCollection();
        });
    }

    private void FpsCounter_FpsOutputChanged(object sender, CounterMonitor.Models.FpsOutput outPut)
    {
        this.Window.DispatcherQueue.TryEnqueue(() =>
        {
            CurrentApplication = outPut.Name;
            this.FpsCounter = outPut.FPS;
        });
    }

    public void Close()
    {
        CounterService.FpsCounter.FpsOutputChanged -= FpsCounter_FpsOutputChanged;
        CounterService.ComputerCounter.ComputerOutput -= ComputerCounter_ComputerOutput;
        CounterService.Stop();
    }

    internal void Clear()
    {
        this.Close();
        ComputerItems.Clear();
    }
}
