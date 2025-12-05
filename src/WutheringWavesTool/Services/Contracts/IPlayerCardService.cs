namespace Haiyu.Services.Contracts;

public interface IPlayerCardService
{
    public Task<RecordCacheDetily> GetRecordAsync(string name);
    public Task<List<RecordCacheDetily>> GetRecordsAsync();

   
}