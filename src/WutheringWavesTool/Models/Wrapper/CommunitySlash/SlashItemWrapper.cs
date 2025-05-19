using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WutheringWavesTool.Models.Wrapper.CommunitySlash;

public sealed partial class SlashItemWrapper : ObservableObject
{
    [ObservableProperty]
    public partial int SlashItemId { get; set; }

    [ObservableProperty]
    public partial string ItemName { get; set; }

    [ObservableProperty]
    public partial string Rank { get; set; }

    [ObservableProperty]
    public partial int Score { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<HalfList> Halfs { get; set; }

    public static ObservableCollection<SlashItemWrapper> Convert(List<SlashChallengeList> values)
    {
        var value = new ObservableCollection<SlashItemWrapper>();
        foreach (var item in values)
        {
            value.Add(
                new SlashItemWrapper()
                {
                    SlashItemId = item.ChallengeId,
                    ItemName = item.ChallengeName,
                    Rank = item.Rank,
                    Score = item.Score,
                    Halfs = item.HalfList,
                }
            );
        }
        return value;
    }
}
