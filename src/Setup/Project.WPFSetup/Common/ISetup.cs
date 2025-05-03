namespace Project.WPFSetup.Common;

public interface ISetup
{
    public Task<(string, bool)> ExecuteAsync(
        SetupProperty property,
        IProgress<(double, string)> progress,
        int maxValue
    );

    public string SetupName { get; }

    public int MaxProgress { get; }
}
