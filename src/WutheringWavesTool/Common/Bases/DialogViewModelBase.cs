using Haiyu.Services.DialogServices;
using Waves.Core.Services;

namespace Haiyu.Common.Bases;

public abstract partial class DialogViewModelBase : ViewModelBase
{
    public DialogViewModelBase(
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager
    )
    {
        DialogManager = dialogManager;
        this.Logger = Instance.Service.GetKeyedService<LoggerService>("AppLog");
    }

    public ContentDialogResult? Result { get; set; }
    public IDialogManager DialogManager { get; }
    public LoggerService Logger { get; }

    [RelayCommand]
    protected void Close()
    {
        if (Result == null)
            this.Result = ContentDialogResult.None;
        BeforeClose();
        DialogManager.CloseDialog();
        AfterClose();
        
        GC.SuppressFinalize(this);
    }

    public virtual void BeforeClose() { }

    public virtual void AfterClose() { }
}
