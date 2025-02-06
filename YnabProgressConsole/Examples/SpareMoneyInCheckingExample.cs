// using Ynab.Clients;
// using Ynab.Extensions;
// using YnabProgress.Compilers;
// using YnabProgressConsole.Compilers;
//
// namespace YnabProgressConsole.Examples;
//
// public static class SpareMoneyInCheckingExample
// {
//     public static async Task Run()
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
//         var budgetsClient = new BudgetsClient();
//         var budgets = await budgetsClient.GetBudgets();
//         var budget = budgets.First();
//         
//         var categoryGroups = await budget.GetCategoryGroups();
//         
//         var criticalspendingAllocation = categoryGroups
//             .FilterToCategories(criticalCategoryGroupNames.ToArray())
//             .Sum(cg => cg.Balance);
//         
//         var accounts = await budget.GetCheckingAccounts();
//         var checkingBalance = accounts.Sum(o => o.Balance);
//
//         var afterCriticalSpending = checkingBalance - criticalspendingAllocation;
//
//         var viewModel = new AmountCompiler().Compile(afterCriticalSpending);
//         var table = ConsoleTableCompiler.Compile(viewModel);
//         
//         Console.WriteLine(table);
//     }
// }