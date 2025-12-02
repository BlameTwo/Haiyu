namespace Haiyu.ViewModel.Communitys;

public sealed partial class GamerSkinViewModel : ViewModelBase
{
    private GameRoilDataItem roil;
    public IKuroClient WavesClient { get; }
    public ITipShow TipShow { get; }

    [ObservableProperty]
    public partial ObservableCollection<RoleSkinList> RoleSkins { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<WeaponSkinList> WeaponSkins { get; set; }

    public GamerSkinViewModel(IKuroClient wavesClient, ITipShow tipShow)
    {
        WavesClient = wavesClient;
        TipShow = tipShow;
        WeakReferenceMessenger.Default.Register<SwitchRoleMessager>(this, SwitchRoleMethod);
    }

    private async void SwitchRoleMethod(object recipient, SwitchRoleMessager message)
    {
        this.roil = message.Data.Item;
        await Loaded();
    }

    public void SetData(GameRoilDataItem item)
    {
        this.roil = item;
    }

    [RelayCommand]
    async Task Loaded()
    {
        var skin = await TryInvokeAsync(async () => await this.WavesClient.GetGamerSkinAsync(roil, this.CTS.Token));
        if (skin.Item1 != 0)
        {
            
            return;
        }
        this.RoleSkins = skin.Item2.RoleSkinList.ToObservableCollection();
        this.WeaponSkins = skin.Item2.WeaponSkinList.ToObservableCollection();
    }


    public override void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        this.RoleSkins.RemoveAll();
        this.WeaponSkins.RemoveAll();
    }
}
