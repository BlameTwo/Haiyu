﻿using WutheringWavesTool.Services.DialogServices;

namespace WutheringWavesTool.Common.Bases;

public abstract partial class DialogViewModelBase : ViewModelBase
{
    public DialogViewModelBase(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager
    )
    {
        DialogManager = dialogManager;
    }

    public ContentDialogResult? Result { get; set; }
    public IDialogManager DialogManager { get; }

    [RelayCommand]
    protected void Close()
    {
        if (Result == null)
            this.Result = ContentDialogResult.None;
        BeforeClose();
        DialogManager.CloseDialog();
        AfterClose();
    }

    public virtual void BeforeClose() { }

    public virtual void AfterClose() { }
}
