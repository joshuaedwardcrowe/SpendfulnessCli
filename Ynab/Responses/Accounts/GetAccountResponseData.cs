using System.Text.Json.Serialization;

namespace Ynab.Responses.Accounts;

public class GetAccountResponseData
{
    [JsonPropertyName("account")]
    public required AccountResponse Account { get; set; }
}