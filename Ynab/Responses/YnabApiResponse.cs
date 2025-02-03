using System.Text.Json.Serialization;

namespace Ynab.Responses;

public class YnabApiResponse<TResponseData> where TResponseData : class
{
    [JsonPropertyName("data")]
    public required TResponseData Data { get; set; }
}