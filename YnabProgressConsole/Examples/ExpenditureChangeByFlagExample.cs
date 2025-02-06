// using Ynab.Clients;
// using Ynab.Extensions;
// using YnabProgress.Compilers;
// using YnabProgressConsole.Compilers;
//
// namespace YnabProgressConsole.Examples;
//
// public static class ExpenditureChangeByFlagExample
// {
//     public static async Task Run()
//     {
//         var budgetsClient = new BudgetsClient();
//         
//         var budgets = await budgetsClient.GetBudgets();
//         var budget = budgets.First();
//         
//         var categoryGroups = await budget.GetCategoryGroups();
//         var transactions = await budget.GetTransactions();
//         
//         Console.WriteLine($"FRate Limit Remaining: {budgetsClient.RateLimitRemaining}");
//         var requestLogViewModel = new RequestLogCompiler().Compile(budgetsClient.RequestRequestLogs);
//         var requestLogTable = ConsoleTableCompiler.Compile(requestLogViewModel);
//         Console.WriteLine(requestLogTable);
//         
//         var spendingCategoryIds = categoryGroups
//             .FilterToSpendingCategories()
//             .SelectMany(categoryGroup => categoryGroup.GetCategoryIds());
//
//         var spendingTransactions = transactions
//             .FilterByCategories(spendingCategoryIds)
//             // .FilterByYear(2024)
//             // .FilterFromMonth(2, 2024)
//             // .FilterUpToMonth(5, 2024)
//             .GroupByMonth()
//             .GroupByFlags()
//             .ToList();
//
//         var expenditureChangesByFlagViewModel = new ExpenditureChangeByFlagCompiler().Compile(spendingTransactions);
//         var expenditureChangesByFlagTable = ConsoleTableCompiler.Compile(expenditureChangesByFlagViewModel);
//         Console.WriteLine(expenditureChangesByFlagTable);
//     }
// }