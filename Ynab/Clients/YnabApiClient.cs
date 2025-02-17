using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ynab.Http;

namespace Ynab.Clients;

public class YnabApiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    protected virtual HttpClient GetHttpClient()
    {
        throw new Exception("No override");
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Get<TApiResponse>(string url) where TApiResponse : class
    {
        var client = GetHttpClient();
        
        var response = await client.GetAsync(url);
        
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadFromJsonAsync<YnabHttpResponseContent<TApiResponse>>(_jsonOptions);
        if (responseContent is null)
        {
            throw new NullReferenceException("Response is null");
        }
        
        return responseContent;
    }
}