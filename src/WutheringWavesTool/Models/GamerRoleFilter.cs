namespace Haiyu.Models;

public sealed partial class GamerRoleFilter : ObservableObject
{
    [ObservableProperty]
    public partial string DisplayName { get; set; }

    [ObservableProperty]
    public partial int Id { get; set; }
}
