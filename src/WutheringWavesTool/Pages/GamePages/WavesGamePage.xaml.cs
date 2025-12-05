using Haiyu.ViewModel.GameViewModels;
using Haiyu.ViewModel.GameViewModels.Contracts;
using Haiyu.ViewModel.GameViewModels.GameContexts;
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

namespace Haiyu.Pages.GamePages
{
    public sealed partial class WavesGamePage : Page,IPage
    {
        public WavesGamePage()
        {
            InitializeComponent();
            ViewModel =  Instance.Service.GetRequiredService<WavesGameContextViewModel>();
            DataContext = this;
        }

        public Type PageType => typeof(WavesGamePage);

        public WavesGameContextViewModel ViewModel { get; set;  }

    }
}
