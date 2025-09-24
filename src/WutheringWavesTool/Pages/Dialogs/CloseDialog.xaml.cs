using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Haiyu.Models.Dialogs;
using Haiyu.Services.DialogServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Haiyu.Pages.Dialogs
{
    public sealed partial class CloseDialog : ContentDialog,
            IResultDialog<CloseWindowResult>
    {
        public CloseDialog()
        {
            this.InitializeComponent();
        }

        private bool isExit = false, isMin = false;
        public CloseWindowResult GetResult()
        {
            return new CloseWindowResult() { IsExit = this.isExit, IsMinTaskBar = this.isMin };
        }

        private void Min_Win(object sender, RoutedEventArgs e)
        {
            if (isClose.IsChecked == true)
            {
                AppSettings.CloseWindow = "False";
            }
            this.isExit = false;
            this.isMin = true;
            Instance.Service.GetRequiredKeyedService<IDialogManager>(nameof(MainDialogService)).CloseDialog();
        }

        private void Close_Win(object sender, RoutedEventArgs e)
        {
            if (isClose.IsChecked == true)
            {
                AppSettings.CloseWindow = "True";
            }
            this.isExit = true;
            this.isMin = false;
            Instance.Service.GetRequiredKeyedService<IDialogManager>(nameof(MainDialogService)).CloseDialog();
        }

        public void SetData(object data)
        {
        }
    }
}
