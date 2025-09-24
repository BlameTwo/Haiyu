namespace Haiyu.Services.Contracts;

public interface IRecordCacheService
{
    Task<bool> CreateRecordAsync(RecordCacheDetily? recordCacheDetily);
    Task<IEnumerable<RecordCacheDetily?>> GetRecordCacheDetilyAsync();

    Task<IEnumerable<(RecordCacheDetily?, string?)>> GetRecordCacheDetilyAndPathAsync();
}
