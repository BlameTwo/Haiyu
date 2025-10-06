namespace Haiyu.Services;

public class TipShow : ITipShow
{
    private Panel _owner;

    public Panel Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public void ShowMessage(string message, Symbol icon)
    {
        if (this.Owner == null)
            return;
        PopupMessage popup = new(message, Owner, icon);
        popup.ShowPopup();
    }
}
