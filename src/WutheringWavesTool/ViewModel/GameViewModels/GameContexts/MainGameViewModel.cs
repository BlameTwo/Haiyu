using System.Collections.ObjectModel;
using Haiyu.Services.DialogServices;

namespace Haiyu.ViewModel.GameViewModels;

public sealed partial class MainGameViewModel : GameContextViewModelBase
{
    public MainGameViewModel(
        [FromKeyedServices(nameof(MainGameContext))] IGameContext gameContext,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,
        IAppContext<App> appContext,
        ITipShow tipShow
    )
        : base(gameContext, dialogManager, appContext, tipShow) { }

    [ObservableProperty]
    public partial ObservableCollection<Slideshow> SlideShows { get; set; }
    #region Datas
    public Notice Notice { get; private set; }
    public News News { get; private set; }
    public Waves.Api.Models.Activity Activity { get; private set; }
    #endregion

    [ObservableProperty]
    public partial bool IsOpen { get; set; } = true;

    [ObservableProperty]
    public partial bool TabLoad { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<Content> Contents { get; set; } = new();

    public override async Task LoadAfter()
    {
        TabLoad = false;
        var starter = await this.GameContext.GetLauncherStarterAsync(this.CTS.Token);
        if (starter == null)
            return;
        this.SlideShows = starter.Slideshow.ToObservableCollection();
        this.Notice = starter.Guidance.Notice;
        this.News = starter.Guidance.News;
        this.Activity = starter.Guidance.Activity;
        TabLoad = true;
    }

    [RelayCommand]
    async Task CardLoaded()
    {
        await Task.Delay(500);
        IsOpen = true;
    }

    internal void SelectTab(string text)
    {
        this.Contents.Clear();
        switch (text)
        {
            case "活动":
                this.Contents = this.Activity.Contents.ToObservableCollection();
                break;
            case "公告":
                this.Contents = this.Notice.Contents.ToObservableCollection();
                break;
            case "新闻":
                this.Contents = this.News.Contents.ToObservableCollection();
                break;
        }
    }

    public override void DisposeAfter()
    {
        if (this.Contents != null)
            this.Contents.Clear();
        if (this.Activity != null)
        {
            this.Activity.Contents.Clear();
            this.Activity.Contents = null;
        }
        if (this.Notice != null)
        {
            this.Notice.Contents.Clear();
            this.Notice.Contents = null;
        }
        if (this.News != null)
        {
            this.News.Contents.Clear();
            this.News.Contents = null;
        }
        if (this.SlideShows != null)
        {
            this.SlideShows.Clear();
            this.SlideShows = null;
        }
    }
}
