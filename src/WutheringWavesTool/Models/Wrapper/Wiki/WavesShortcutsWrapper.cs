using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Haiyu.Models.Wrapper.Wiki;

public partial class WavesShortcutsWrapper : ObservableObject
{
    [ObservableProperty]
    public partial string Title { get; set; }

    [ObservableProperty]
    public partial string ContentUrl { get; set;  }

    [ObservableProperty]
    public partial string CatalogueId { get; set; }

    [ObservableProperty]
    public partial string LinkType { get; set; }

    public IAsyncRelayCommand OpenWeb => new AsyncRelayCommand(async () =>
    {
        await Launcher.LaunchUriAsync(new($"https://wiki.kurobbs.com/mc/catalogue/list?fid={CatalogueId}"));
    });
}
