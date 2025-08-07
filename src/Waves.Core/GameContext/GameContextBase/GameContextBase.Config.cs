using Waves.Core.Models;

namespace Waves.Core.GameContext;

partial class GameContextBase
{
    public async Task<bool> SetLimitSpeedAsync(int value, CancellationToken token = default)
    {
        if (
            await this.GameLocalConfig.SaveConfigAsync(
                GameLocalSettingName.LimitSpeed,
                value.ToString()
            )
        )
        {
            this.SpeedValue = value;
            this.IsLimitSpeed = true;
            return true;
        }
        return false;
    }

    public async Task<bool> SetDx11LauncheAsync(bool value, CancellationToken token = default)
    {
        if (
            await this.GameLocalConfig.SaveConfigAsync(
                GameLocalSettingName.IsDx11,
                value.ToString()
            )
        )
        {
            this.IsDx11Launche = value;
        }
        return false;
    }

    public async Task<GameContextConfig> ReadContextConfigAsync(CancellationToken token = default)
    {
        GameContextConfig config = new();
        var speed = GameLocalConfig.GetConfig(GameLocalSettingName.LimitSpeed);
        var dx11 = GameLocalConfig.GetConfig(GameLocalSettingName.IsDx11);
        if (int.TryParse(speed, out var rate))
        {
            config.LimitSpeed = rate;
        }
        else
            config.LimitSpeed = 0;
        if (string.IsNullOrWhiteSpace(dx11))
            config.IsDx11 = false;
        if (bool.TryParse(dx11, out var isDx11))
        {
            config.IsDx11 = isDx11;
        }
        else
            config.IsDx11 = false;
        return config;
    }
}
