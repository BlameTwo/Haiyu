using Haiyu.Models.Wrapper.WindowRoils;

namespace Haiyu.ViewModel.Communitys.WinViewModel;

public sealed partial class GamerRoilsDetilyViewModel : ViewModelBase, IDisposable
{
    public GamerRoilsDetilyViewModel(
        IWavesClient wavesClient,
        GamerRoilViewModel gamerRoilViewModel
    )
    {
        WavesClient = wavesClient;
        GamerRoilViewModel = gamerRoilViewModel;
    }

    public ShowRoleData Data { get; internal set; }
    public IWavesClient WavesClient { get; }

    [ObservableProperty]
    public partial ObservableCollection<INavigationRoilsItem> Roles { get; set; }
    public long? SelectCache { get; internal set; }

    [ObservableProperty]
    public partial INavigationRoilsItem SelectItem { get; set; }

    [ObservableProperty]
    public partial GamerRoilViewModel GamerRoilViewModel { get; set; }

    [ObservableProperty]
    public partial string Title { get; set; }


    [RelayCommand]
    async Task Loaded()
    {
        await Task.Delay(200);
        var GameRoil = await WavesClient.GetGamerRoleDataAsync(
            Data.GameRoilDataItem,
            this.CTS.Token
        );
        if (GameRoil == null)
            return;
        Roles = new();
        var group = GameRoil.RoleList.GroupBy(x => x.AttributeId);
        foreach (var item in group)
        {
            this.Roles.Add(new NavigationRoilsTypeItem(item.First().AttributeName));
            foreach (var gr in item)
            {
                Roles.Add(new NavigationRoilsDetilyItem(gr, Data.GameRoilDataItem));
            }
        }
        var first = this.Roles.First(x =>
        {
            if (x is NavigationRoilsDetilyItem)
                return true;
            return false;
        });
        if (this.SelectCache != null)
        {
            var filter = this
                .Roles.Where(x =>
                {
                    if (x is NavigationRoilsDetilyItem item && item.RoilId == this.SelectCache)
                    {
                        return true;
                    }
                    return false;
                })
                .FirstOrDefault();
            if (filter != null)
            {
                this.SelectItem = filter;
            }
        }
        else if (first != null)
        {
            if (first != null)
            {
                this.SelectItem = first;
            }
            else
            {
                throw new Exception("没有角色，你的账号数据为空");
            }
        }
        else
        {
            throw new Exception("没有角色，你的账号数据为空");
        }
    }

    internal async Task SwitchPage(NavigationRoilsDetilyItem item)
    {
        this.Title = item.RoleName;
        await this.GamerRoilViewModel.SetDataAsync(item);
    }

    public void Dispose()
    {
        if (Roles != null)
        {
            Roles.Clear();
            this.GamerRoilViewModel.Dispose();
        }
        Roles = null;
        SelectItem = null;
    }
}
