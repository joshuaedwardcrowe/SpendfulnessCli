using System.Net.Http.Json;
using Ynab.Responses;

namespace Ynab.Clients;

public class YnabApiClient
{
    protected virtual HttpClient GetHttpClient()
    {
        throw new Exception("No override");
    }

    protected async Task<YnabApiResponse<TApiResponse>> Get<TApiResponse>(string url) where TApiResponse : class
    {
        var client = GetHttpClient();
        
        var response = await client.GetAsync(url);
        
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadFromJsonAsync<YnabApiResponse<TApiResponse>>();
        
        return responseContent;
    }
}