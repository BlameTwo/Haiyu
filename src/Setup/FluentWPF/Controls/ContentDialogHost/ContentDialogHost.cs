using FluentWPF.Contracts;
using System.Threading;
using System.Windows.Controls;

namespace FluentWPF.Controls;

/// <summary>
/// 对话框容器
/// 此容器只是对于内容的一个包裹，在<see cref="ContentDialogHost"/>的 <see cref="Close"/>方法中可关闭外层包裹并尝试释放包裹内容
/// 使用方法：
///
/// </summary>
public partial class ContentDialogHost : ContentControl, IContentDialogHost
{
    public ContentDialogHost()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ContentDialogHost),
            new FrameworkPropertyMetadata(typeof(ContentDialogHost))
        );
    }

    public void Close() { }

    public void Show() { }
}
