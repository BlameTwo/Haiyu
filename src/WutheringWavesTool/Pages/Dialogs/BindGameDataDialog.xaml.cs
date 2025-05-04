namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class BindGameDataDialog : ContentDialog, IDialog
{
    public BindGameDataDialog()
    {
        this.InitializeComponent();
        this.ViewModel = Instance.Service.GetRequiredService<BindGameDataViewModel>();
    }

    public BindGameDataViewModel ViewModel { get; private set; }

    public void SetData(object data)
    {
        if (data is string str)
        {
            ViewModel.InitCore(str);
        }
    }

    private void ContentDialog_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        this.ViewModel.Dispose();
        this.ViewModel = null;
        GC.Collect();
    }
}
