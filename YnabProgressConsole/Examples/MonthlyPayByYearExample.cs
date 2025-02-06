// using Ynab.Clients;
// using Ynab.Extensions;
// using YnabProgress.Compilers;
// using YnabProgressConsole.Compilers;
//
// namespace YnabProgressConsole.Examples;
//
// public static class MonthlyPayByYearExample
// {
//     public static async Task Run()
//     {
//         var careerPayees = new List<string> { "Insert company name here" };
//         
//         var budgetsClient = new BudgetsClient();
//         
//         var budgets = await budgetsClient.GetBudgets();
//         var budget = budgets.First();
//         
//         var transactions = await budget.GetTransactions();
//
//         var monthlyPayByYear = transactions
//             .FilterToInflow()
//             .FilterByPayeeName(careerPayees.ToArray())
//             .AverageByYear();
//         
//         var viewModel = new AmountsByYearCompiler().Compile(monthlyPayByYear);
//         var table = ConsoleTableCompiler.Compile(viewModel);
//         
//         Console.WriteLine(table);
//
//         var requestLogViewModel = new RequestLogCompiler().Compile(budgetsClient.RequestRequestLogs);
//         var requestLogTable = ConsoleTableCompiler.Compile(requestLogViewModel);
//         
//         Console.WriteLine(requestLogTable);
//     }
// }