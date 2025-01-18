﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Waves.Api.Models.Record;
using WutheringWavesTool.Common;
using WutheringWavesTool.Models.Args;
using WutheringWavesTool.Services;
using WutheringWavesTool.Services.Contracts;
using static System.Formats.Asn1.AsnWriter;

namespace WutheringWavesTool.ViewModel.DialogViewModels;

public partial class InputRecordCardViewModel : ObservableObject
{
    public InputRecordCardViewModel(IServiceScope scope)
    {
        DialogManager = scope.ServiceProvider.GetRequiredService<IDialogManager>();
        TipShow = scope.ServiceProvider.GetRequiredService<ITipShow>();
        RecordCacheService = scope.ServiceProvider.GetRequiredService<IRecordCacheService>();
    }

    [ObservableProperty]
    public partial string Link { get; set; }

    partial void OnLinkChanged(string value)
    {
        this.InvokeCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    async Task Loaded()
    {
        this.CacheItem = (
            await RecordCacheService.GetRecordCacheDetilyAsync()
        ).ToObservableCollection();
    }

    [ObservableProperty]
    public partial ObservableCollection<RecordCacheDetily?> CacheItem { get; private set; }

    public bool GetIsInvoke() => !string.IsNullOrWhiteSpace(Link) || !(SelectItem == null);

    [RelayCommand]
    public void Close()
    {
        this.Args = null;
        TipShow.ShowMessage("抽卡链接为空", Microsoft.UI.Xaml.Controls.Symbol.Clear);
        this.DialogManager.Close();
    }

    [RelayCommand(CanExecute = nameof(GetIsInvoke))]
    public void Invoke()
    {
        if (!string.IsNullOrWhiteSpace(Link))
        {
            this.Args = new CreateRecordArgs(CreateRecordType.Create)
            {
                Link = this.Link,
                Cache = null,
            };
        }
        else if (this.SelectItem != null)
        {
            this.Args = new CreateRecordArgs(CreateRecordType.Create)
            {
                Link = this.Link,
                Cache = this.SelectItem,
            };
        }
        this.DialogManager.Close();
    }

    [RelayCommand(CanExecute = nameof(GetIsInvoke))]
    public void Update()
    {
        this.Args = new CreateRecordArgs(CreateRecordType.Update)
        {
            Link = this.Link,
            Cache = null,
        };
        this.DialogManager.Close();
    }

    public IServiceScope Scope { get; }
    public IDialogManager DialogManager { get; }
    public ITipShow TipShow { get; }
    public IRecordCacheService RecordCacheService { get; }
    public RecordCacheDetily SelectItem { get; internal set; }

    public CreateRecordArgs? Args { get; internal set; }
}
