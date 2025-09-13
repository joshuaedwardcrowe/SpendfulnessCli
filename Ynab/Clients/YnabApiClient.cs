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

    protected async Task<YnabHttpResponseContent<TApiResponse>> Get<TApiResponse>(string url)
        where TApiResponse : class
    {
        var client = GetHttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await ReadResponseContent<TApiResponse>(response, url);
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Post<TApiRequest, TApiResponse>(string url, TApiRequest payload)
        where TApiRequest : class where TApiResponse : class
    {
        var client = GetHttpClient();
        var response = await client.PostAsJsonAsync(url, payload, _jsonOptions);
        response.EnsureSuccessStatusCode();
        return await ReadResponseContent<TApiResponse>(response, url);
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Patch<TApiRequest, TApiResponse>(string url, TApiRequest payload) 
        where TApiRequest : class where TApiResponse : class
    {
        var client = GetHttpClient();
        var response = await client.PatchAsJsonAsync(url, payload, _jsonOptions);
        response.EnsureSuccessStatusCode();
        return await ReadResponseContent<TApiResponse>(response, url);
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Put<TApiRequest, TApiResponse>(string url, TApiRequest payload)
        where TApiRequest : class where TApiResponse : class
    {
        var client = GetHttpClient();        
        var response = await client.PutAsJsonAsync(url, payload, _jsonOptions);
        response.EnsureSuccessStatusCode();
        return await ReadResponseContent<TApiResponse>(response, url);
    }

    private async Task<YnabHttpResponseContent<TApiResponse>> ReadResponseContent<TApiResponse>(HttpResponseMessage response, string url)
        where TApiResponse : class
    {
        var responseContent = await response.Content.ReadFromJsonAsync<YnabHttpResponseContent<TApiResponse>>(_jsonOptions);
        if (responseContent is null)
        {
            throw new YnabException(YnabExceptionCode.ApiResponseIsEmpty, $"No response on {url}");
        }
        return responseContent;
    }
}