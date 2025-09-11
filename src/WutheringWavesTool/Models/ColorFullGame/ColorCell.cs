using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WutheringWavesTool.Models.ColorFullGame
{
    public partial class ColorCell:ObservableObject
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

        /// <summary>
        /// 获得保存的颜色，用来重置刷新
        /// </summary>
        /// <returns></returns>
        public SolidColorBrush GetSaveColor()
        {
            return new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));
        }
    }
}
