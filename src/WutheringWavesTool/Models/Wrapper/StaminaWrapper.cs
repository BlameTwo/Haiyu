using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiyu.Models.Wrapper;

public partial class StaminaWrapper : ObservableObject
{
    public StaminaWrapper(GamerBassData data)
    {
        this.Energy = data.Energy;
        this.MaxEnergy = data.MaxEnergy;
        this.StoreEnergy = data.StoreEnergy;
        this.MaxStoreEnergy = data.StoreEnergyLimit;
        this.Liveness = data.Liveness;
        this.MaxLiveness = data.LivenessMaxCount;
        this.WeekyInstCount = data.WeeklyInstCount;
        this.MaxWeekyInstCount = data.WeeklyInstCountLimit;
        this.RougeScore = data.RougeScore;
        this.MaxRougeScore = data.RougeScoreLimit;
        this.Name = data.Name;
        this.UserLevel = data.Level;
    }

    #region User
    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial int UserLevel { get; set; }
    #endregion

    #region 结晶波片

    [ObservableProperty]
    public partial int Energy { get; set; }

    [ObservableProperty]
    public partial int MaxEnergy { get; set; }

    #endregion

    #region 结晶单质
    [ObservableProperty]
    public partial int StoreEnergy { get; set; }

    [ObservableProperty]
    public partial int MaxStoreEnergy { get; set; }
    #endregion

    #region 活跃度
    [ObservableProperty]
    public partial int Liveness { get; set; }

    [ObservableProperty]
    public partial int MaxLiveness { get; set; }
    #endregion

    #region 战歌重奏
    [ObservableProperty]
    public partial int WeekyInstCount { get; set; }

    [ObservableProperty]
    public partial int MaxWeekyInstCount { get; set; }
    #endregion

    #region 千道门扉的异想

    [ObservableProperty]
    public partial int RougeScore { get; set; }

    [ObservableProperty]
    public partial int MaxRougeScore { get; set; }
    #endregion
}
