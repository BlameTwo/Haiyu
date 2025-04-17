﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WutheringWavesTool.Models
{
    public class PletteArgs
    {
        public PletteArgs(Color? background, Color? forground, Color? shadow)
        {
            Background = background;
            Forground = forground;
            Shadow = shadow;
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color? Background { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        public Color? Forground { get; set; }

        /// <summary>
        /// 阴影
        /// </summary>
        public Color? Shadow { get; set; }
    }
}
