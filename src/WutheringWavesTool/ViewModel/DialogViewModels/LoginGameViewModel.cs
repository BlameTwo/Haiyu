using Haiyu.Services.DialogServices;

namespace Haiyu.ViewModel.DialogViewModels;

public sealed partial class LoginGameViewModel : DialogViewModelBase
{
    private string? _loginType;

    public LoginGameViewModel(
        IAppContext<App> appContext,
        IViewFactorys viewFactorys,
        IKuroClient wavesClient,
        [FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager
    )
        : base(dialogManager)
    {
        AppContext = appContext;
        ViewFactorys = viewFactorys;
        WavesClient = wavesClient;

        RegisterMessanger();
    }

    private void RegisterMessanger()
    {
        this.Messenger.Register<GeeSuccessMessanger>(this, GeeSuccessMethod);
    }

    [ObservableProperty]
    public partial Visibility PhoneVisibility { get; set; }

    [ObservableProperty]
    public partial Visibility TokenVisibility { get; set; }

    [ObservableProperty]
    public partial string Phone { get; set; }

    [ObservableProperty]
    public partial string Code { get; set; }

    [ObservableProperty]
    public partial string Token { get; set; }

    [ObservableProperty]
    public partial string TokenId { get; set; }

    [ObservableProperty]
    public partial string TokenDid { get; set; }

    [ObservableProperty]
    public partial string TipMessage { get; set; }
    public string GeetValue { get; set; }

    public IAppContext<App> AppContext { get; }
    public IViewFactorys ViewFactorys { get; }
    public IKuroClient WavesClient { get; }

    private async void GeeSuccessMethod(object recipient, GeeSuccessMessanger message)
    {
        if (message.Type == GeetType.Login)
        {
            this.GeetValue = message.Result;
            if (string.IsNullOrWhiteSpace(GeetValue))
                return;
            var sendSMS = await WavesClient.SendSMSAsync(Phone, GeetValue);
            if (sendSMS == null)
            {
                TipMessage = "验证失败！";
                return;
            }
            if (sendSMS.Code == 242)
            {
                TipMessage = "短信验证码发送频繁！";
                return;
            }
            if (sendSMS.Data.GeeTest == false)
            {
                TipMessage = "验证码发送成功！";
            }
            else
            {
                TipMessage = "请再次点击获取验证码";
            }
        }
    }

    internal void SwitchView(string? v)
    {
        if (v == "Phone")
        {
            this.PhoneVisibility = Visibility.Visible;
            TokenVisibility = Visibility.Collapsed;
        }
        else
        {
            this.PhoneVisibility = Visibility.Collapsed;
            TokenVisibility = Visibility.Visible;
        }
        this._loginType = v;
    }

    [RelayCommand]
    void ShowGetGeet()
    {
        if (string.IsNullOrWhiteSpace(Phone))
            return;
        var view = ViewFactorys.CreateGeetWindow(GeetType.Login);
        view.AppWindowApp.Show();
    }

    [RelayCommand]
    async Task Login()
    {
        if (_loginType == "Phone")
        {
            var login = await WavesClient.LoginAsync(mobile: Phone, code: Code);
            if (!login.Success)
            {
                TipMessage = login.Msg;
                await Task.Delay(2000);
                return;
            }
            AppSettings.Token = login.Data.Token;
            AppSettings.TokenId = login.Data.UserId;
            AppSettings.TokenDid = "";
            WeakReferenceMessenger.Default.Send(
                new LoginMessanger(login.Success, login.Data.Token, long.Parse(login.Data.UserId))
            );
            DialogManager.CloseDialog();
        }
        else
        {
            AppSettings.Token = this.Token;
            AppSettings.TokenId = this.TokenId;
            AppSettings.TokenDid = this.TokenDid;
            var mine = await WavesClient.GetWavesMineAsync(long.Parse(this.TokenId));
            WeakReferenceMessenger.Default.Send(
                new LoginMessanger(true, Token, long.Parse(TokenId))
            );
            if (mine != null && mine.Code == 200)
            {
                DialogManager.CloseDialog();
            }
            else if (mine != null)
            {
                TipMessage = mine.Msg;
            }
            else
            {
                TipMessage = "错误！请反馈开发者";
            }
        }
    }
}
