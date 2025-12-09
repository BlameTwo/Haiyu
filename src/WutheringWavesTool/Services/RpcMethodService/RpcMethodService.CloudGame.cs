using Waves.Api.Models.Rpc;

namespace Haiyu.Services;

public partial class RpcMethodService
{
    /// <summary>
    /// 获取单个单个账号的抽卡令牌
    /// </summary>
    /// <param name="key"></param>
    /// <param name="_param"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<string> GetReocrdTokenAsync(string key, List<RpcParams>? _param = null)
    {
        VerifyToken(_param);
        if(_param == null || _param.Count == 0)
        {
            throw new ArgumentException("param is null！");
        }
        var users = _param.Where(x => x.Key == "userName").ToArray();
        if(users.Count() != 1)
        {
            throw new ArgumentException("userName error!");
        }
        var user = users[0].Value;
        var UserData = await CloudConfigManager.GetUserAsync(user);
        if (UserData == null)
            throw new ArgumentException("local userName error!");
        var open = await CloudGameService.OpenUserAsync(UserData);
        if (open.Item1)
        {
            var record = await CloudGameService.GetRecordAsync();
            if(record != null)
            {
                return JsonSerializer.Serialize(record.Data, RpcContext.Default.RecordData);
            }
        }
        else
        {
            throw new ArgumentException(open.Item2);
        }

        throw new ArgumentException("local userName error!");
    }

    /// <summary>
    /// 获取全部账号名称
    /// </summary>
    /// <param name="key"></param>
    /// <param name="_param"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<string> GetCloudAccountsAsync(string key, List<RpcParams>? _param = null)
    {
        VerifyToken(_param);
        var users = await CloudConfigManager.GetUsersAsync();
        if(users == null || users.Count == 0)
        {
            throw new ArgumentException("user is null！");
        }
        return JsonSerializer.Serialize(users.Select(x=>x.Username).ToList(),RpcContext.Default.ListString);
    }
}
