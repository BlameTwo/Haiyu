﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Waves.Api.Helper;
using Waves.Api.Models.Enums;
using Waves.Api.Models.Record;
using Waves.Api.Models.Wrappers;
using WutheringWavesTool.Common;
using WutheringWavesTool.Models.Args;
using WutheringWavesTool.Models.Wrapper;

namespace WutheringWavesTool.ViewModel.Record;

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
        if (DataItem.Type == CardPoolType.RoleActivity)
        {
            StarItems = RecordHelper
                .FormatStartFive(
                    this.Items,
                    RecordHelper.FormatFiveRoleStar(this.DataItem.FiveGroup!)
                )!
                .Format(this.DataItem.AllRole)
                .Reverse()
                .ToCardItemObservableCollection();
        }
        if (DataItem.Type == CardPoolType.WeaponsActivity)
        {
            StarItems = RecordHelper
                .FormatStartFive(
                    this.Items,
                    RecordHelper.FormatFiveWeaponeRoleStar(this.DataItem.FiveGroup!)
                )!
                .Format(this.DataItem.AllWeapon)
                .Reverse()
                .ToCardItemObservableCollection();
        }
        if (DataItem.Type == CardPoolType.WeaponsResident)
        {
            StarItems = RecordHelper
                .FormatRecordFive(this.Items)!
                .Format(this.DataItem.AllWeapon)
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
                .Format(this.DataItem.AllRole)
                .Reverse()
                .ToCardItemObservableCollection();
        }
        var result = RecordHelper.GetAdvanceData(Items);
        MakeCount = result.Item2;
    }
}
