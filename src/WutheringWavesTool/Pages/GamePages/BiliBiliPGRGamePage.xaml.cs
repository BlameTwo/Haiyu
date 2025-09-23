using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WutheringWavesTool.ViewModel.GameViewModels;

namespace WutheringWavesTool.Pages.GamePages
{
    public sealed partial class BiliBiliPGRGamePage : Page,IPage
    {
        public BiliBiliPGRGamePage()
        {
            this.InitializeComponent();
            this.ViewModel = Instance.Service.GetRequiredService<BiliBiliPGRGameViewModel>();
        }

        public Type PageType => typeof(BiliBiliPGRGamePage);

        public BiliBiliPGRGameViewModel ViewModel { get; }

        

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ViewModel.Dispose();
            this.Bindings.StopTracking();
        }
    }
}
