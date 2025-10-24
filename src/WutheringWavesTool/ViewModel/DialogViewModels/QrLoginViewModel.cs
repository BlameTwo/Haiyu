﻿
using Microsoft.Graphics.Canvas;
using Waves.Api.Models.QRLogin;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Haiyu.Common.QR;
using Haiyu.Models.Dialogs;
using Haiyu.Services.DialogServices;
using ZXing;
using ZXing.Common;

namespace Haiyu.ViewModel.DialogViewModels;

public partial class QrLoginViewModel : DialogViewModelBase
{
    public QrLoginViewModel([FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,IAppContext<App> appContext,IWavesClient wavesClient) : base(dialogManager)
    {
        AppContext = appContext;
        WavesClient = wavesClient;
    }

    private CanvasDevice _canvasDevice;
    private Direct3D11CaptureFramePool? _framePool;
    private GraphicsCaptureSession? _session;

    public IAppContext<App> AppContext { get; }
    public IWavesClient WavesClient { get; }

    [ObservableProperty]
    public partial Visibility SelectWindowVisibility { get; set; } = Visibility.Visible;

    [ObservableProperty]
    public partial Visibility LoginBthVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility VerifyCodeVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial Visibility SessionVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial string ScreenMessage { get; set; } = "选择显示器";

    [ObservableProperty]
    public partial ObservableCollection<Datum> Datums { get; set; }

    [ObservableProperty]
    public partial Datum SelectDatum { get; set; }

    [ObservableProperty]
    public partial string GameName { get; set; }

    [ObservableProperty]
    public partial string Phone { get; set; }

    private async void _framePool_FrameArrived(Direct3D11CaptureFramePool sender, object args)
    {
        using (var frame = _framePool.TryGetNextFrame())
        {
            try
            {
                if (frame == null)
                    return;
                CanvasBitmap canvasBitmap = CanvasBitmap.CreateFromDirect3D11Surface(
                    _canvasDevice,
                    frame.Surface);
                await FillSurfaceWithBitmap(canvasBitmap);
                Logger.WriteInfo("开始读取屏幕帧");
            }

            catch (Exception e) when (_canvasDevice.IsDeviceLost(e.HResult))
            {
            }
        }
    }


    private async Task FillSurfaceWithBitmap(CanvasBitmap canvasBitmap)
    {
        try
        {
            
            var luminanceSource = new CanvasBitmapLuminanceSource(canvasBitmap);

            var binaryBitmap = new BinaryBitmap(new HybridBinarizer(luminanceSource));
            var reader = new MultiFormatReader();
            var hints = new DecodingOptions
            {
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
                TryHarder = true
            };
            var result2 = reader.decode(binaryBitmap);
            if (result2 == null)
                return;
            if(result2.Text != null)
            {
                this.QRResult = result2.Text;
                var result =  await WavesClient.PostQrValueAsync(QRResult, CTS.Token);
                if(result ==null) return;
                if(result.Code == 200 && result.Success == true)
                {
                    TipMessage = "获取登陆信息成功";
                    this.ScreenMessage = "重新选择显示器扫码";
                    _framePool?.FrameArrived -= _framePool_FrameArrived;
                    _session?.Dispose();
                    _framePool?.Dispose();
                    _framePool = null;
                    _session = null;
                    this.Datums = result.Data.ToObservableCollection();
                    this.SelectDatum = Datums[0];
                    this.LoginBthVisibility = Visibility.Visible;
                    SessionVisibility = Visibility.Visible;
                }
                else
                {
                    _framePool?.FrameArrived -= _framePool_FrameArrived;
                    _session?.Dispose();
                    _framePool?.Dispose();
                    _framePool = null;
                    _session = null;
                    TipMessage = result.Msg;
                }
            }
            else
            {

                Logger.WriteInfo("屏幕帧读取二维码失败，正在截取下一帧");
            }

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"解码失败: {ex.Message}");
        }
    }

    public QRScanResult? Result { get; set; }

    [ObservableProperty]
    public partial string TipMessage { get; set; }

    [ObservableProperty]
    public partial string QRResult { get; set; } = "选择游戏窗口（需要露出游戏二维码）";

    [ObservableProperty]
    public partial string VerifyCode { get; set; } = "";

    [ObservableProperty]
    public partial ObservableCollection<GameRoilDataWrapper> Roles { get; private set; }

    [RelayCommand]
    async Task Invoke()
    {
        if (!GraphicsCaptureSession.IsSupported())
        {
            return;
        }
        var picker = new GraphicsCapturePicker();
        InitializeWithWindow.Initialize(picker, this.AppContext.App.MainWindow.GetWindowHandle());
        GraphicsCaptureItem item = await picker.PickSingleItemAsync();
        if (item != null)
        {
            if (_framePool != null)
            {
                _framePool?.FrameArrived -= _framePool_FrameArrived;
                _session?.Dispose();
                _framePool?.Dispose();
            }
            _canvasDevice = new CanvasDevice();
            _framePool = Direct3D11CaptureFramePool.Create(_canvasDevice, Windows.Graphics.DirectX.DirectXPixelFormat.B8G8R8A8UIntNormalized, 1, item.Size);
            _framePool.FrameArrived += _framePool_FrameArrived;
            _session = _framePool.CreateCaptureSession(item);
            _session.StartCapture();
        }
    }

    public override void AfterClose()
    {
        if (_framePool != null)
        {
            _framePool?.FrameArrived -= _framePool_FrameArrived;
            
            _session?.Dispose();
            _framePool?.Dispose();
        }
        base.AfterClose();
    }

    [RelayCommand]
    async Task LoginAsync()
    {
        var result = await  WavesClient.QRLoginAsync(QRResult, VerifyCode,this.SelectDatum.Id, CTS.Token);
        if (result == null)
        {
            TipMessage = "登陆失败，请及时联系开发者";
            return;
        }
        if(result.Code == 2240)
        {
            TipMessage = "该设备不安全，安全验证已经发送至手机";
            var result2 = await WavesClient.GetQrCodeAsync(QRResult);
            VerifyCodeVisibility = Visibility.Visible;
            return;
        }
        else if(result.Code == 200)
        {
            TipMessage = "登陆成功";

            Logger.WriteInfo($"扫码登陆成功，结果{result.Data}");
        }
        this.Result = new QRScanResult(true);
        await Task.Delay(1000);
        this.Close();
    }
}

