using System.Threading.Tasks;
using Windows.Web.AtomPub;
using Haiyu.Helpers;
using Haiyu.Models.Wrapper.WindowRoils;

namespace Haiyu.ViewModel.Communitys.WinViewModel;

public sealed partial class GamerRoilViewModel : ViewModelBase, IDisposable
{
    private bool disposedValue;

    public NavigationRoilsDetilyItem ItemData { get; private set; }
    public IWavesClient WavesClient { get; }

    public GamerRoilViewModel(IWavesClient wavesClient)
    {
        WavesClient = wavesClient;
    }

    internal async Task SetDataAsync(NavigationRoilsDetilyItem? navigationRoilsDetilyItem)
    {
        if (navigationRoilsDetilyItem != null)
            this.ItemData = navigationRoilsDetilyItem;
        await this.Loaded();
    }

    #region RoleData
    [ObservableProperty]
    public partial BitmapImage RolePic { get; set; }

    [ObservableProperty]
    public partial string RoleName { get; set; }

    [ObservableProperty]
    public partial int RoleLevel { get; set; }

    [ObservableProperty]
    public partial int RoleStar { get; set; }

    [ObservableProperty]
    public partial BitmapImage AttImage { get; set; }
    #endregion

    #region Weapon
    [ObservableProperty]
    public partial BitmapImage WeaponImage { get; set; }

    [ObservableProperty]
    public partial string WeaponName { get; set; }

    [ObservableProperty]
    public partial string WeaponTypeName { get; set; }

    [ObservableProperty]
    public partial int WeaponLevel { get; set; }

    [ObservableProperty]
    public partial int WeaponStarLevel { get; set; }

    [ObservableProperty]
    public partial string WeaponSession { get; set; }

    [ObservableProperty]
    public partial int WeaponReason { get; set; }

    #endregion

    #region Skill
    [ObservableProperty]
    public partial ObservableCollection<SkillList> Skills { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<EquipPhantomList> PhantomData { get; private set; }

    [ObservableProperty]
    public partial ObservableCollection<ChainList> Chains { get; set; }
    #endregion

    #region Visibility
    [ObservableProperty]
    public partial Visibility WeaponVisibility { get; set; } = Visibility.Visible;

    [ObservableProperty]
    public partial Visibility ChainVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility PhantomDataVisibility { get; set; } = Visibility.Collapsed;
    #endregion

    #region Phantom
    [ObservableProperty]
    public partial string PhatomTypeName { get; set; }

    [ObservableProperty]
    public partial BitmapImage PhatomTypeImage { get; set; }

    [ObservableProperty]
    public partial bool PhatomVisibility { get; set; }
    #endregion

    async Task Loaded()
    {
        var result = await WavesClient.GetGamerRoilDetily(this.ItemData.Item, this.ItemData.RoilId);
        if (result == null)
        {
            return;
        }
        this.RolePic = new(new(result.Role.RolePicUrl));
        this.RoleName = result.Role.RoleName;
        this.RoleLevel = result.Role.Level;
        this.RoleStar = result.Role.StarLevel;
        this.AttImage = new BitmapImage(new(RoleHelper.SwitchType(result.Role.AttributeId)));
        //武器与技能
        this.WeaponImage = new(new(result.WeaponData.Weapon.WeaponIcon));
        this.WeaponName = result.WeaponData.Weapon.WeaponName;
        this.WeaponLevel = result.WeaponData.Level;
        this.WeaponStarLevel = result.WeaponData.Weapon.WeaponStarLevel;
        this.WeaponTypeName = result.WeaponData.Weapon.WeaponEffectName.ToString();
        this.WeaponSession = result.WeaponData.Weapon.EffectDescription;
        this.WeaponReason = result.WeaponData.ResonLevel;
        this.Skills = result.SkillList.ToObservableCollection();
        if (result.PhantomData.Cost != 0)
        {
            this.PhantomData = result.PhantomData.EquipPhantomList.ToObservableCollection();
            this.PhatomVisibility = true;
        }
        else
        {
            this.PhatomVisibility = false;
        }
        this.Chains = result.ChainList.ToObservableCollection();
    }

    internal void SetPage(string? v)
    {
        if (v == "Weapon")
        {
            WeaponVisibility = Visibility.Visible;
            ChainVisibility = Visibility.Collapsed;
            PhantomDataVisibility = Visibility.Collapsed;
        }
        else if (v == "Chain")
        {
            WeaponVisibility = Visibility.Collapsed;
            ChainVisibility = Visibility.Visible;
            PhantomDataVisibility = Visibility.Collapsed;
        }
        else if (v == "PhantomData")
        {
            WeaponVisibility = Visibility.Collapsed;
            ChainVisibility = Visibility.Collapsed;
            PhantomDataVisibility = Visibility.Visible;
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.CTS.Cancel();
                if (PhantomData != null)
                    PhantomData.Clear();
                if (Chains != null)
                    Chains.Clear();
                if (AttImage != null)
                    Skills.Clear();
                AttImage = null;
                WeaponImage = null;
                RolePic = null;
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
