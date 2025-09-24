using System.Collections.Concurrent;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;

namespace Haiyu.Common
{
    internal sealed partial class InputActiveDesktopAcrylicBackdrop
        : Microsoft.UI.Xaml.Media.SystemBackdrop
    {
        private readonly ConcurrentDictionary<
            ICompositionSupportsSystemBackdrop,
            DesktopAcrylicController
        > controllers = [];

        protected override void OnTargetConnected(
            ICompositionSupportsSystemBackdrop target,
            XamlRoot xamlRoot
        )
        {
            base.OnTargetConnected(target, xamlRoot);

            SystemBackdropConfiguration configuration = GetDefaultSystemBackdropConfiguration(
                target,
                xamlRoot
            );
            configuration.IsInputActive = true;

            DesktopAcrylicController newController = new();
            newController.AddSystemBackdropTarget(target);
            newController.SetSystemBackdropConfiguration(configuration);
            newController.FallbackColor = new Color()
            {
                A = 10,
                R = 175,
                G = 183,
                B = 140,
            };
            controllers.TryAdd(target, newController);
        }

        protected override void OnTargetDisconnected(ICompositionSupportsSystemBackdrop target)
        {
            base.OnTargetDisconnected(target);

            if (controllers.TryRemove(target, out DesktopAcrylicController? controller))
            {
                controller.RemoveSystemBackdropTarget(target);
                controller.Dispose();
            }
        }
    }
}
