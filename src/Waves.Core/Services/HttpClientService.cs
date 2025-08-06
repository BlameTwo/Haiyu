using Waves.Core.Contracts;
using Waves.Core.Models.Handlers;

namespace Waves.Core.Services;

public class HttpClientService : IHttpClientService
{
    public HttpClientService()
    {
    }

    public HttpClient HttpClient { get; private set; }

    public HttpClient GameDownloadClient { get; private set; }

    public void BuildClient()
    {
        this.HttpClient = new HttpClient(new WavesGameHandler());
        this.GameDownloadClient = new HttpClient(new WavesGameHandler());
    }
}
