
using Microsoft.Graphics.Canvas;
using Windows.Graphics;
using Windows.Graphics.Capture;
using WutheringWavesTool.Common.QR;
using WutheringWavesTool.Models.Dialogs;
using WutheringWavesTool.Services.DialogServices;
using ZXing;
using ZXing.Common;

namespace WutheringWavesTool.ViewModel.DialogViewModels;

public partial class QrLoginViewModel : DialogViewModelBase
{
    public QrLoginViewModel([FromKeyedServices(nameof(MainDialogService))] IDialogManager dialogManager,IAppContext<App> appContext,IWavesClient wavesClient) : base(dialogManager)
    {
        AppContext = appContext;
        WavesClient = wavesClient;
    }

    private CanvasDevice _canvasDevice;
    private Direct3D11CaptureFramePool _framePool;
    private SizeInt32 _lastSize;
    private CanvasBitmap _currentFrame;
    private GraphicsCaptureSession _session;

    public IAppContext<App> AppContext { get; }
    public IWavesClient WavesClient { get; }

    private void StartCaptureInternal(GraphicsCaptureItem item)
    {
        _canvasDevice = new CanvasDevice();
        _framePool = Direct3D11CaptureFramePool.Create(_canvasDevice, Windows.Graphics.DirectX.DirectXPixelFormat.B8G8R8A8UIntNormalized, 1, item.Size);

        _framePool.FrameArrived += _framePool_FrameArrived;
        _session = _framePool.CreateCaptureSession(item);
        _session.StartCapture();
    }

    private void _framePool_FrameArrived(Direct3D11CaptureFramePool sender, object args)
    {
        using (var frame = _framePool.TryGetNextFrame())
        {
            ProcessFrame(frame);
        }
    }

    private async void ProcessFrame(Direct3D11CaptureFrame frame)
    {
        try
        {
            bool needsReset = false;
            bool recreateDevice = false;
            if (frame == null)
                return;
            if ((frame.ContentSize.Width != _lastSize.Width) ||
                (frame.ContentSize.Height != _lastSize.Height))
            {
                needsReset = true;
                _lastSize = frame.ContentSize;
            }

            try
            {
                CanvasBitmap canvasBitmap = CanvasBitmap.CreateFromDirect3D11Surface(
                    _canvasDevice,
                    frame.Surface);

                _currentFrame = canvasBitmap;
                await FillSurfaceWithBitmap(canvasBitmap);
            }

            catch (Exception e) when (_canvasDevice.IsDeviceLost(e.HResult))
            {
                needsReset = true;
                recreateDevice = true;
            }
        }
        catch (Exception)
        {
            
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
                    Debug.WriteLine("获取登陆信息成功");
                    _framePool.FrameArrived -= _framePool_FrameArrived;
                    _session.Dispose();
                    _framePool.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            // 处理解码过程中可能出现的异常
            System.Diagnostics.Debug.WriteLine($"解码失败: {ex.Message}");
        }
    }

    public QRScanResult? Result { get; set; }

    [ObservableProperty]
    public partial string TipMessage { get; set; }

    [ObservableProperty]
    public partial string QRResult { get; set; } = "选择游戏窗口（需要露出二维码）";

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
            StartCaptureInternal(item);
        }
    }

    [RelayCommand]
    async Task LoginAsync()
    {
        var result =await  WavesClient.QRLoginAsync(QRResult, VerifyCode, CTS.Token);
        if (result == null)
            return;
        if(result.Code == 2240)
        {
            TipMessage = "该设备不安全，本次扫码需要手动发送验证码验证";
            var result2 = await WavesClient.GetQrCodeAsync(QRResult);
        }
        else if(result.Code == 200)
        {
            TipMessage = "登陆成功";
        }
    }


    [RelayCommand]
    void Loaded()
    {
    }
    
}

