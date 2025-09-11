﻿using Microsoft.Windows.Storage.Pickers;

namespace WutheringWavesTool.Services.Contracts;

public interface IPickersService
{
    public Task<PickFolderResult> GetFolderPicker();

    public Task<PickFileResult> GetFileOpenPicker(List<string> extention);

    public Task<PickFileResult> GetFileSavePicker();
}
