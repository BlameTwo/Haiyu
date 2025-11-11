using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waves.Core.Models.Downloader;

namespace GameFileDiffTool;


public class FileDifference
{
    public string Dest { get; set; }
    public string OldMd5 { get; set; }
    public string NewMd5 { get; set; }
    public string ChangeType { get; set; } // "Added", "Deleted", "Changed"
    public long Size { get; internal set; }
}
