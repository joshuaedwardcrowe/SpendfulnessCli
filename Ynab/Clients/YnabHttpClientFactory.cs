using System.Net.Http.Headers;

namespace Ynab.Clients;

public class YnabHttpClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string BaseUrl = "https://api.ynab.com/v1";

    public YnabHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public HttpClient Create(string? parentPath = null, string? nextPath = null)
    {
        var httpClient = _httpClientFactory.CreateClient();

        httpClient.BaseAddress = new Uri($"{BaseUrl}/{parentPath}/{nextPath}");

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "_yS6MFPwm5FAEpVRv-hrhp-KrluIm_q-hqTXmw6UFWU");
        
        return httpClient;
    }
}