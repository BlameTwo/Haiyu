using System;
using WutheringWavesTool.Models.Wrapper.CommunityWorld;

namespace WutheringWavesTool.ViewModel.Communitys;

public partial class GamerExploreIndexViewModel : ViewModelBase, IDisposable
{
    private bool disposedValue;
    private GameRoilDataItem? _roilData;

    public IWavesClient WavesClient { get; }
    public ITipShow TipShow { get; }

    [ObservableProperty]
    public partial ObservableCollection<ExploreIndexCountry> Countrys { get; set; } = new();

    [ObservableProperty]
    public partial ObservableCollection<DataCenterExploreCountryItem> CountrysItems { get; set; } =
        new();

    [ObservableProperty]
    public partial ExploreIndexCountry SelectCountry { get; set; }
    public GamerExploreIndexData? BassData { get; private set; }

    public GamerExploreIndexViewModel(IWavesClient wavesClient, ITipShow tipShow)
    {
        WavesClient = wavesClient;
        TipShow = tipShow;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        this._roilData = message.Data.Item;
        await RefreshDataAsync();
    }

    internal async Task SetDataAsync(GameRoilDataItem item)
    {
        this._roilData = item;
    }

    partial void OnSelectCountryChanged(ExploreIndexCountry value)
    {
        if (value == null || BassData == null)
        {
            TipShow.ShowMessage("数据拉取失败！", Symbol.Clear);
            return;
        }
        var country = this
            .BassData.ExploreList.Where(x => x.Country.CountryId == value.CountryId)
            .FirstOrDefault();
        if (country == null)
        {
            TipShow.ShowMessage("数据拉取失败！", Symbol.Clear);
            return;
        }
        this.CountrysItems.Clear();
        foreach (var item in country.AreaInfoList)
        {
            this.CountrysItems.Add(new(item));
        }
    }

    [RelayCommand]
    async Task LoadedAsync()
    {
        await RefreshDataAsync();
    }

    private async Task RefreshDataAsync()
    {
        Countrys.Clear();
        if (this._roilData == null)
        {
            TipShow.ShowMessage("玩家数据拉取失败！", Microsoft.UI.Xaml.Controls.Symbol.Clear);
            return;
        }
        var data = await TryInvokeAsync(WavesClient.GetGamerExploreIndexDataAsync(
            this._roilData,
            this.CTS.Token
        ));
        if(data.Item1 != 0)
        {
            TipShow.ShowMessage(data.Item3, Microsoft.UI.Xaml.Controls.Symbol.Clear);
            return;
        }
        this.BassData = data.Item2;
        foreach (var item in BassData.ExploreList)
        {
            Countrys.Add(new(item));
        }
        this.SelectCountry = Countrys[0];
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                WeakReferenceMessenger.Default.UnregisterAll(this);
                this.Messenger.UnregisterAll(this);
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
