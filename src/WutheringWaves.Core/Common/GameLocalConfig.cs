﻿using SqlSugar;

namespace WutheringWaves.Core.Common;

public class GameLocalSettingName
{
    /// <summary>
    /// 游戏启动文件夹
    /// </summary>
    public const string GameLauncherBassFolder = nameof(GameLauncherBassFolder);

    /// <summary>
    /// 游戏启动可执行文件
    /// </summary>
    public const string GameLauncherBassProgram = nameof(GameLauncherBassProgram);

    /// <summary>
    /// 本地游戏资源版本
    /// </summary>
    public const string LocalGameResourceVersion = nameof(LocalGameResourceVersion);

    /// <summary>
    /// 本地游戏版本
    /// </summary>
    public const string LocalGameVersion = nameof(LocalGameVersion);

    /// <summary>
    /// 下载速度
    /// </summary>
    public const string LimitSpeed = nameof(LimitSpeed);

    /// <summary>
    /// 是否使用DX11启动
    /// </summary>
    public const string IsDx11 = nameof(IsDx11);

    /// <summary>
    /// 预下载路径
    /// </summary>
    public const string ProdDownloadFolderPath = nameof(ProdDownloadFolderPath);
    public const string ProdDownloadFolderDone = nameof(ProdDownloadFolderDone);

    public const string ProdDownloadVersion = nameof(ProdDownloadVersion);

    public const string GameTime = nameof(GameTime);
}

public class GameLocalConfig
{
    public string SettingPath { get; set; }

    public async Task<bool> SaveConfigAsync(string key, string value)
    {
        try
        {
            string connectionString = $"Data Source={SettingPath};";
            using (
                ISqlSugarClient context = new SqlSugarClient(
                    new ConnectionConfig()
                    {
                        ConnectionString = connectionString,
                        DbType = DbType.Sqlite,
                        IsAutoCloseConnection = true,
                    }
                )
            )
            {
                context.CodeFirst.InitTables<LocalSettings>();
                var settings = new LocalSettings() { Key = key, Value = value };
                var existingSetting = await context
                    .Queryable<LocalSettings>()
                    .Where(x => x.Key == key)
                    .AnyAsync();
                if (existingSetting)
                {
                    await context.Updateable(settings).ExecuteCommandAsync();
                }
                else
                {
                    await context.Insertable(settings).ExecuteCommandAsync();
                }
                context.Ado.CommitTran();
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string> GetConfigAsync(string key)
    {
        string connectionString = $"Data Source={SettingPath};";
        using (
            SqlSugarClient context = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = DbType.Sqlite,
                    IsAutoCloseConnection = true,
                }
            )
        )
        {
            context.CodeFirst.InitTables<LocalSettings>();
            var existingSetting = await context
                .Queryable<LocalSettings>()
                .Where(x => x.Key == key)
                .FirstAsync();
            if (existingSetting == null)
            {
                return null;
            }
            else
            {
                return existingSetting.Value;
            }
        }
    }
}

[SugarTable("settings")]
public class LocalSettings
{
    [SugarColumn(ColumnName = "Key", IsPrimaryKey = true)]
    public string Key { get; set; }

    [SugarColumn(ColumnName = "Value")]
    public string Value { get; set; }
}

[SugarTable("playerTimes")]
public class PlayerTime
{
    [SugarColumn(ColumnName = "DateTime", IsPrimaryKey = true)]
    public DateTime Now { get; set; }

    [SugarColumn(ColumnName = "Tick")]
    public long Tick { get; set; }
}
