using System.Collections.Generic;
using Waves.Core.Models.Downloader;

namespace Haiyu.Plugin.Models;

/// <summary>
/// 服务器分析结果
/// </summary>
public class ServerAnalyseModel
{
    public List<IndexResource> RewriterFiles { get; }
    public List<IndexResource> DeleteFiles { get; }
    public List<IndexResource> UnchangedFiles { get; }
    public List<IndexResource> AddFiles { get; }

    public double ScoreValue { get; }
    public bool IsSwitch { get; }

    public ServerAnalyseModel(
        List<IndexResource> addFiles,
        List<IndexResource> rewriterFiles,
        List<IndexResource> deleteFiles,
        List<IndexResource> unchangedFiles,
       bool isSwitch
,
       double scoreValue)
    {
        AddFiles = addFiles;
        RewriterFiles = rewriterFiles;
        DeleteFiles = deleteFiles;
        UnchangedFiles = unchangedFiles;
        IsSwitch = isSwitch;
        ScoreValue = scoreValue;
    }
}
