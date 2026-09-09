using CommunityToolkit.WinUI.Helpers;
using Haiyu.Common.Contracts;
using Waves.Settings.Models;

namespace Haiyu.ViewModel.ToolkitsViewModel;

/// <summary>监控悬浮窗设置变化消息。</summary>
public sealed record MonitorSettingsChangedMessage(
    MoniterTheme Theme,
    FPSUIConfig FPS,
    CPUUIConfig CPU,
    GPUUIConfig GPU,
    MoniterUIItem Power,
    MoniterUIItem Memory
);

public sealed partial class MonitorSettingViewModel : ViewModelBase
{
    private readonly MoniterSettings _settings;
    private CancellationTokenSource? _saveCancellation;
    private bool _isLoaded;

    public MonitorSettingViewModel(WindowSession session)
    {
        Session = session;
        _settings = new MoniterSettings(
            Path.Combine(AppSettings.BassFolder, "MonitorSettings.json")
        );
    }

    public WindowSession Session { get; }
    public ObservableCollection<string> FontFamilies { get; } =
    ["微软雅黑", "Microsoft YaHei UI", "Segoe UI", "等线", "宋体"];

    public SolidColorBrush FontColorBrush => new(FontColor);

    public string FontColorText =>
        $"#{FontColor.A:X2}{FontColor.R:X2}{FontColor.G:X2}{FontColor.B:X2}";

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial double FontSize { get; set; } = 14;

    [ObservableProperty]
    public partial Color FontColor { get; set; } = Colors.White;

    [ObservableProperty]
    public partial string FontFamily { get; set; } = "微软雅黑";

    [ObservableProperty]
    public partial double MonitorItemSpacing { get; set; } = 5;

    [ObservableProperty]
    public partial bool IsAllowDesktop { get; set; } = true;

    [ObservableProperty]
    public partial bool CurrentFpsEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool FPS1LowEnable { get; set; }

    [ObservableProperty]
    public partial bool FPS01LowEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool FPSMaxEnable { get; set; }

    [ObservableProperty]
    public partial bool MaxFrameTimeEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool CPULoadEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool CPUClockEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool CPUTempateEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool CPUVoltagesEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool GPULoadEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool GPUClockEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool GPUTempateEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool GPUVoltagesEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool GPUVMemoryEnable { get; set; } = true;

    [ObservableProperty]
    public partial bool PowerEnable { get; set; }

    [ObservableProperty]
    public partial bool MemoryEnable { get; set; }

