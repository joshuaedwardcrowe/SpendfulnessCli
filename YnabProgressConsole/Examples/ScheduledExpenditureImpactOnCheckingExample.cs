// using Ynab.Clients;
// using Ynab.Extensions;
// using Ynab.Responses.Budgets;
// using YnabProgress.Compilers;
// using YnabProgressConsole.Compilers;
//
// namespace YnabProgressConsole.Examples;
//
// public static class ScheduledExpenditureImpactOnCheckingExample
// {
//     public static async Task Run()
//     {
//         var budgetsClient = new BudgetsClient();
//         
//         var budgets = await budgetsClient.GetBudgets();
//         var budget = budgets.First();
//         
//         var checkingBalance = await budget.GetCheckingBalance();
//         var criticalSpendingBalance = await GetCriticalSpendingBalance(budget);
//         var scheduledSpending = await GetScheduledSpending(budget);
//         
//         var projectedSpending = criticalSpendingBalance + scheduledSpending;
//         var remaining = checkingBalance - projectedSpending;
//         
//         var viewModel = new AmountCompiler().Compile(remaining);
//         var table = ConsoleTableCompiler.Compile(viewModel);
//         
//         Console.WriteLine(table);
//         
//         var requestLogViewModel = new RequestLogCompiler().Compile(budgetsClient.RequestRequestLogs);
//         var requestLogTable = ConsoleTableCompiler.Compile(requestLogViewModel);
//         
//         Console.WriteLine(requestLogTable);
//     }
//     
//     private static async Task<decimal> GetCriticalSpendingBalance(Budget budget)
//     {
//         var criticalCategoryGroupNames = new List<string>
//         {
//             "Phone",
//             "Career",
//             "Owning a Home",
//             "Maintaining a Home",
//             "Credit Card Payments"
//         };
//         
//         var categoryGroups = await budget.GetCategoryGroups();
//         
//         return categoryGroups
//             .FilterToCategories(criticalCategoryGroupNames.ToArray())
//             .Sum(categoryGroup => categoryGroup.Balance);
//     }
//
//     private static async Task<decimal> GetScheduledSpending(Budget budget)
//     {
//         var scheduledTransactions = await budget.GetScheduledTransactions();
//
//         var today = DateTime.Now;
//         var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
//         var lastDateOfMonth = new DateTime(today.Year, today.Month, daysInMonth);
//
//         return scheduledTransactions
//             .Where(o => o.NextOccurence > lastDateOfMonth)
//             .Sum(o => o.Amount);
//     }
// }