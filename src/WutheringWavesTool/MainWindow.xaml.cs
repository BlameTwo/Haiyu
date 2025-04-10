﻿using Microsoft.UI.Composition.SystemBackdrops;

namespace WutheringWavesTool
{
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        DesktopAcrylicController controller;

        public MainWindow()
        {
            this.InitializeComponent();
            this.SystemBackdrop = new DevWinUI.AcrylicSystemBackdrop(DesktopAcrylicKind.Base);
        }
    }
}
