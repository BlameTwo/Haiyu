﻿using Microsoft.UI.Xaml.Controls;
using WutheringWavesTool.Controls;
using WutheringWavesTool.Services.Contracts;

namespace WutheringWavesTool.Services;

public sealed class TipShow : ITipShow
{
    private Panel _owner;

    public Panel Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public void ShowMessage(string message, Symbol icon)
    {
        PopupMessage popup = new(message, Owner, icon);
        popup.ShowPopup();
    }
}
