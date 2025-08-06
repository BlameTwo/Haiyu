using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Waves.Api.Models.QRLogin;


public class DeviceDatum
{
    [JsonPropertyName("authDeviceNo")]
    public string AuthDeviceNo { get; set; }

    [JsonPropertyName("deviceName")]
    public string DeviceName { get; set; }

    [JsonPropertyName("deviceRoles")]
    public List<DeviceRole> DeviceRoles { get; set; }

}

public class DeviceRole
{
    [JsonPropertyName("autoLogin")]
    public bool AutoLogin { get; set; }

    [JsonPropertyName("hasAuth")]
    public bool HasAuth { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("mode")]
    public int Mode { get; set; }

    [JsonPropertyName("puid")]
    public string Puid { get; set; }
    [JsonIgnore]
    public IRelayCommand SendDeleteCommand => new RelayCommand(() =>
    {

    });
}

public class DeviceInfo
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("data")]
    public List<DeviceDatum> Data { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
