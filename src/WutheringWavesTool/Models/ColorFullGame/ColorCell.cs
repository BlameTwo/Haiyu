using System.Text.Json.Serialization;

namespace Haiyu.Models.ColorFullGame;

public partial class ColorCell : ObservableObject
{
    [JsonPropertyName("row")]
    [ObservableProperty]
    public partial int Row { get; set; }


    [JsonPropertyName("column")]
    [ObservableProperty]
    public partial int Column { get; set; }

    [JsonPropertyName("r")]
    public int R { get; set; }

    [JsonPropertyName("g")]
    public int G { get; set; }

    [JsonPropertyName("b")]
    public int B { get; set; }

    [JsonIgnore]
    [ObservableProperty]
    public partial SolidColorBrush CurrentColor { get; set; }

    partial void OnCurrentColorChanged(SolidColorBrush value)
    {
        this.R = value.Color.R;
        this.G = value.Color.G;
        this.B = value.Color.B;
    }

    [JsonPropertyName("isStone")]
    [ObservableProperty]
    public partial bool IsStone { get; set; }

    /// <summary>
    /// 获得保存的颜色，用来重置刷新
    /// </summary>
    /// <returns></returns>
    public SolidColorBrush GetSaveColor()
    {
        return new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));
    }

    public void SetSaveColor()
    {
        this.R = this.CurrentColor.Color.R;
        this.G = this.CurrentColor.Color.G;
        this.B = this.CurrentColor.Color.B;
    }
}
