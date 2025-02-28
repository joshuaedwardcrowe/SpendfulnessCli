using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ynab.Converters;
using Ynab.Exceptions;
using Ynab.Http;
using NotImplementedException = System.NotImplementedException;

namespace Ynab.Clients;

public class YnabApiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        Converters = { 
            new JsonStringEnumConverter(),
            new IgnoreEmptyStringNullableEnumConverter()
        }
    };
    
    protected virtual HttpClient GetHttpClient()
    {
        // This is genuine exceptional circumstances.
        throw new NotImplementedException();
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Get<TApiResponse>(string url) where TApiResponse : class
    {
        var client = GetHttpClient();
        
        var response = await client.GetAsync(url);
        
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadFromJsonAsync<YnabHttpResponseContent<TApiResponse>>(_jsonOptions);
        if (responseContent is null)
        {
            throw new YnabException(YnabExceptionCode.ApiResponseIsEmpty, $"No response on {url}");
        }
        
        return responseContent;
    }
}