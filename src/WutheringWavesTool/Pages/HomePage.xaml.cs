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

namespace Haiyu.Pages
{
    public sealed partial class HomePage : Page,IPage
    {
        public HomePage()
        {
            InitializeComponent();
            this.ViewModel = Instance.GetService<HomeViewModel>();

        }

        public Type PageType => typeof(HomePage);

        public HomeViewModel ViewModel { get; }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.ViewModel.NavigationService.UnRegisterView();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ViewModel.NavigationService.RegisterView(this.frame);
        }


    }
}
