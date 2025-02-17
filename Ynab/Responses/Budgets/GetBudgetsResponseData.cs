using System.Text.Json.Serialization;

namespace Ynab.Responses.Budgets;

public class GetBudgetsResponseData
{
    [JsonPropertyName("budgets")]
    public required IEnumerable<BudgetResponse> Budgets { get; set; }
}