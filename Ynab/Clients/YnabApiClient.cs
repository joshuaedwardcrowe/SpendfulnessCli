using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ynab.Converters;
using Ynab.Exceptions;
using Ynab.Http;

namespace Ynab.Clients;

public abstract class YnabApiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        Converters = { 
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new IgnoreEmptyStringNullableEnumConverter()
        }
    };

    protected abstract HttpClient GetHttpClient();

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

    protected async Task<YnabHttpResponseContent<TApiResponse>> Post<TApiResponse>(string url, object payload) where TApiResponse : class
    {
        var client = GetHttpClient();

        var response = await client.PostAsJsonAsync(url, payload, _jsonOptions);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadFromJsonAsync<YnabHttpResponseContent<TApiResponse>>(_jsonOptions);

        if (responseContent is null)
        {
            throw new YnabException(YnabExceptionCode.ApiResponseIsEmpty, $"No response on {url}");
        }

        return responseContent;
    }
}