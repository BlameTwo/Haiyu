
using Microsoft.Extensions.DependencyInjection;

namespace Haiyu.Pages
{
    
    public sealed partial class ToolPage : Page,IPage
    {
        public ToolPage()
        {
            InitializeComponent();
            this.ViewModel = Instance.Host.Services.GetRequiredService<ToolViewModel>();
        }

        public Type PageType => typeof(ToolPage);

        public ToolViewModel ViewModel { get; }

    }
}
