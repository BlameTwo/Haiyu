namespace WutheringWavesTool.Pages.Dialogs;

public sealed partial class InputRecordCardDialog : ContentDialog, IDialog
{
    public InputRecordCardDialog()
    {
        this.InitializeComponent();
        this.RequestedTheme =
            AppSettings.AppTheme == null ? ElementTheme.Default
            : AppSettings.AppTheme == "Dark" ? ElementTheme.Dark
            : AppSettings.AppTheme == "Light" ? ElementTheme.Light
            : ElementTheme.Default;
    }

    public InputRecordCardViewModel ViewModel { get; internal set; }

    public void SetData(object data) { }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        this.ViewModel.Args = new(CreateRecordType.SelectItemOpen)
        {
            Link = null,
            Cache = e.ClickedItem as RecordCacheDetily,
        };
        this.ViewModel.DialogManager.CloseDialog();
    }
}
