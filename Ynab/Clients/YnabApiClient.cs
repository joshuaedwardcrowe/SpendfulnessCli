using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ynab.Responses;

namespace Ynab.Clients;

public class YnabApiClient
{
    public int RateLimitRemaining = 200;
    private const string RateLimitHeaderName = "X-Rate-Limit";
    public List<ApiRequestLog> RequestLogs { get; set; }
    
    protected HttpClient HttpClient { get; } = new()
    {
        BaseAddress = new Uri("https://api.ynab.com/v1/"),
        DefaultRequestHeaders =
        {
            // TODO: Take this out of here.
            Authorization = new AuthenticationHeaderValue("Bearer", "_yS6MFPwm5FAEpVRv-hrhp-KrluIm_q-hqTXmw6UFWU")
        }
    };

    protected YnabApiClient(List<ApiRequestLog> requestLog)
    {
        RequestLogs = requestLog;
    }

    protected async Task<YnabApiResponse<TResponse>> Get<TResponse>(string specificPath) where TResponse : class
    {
        var requestTimer = new Stopwatch();
        requestTimer.Start();
        var response = await HttpClient.GetAsync(specificPath);
        requestTimer.Stop();
        
        var method = ParseHttpMethod(response.RequestMessage);
        var path = ParseRequestPath(response.RequestMessage);
        var rra = ParseRateLimitAllowance(response.Headers);
        LogRequest(method, path, rra, requestTimer.ElapsedMilliseconds);
        
        var parsedContent = await response.Content.ReadFromJsonAsync<YnabApiResponse<TResponse>>();
        if (parsedContent is null)
            throw new Exception("Request failed");

        return parsedContent;
    }

    private void LogRequest(
        string method,
        string path, 
        int remainingRequestAllowance,
        long requestTime)
    {
        var requestLog = new ApiRequestLog
        {
            Method = method,
            Path = path,
            RemainingRequestAllowance = remainingRequestAllowance,
            ResponeTime = requestTime,
        };
        
        RequestLogs.Add(requestLog);
    }

    private string ParseHttpMethod(HttpRequestMessage? request)
        => request?.Method.Method ?? string.Empty;
    
    private string ParseRequestPath(HttpRequestMessage? request)
        => request?.RequestUri?.AbsolutePath ?? string.Empty;

    private int ParseRateLimitAllowance(HttpResponseHeaders headers)
    {
        if (!headers.TryGetValues(RateLimitHeaderName, out var rateLimitHeaderValues))
        {
            return 200;
        }
        
        var rateLimitNote = rateLimitHeaderValues.FirstOrDefault();

        if (string.IsNullOrEmpty(rateLimitNote))
            return 200; // TODO: Move this to a config value somewhere.
        
        var split = rateLimitNote.Split('/');
        var usage = int.Parse(split[0]);
        var allowance = int.Parse(split[1]);
        
        return allowance - usage;
    }
}