using System.Text.Json.Serialization;

namespace Ynab.Responses.Budgets;

public class BudgetResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}