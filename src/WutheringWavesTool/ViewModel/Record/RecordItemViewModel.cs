namespace Haiyu.ViewModel.Record;

public sealed partial class RecordItemViewModel : ViewModelBase
{
    public CardPoolType Type { get; private set; }
    public RecordRequest Request { get; private set; }
    public IList<RecordCardItemWrapper> Items { get; set; }

    public RecordArgs DataItem { get; private set; }

    [ObservableProperty]
    public partial double MakeCount { get; set; } = 0.0;

    [ObservableProperty]
    public partial ObservableCollection<RecordActivityFiveStarItemWrapper> StarItems { get; set; }

    internal void SetData(RecordArgs item)
    {
        this.DataItem = item;
        switch (item.Type)
        {
            case Waves.Api.Models.Enums.CardPoolType.RoleActivity:
                this.Items = item.RoleActivity.ToList();
                break;
            case Waves.Api.Models.Enums.CardPoolType.WeaponsActivity:
                this.Items = item.WeaponsActivity.ToList();
                break;
            case Waves.Api.Models.Enums.CardPoolType.RoleResident:
                this.Items = item.RoleResident.ToList();
                break;
            case Waves.Api.Models.Enums.CardPoolType.WeaponsResident:
                this.Items = item.WeaponsResident.ToList();
                break;
            case Waves.Api.Models.Enums.CardPoolType.Beginner:
                this.Items = item.Beginner.ToList();
                break;
            case Waves.Api.Models.Enums.CardPoolType.BeginnerChoice:
                this.Items = item.BeginnerChoice.ToList();
                break;
            case Waves.Api.Models.Enums.CardPoolType.GratitudeOrientation:
                this.Items = item.GratitudeOrientation.ToList();
                break;
        }
    }

    [RelayCommand]
    async Task Loaded()
    {
        await Task.Delay(100);
        int lastCount;
        if (DataItem.Type == CardPoolType.RoleActivity)
        {
            StarItems = RecordHelper
                .FormatStartFive(
                    this.Items,
                    out lastCount,
                    RecordHelper.FormatFiveRoleStar(this.DataItem.FiveGroup!)
                )!
                .Item1
                .Format(this.DataItem.AllRole,false)
                .Reverse()
                .ToCardItemObservableCollection();
        }
        if (DataItem.Type == CardPoolType.WeaponsActivity)
        {
            StarItems = RecordHelper
                .FormatStartFive(
                    this.Items,
                    out lastCount,
                    RecordHelper.FormatFiveWeaponeRoleStar(this.DataItem.FiveGroup!)
                )!
                .Item1
                .Format(this.DataItem.AllWeapon,false)
                .Reverse()
                .ToCardItemObservableCollection();
        }
        if (DataItem.Type == CardPoolType.WeaponsResident)
        {
            StarItems = RecordHelper
                .FormatRecordFive(this.Items)!
                .Format(this.DataItem.AllWeapon,false)
                .Reverse()
                .ToCardItemObservableCollection();
        }
        if (
            DataItem.Type == CardPoolType.RoleResident
            || DataItem.Type == CardPoolType.GratitudeOrientation
            || DataItem.Type == CardPoolType.Beginner
            || DataItem.Type == CardPoolType.BeginnerChoice
        )
        {
            StarItems = RecordHelper
                .FormatRecordFive(this.Items)!
                .Format(this.DataItem.AllRole,false)
                .Reverse()
                .ToCardItemObservableCollection();
        }
        var result = RecordHelper.GetAdvanceData(Items);
        MakeCount = result.Item2;
    }
}