    [RelayCommand]
    private async Task LoadedAsync()
    {
        if (_isLoaded || IsLoading)
            return;

        IsLoading = true;
        try
        {
            var theme = await _settings.GetMoniterThemeAsync(LifetimeToken) ?? new MoniterTheme();
            var fps =
                await _settings.GetMoniterFPSAsync(LifetimeToken)
                ?? new FPSUIConfig
                {
                    CurrentFpsEnable = true,
                    FPS01LowEnable = true,
                    MaxFrameTimeEnable = true,
                };
            var cpu =
                await _settings.GetMoniterCPUAsync(LifetimeToken)
                ?? new CPUUIConfig
                {
                    LoadEnable = true,
                    ClockEnable = true,
                    TempateEnable = true,
                    VoltagesEnable = true,
                };
            var gpu =
                await _settings.GetMoniterGPUAsync(LifetimeToken)
                ?? new GPUUIConfig
                {
                    LoadEnable = true,
                    ClockEnable = true,
                    TempateEnable = true,
                    VoltagesEnable = true,
                    VMemoryEnable = true,
                };
            var power = await _settings.GetMoniterPowerAsync(LifetimeToken) ?? new MoniterUIItem();
            var memory =
                await _settings.GetMoniterMemoryAsync(LifetimeToken) ?? new MoniterUIItem();

            Apply(theme, fps, cpu, gpu, power, memory);
            _isLoaded = true;
            PublishSettings();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void Apply(
        MoniterTheme theme,
        FPSUIConfig fps,
        CPUUIConfig cpu,
        GPUUIConfig gpu,
        MoniterUIItem power,
        MoniterUIItem memory
    )
    {
        FontSize = theme.FontSize;
        FontColor = ParseColor(theme.FontColor);
        FontFamily = theme.FontFamily;
        MonitorItemSpacing = theme.MonitorItemSpacing;
        IsAllowDesktop = theme.IsAllowDesktop;
        CurrentFpsEnable = fps.CurrentFpsEnable;
        FPS1LowEnable = fps.FPS1LowEnable;
        FPS01LowEnable = fps.FPS01LowEnable;
        FPSMaxEnable = fps.FPSMaxEnable;
        MaxFrameTimeEnable = fps.MaxFrameTimeEnable;
        CPULoadEnable = cpu.LoadEnable;
        CPUClockEnable = cpu.ClockEnable;
        CPUTempateEnable = cpu.TempateEnable;
        CPUVoltagesEnable = cpu.VoltagesEnable;
        GPULoadEnable = gpu.LoadEnable;
        GPUClockEnable = gpu.ClockEnable;
        GPUTempateEnable = gpu.TempateEnable;
        GPUVoltagesEnable = gpu.VoltagesEnable;
        GPUVMemoryEnable = gpu.VMemoryEnable;
        PowerEnable = power.Enable;
        MemoryEnable = memory.Enable;
    }

    private MonitorSettingsChangedMessage CreateMessage() =>
        new(
            new MoniterTheme
            {
                FontSize = Math.Clamp(FontSize, 10, 32),
                FontColor = $"#{FontColor.A:X2}{FontColor.R:X2}{FontColor.G:X2}{FontColor.B:X2}",
                FontFamily = string.IsNullOrWhiteSpace(FontFamily) ? "微软雅黑" : FontFamily,
                MonitorItemSpacing = Math.Clamp(MonitorItemSpacing, 0, 30),
                IsAllowDesktop = IsAllowDesktop,
            },
            new FPSUIConfig
            {
                CurrentFpsEnable = CurrentFpsEnable,
                FPS1LowEnable = FPS1LowEnable,
                FPS01LowEnable = FPS01LowEnable,
                FPSMaxEnable = FPSMaxEnable,
                MaxFrameTimeEnable = MaxFrameTimeEnable,
            },
            new CPUUIConfig
            {
                LoadEnable = CPULoadEnable,
                ClockEnable = CPUClockEnable,
                TempateEnable = CPUTempateEnable,
                VoltagesEnable = CPUVoltagesEnable,
            },
            new GPUUIConfig
            {
                LoadEnable = GPULoadEnable,
                ClockEnable = GPUClockEnable,
                TempateEnable = GPUTempateEnable,
                VoltagesEnable = GPUVoltagesEnable,
                VMemoryEnable = GPUVMemoryEnable,
            },
            new MoniterUIItem { Enable = PowerEnable },
            new MoniterUIItem { Enable = MemoryEnable }
        );

    private void SettingsChanged()
    {
        if (!_isLoaded || !IsAlive)
            return;

        var message = CreateMessage();
        WeakReferenceMessenger.Default.Send(message);
        _saveCancellation?.Cancel();
        _saveCancellation?.Dispose();
        _saveCancellation = CancellationTokenSource.CreateLinkedTokenSource(LifetimeToken);
        _ = SaveAfterDelayAsync(message, _saveCancellation.Token);
    }

    private void PublishSettings() => WeakReferenceMessenger.Default.Send(CreateMessage());

    private async Task SaveAfterDelayAsync(
        MonitorSettingsChangedMessage message,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await Task.Delay(300, cancellationToken);
            await _settings.SetMoniterThemeAsync(message.Theme, cancellationToken);
            await _settings.SetMoniterFPSAsync(message.FPS, cancellationToken);
            await _settings.SetMoniterCPUAsync(message.CPU, cancellationToken);
            await _settings.SetMoniterGPUAsync(message.GPU, cancellationToken);
            await _settings.SetMoniterPowerAsync(message.Power, cancellationToken);
            await _settings.SetMoniterMemoryAsync(message.Memory, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { }
        catch (Exception exception)
        {
            Logger.WriteError($"保存监控设置失败：{exception.Message}{exception.StackTrace}");
        }
    }

    partial void OnFontSizeChanged(double value) => SettingsChanged();

    private static Color ParseColor(string? value)
    {
        try
        {
            return CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor(value);
        }
        catch (Exception)
        {
            return Colors.Red;
        }
    }

    partial void OnFontColorChanged(Color value)
    {
        OnPropertyChanged(nameof(FontColorBrush));
        OnPropertyChanged(nameof(FontColorText));
        SettingsChanged();
    }

    partial void OnFontFamilyChanged(string value) => SettingsChanged();

    partial void OnMonitorItemSpacingChanged(double value) => SettingsChanged();

    partial void OnIsAllowDesktopChanged(bool value) => SettingsChanged();

    partial void OnCurrentFpsEnableChanged(bool value) => SettingsChanged();

    partial void OnFPS1LowEnableChanged(bool value) => SettingsChanged();

    partial void OnFPS01LowEnableChanged(bool value) => SettingsChanged();

    partial void OnFPSMaxEnableChanged(bool value) => SettingsChanged();

    partial void OnMaxFrameTimeEnableChanged(bool value) => SettingsChanged();

    partial void OnCPULoadEnableChanged(bool value) => SettingsChanged();

    partial void OnCPUClockEnableChanged(bool value) => SettingsChanged();

    partial void OnCPUTempateEnableChanged(bool value) => SettingsChanged();

    partial void OnCPUVoltagesEnableChanged(bool value) => SettingsChanged();

    partial void OnGPULoadEnableChanged(bool value) => SettingsChanged();

    partial void OnGPUClockEnableChanged(bool value) => SettingsChanged();

    partial void OnGPUTempateEnableChanged(bool value) => SettingsChanged();

    partial void OnGPUVoltagesEnableChanged(bool value) => SettingsChanged();

    partial void OnGPUVMemoryEnableChanged(bool value) => SettingsChanged();

    partial void OnPowerEnableChanged(bool value) => SettingsChanged();

    partial void OnMemoryEnableChanged(bool value) => SettingsChanged();

    protected override void OnDisposing()
    {
        _saveCancellation?.Cancel();
        _saveCancellation?.Dispose();
        _saveCancellation = null;
        base.OnDisposing();
    }
}
