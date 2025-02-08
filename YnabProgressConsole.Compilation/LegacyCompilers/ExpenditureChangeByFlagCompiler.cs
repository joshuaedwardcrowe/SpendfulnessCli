// using System.Diagnostics;
// using Ynab.Collections;
// using YnabProgressConsole.Calculators;
// using YnabProgressConsole.ViewModels;
//
// namespace YnabProgressConsole.Compilers;
//
// public class ExpenditureChangeByFlagCompiler : IViewModelCompiler<IEnumerable<FlaggedTransactionsForMonth>>
// {
//     private readonly ViewModel _expenditureChangeByFlagViewModel = new();
//
//     public ViewModel Compile(IEnumerable<FlaggedTransactionsForMonth> data)
//     {
//         var compilerStopWatch = new Stopwatch();
//         compilerStopWatch.Start();
//         
//         var flaggedTransactionsByMonthIndexed = data.ToList();
//         var commonFlags = GetCommonFlags(flaggedTransactionsByMonthIndexed);
//
//         CompileColumns(commonFlags);
//         CompileRows(commonFlags, flaggedTransactionsByMonthIndexed);
//         
//         compilerStopWatch.Stop();
//         _expenditureChangeByFlagViewModel.CompiledIn = compilerStopWatch.ElapsedMilliseconds;
//
//         return _expenditureChangeByFlagViewModel;
//     }
//
//     private void CompileColumns(IEnumerable<string> flags)
//     {
//         var columnNames = new List<string> { "Month" };
//         columnNames.AddRange(flags);
//
//         _expenditureChangeByFlagViewModel.Columns.AddRange(columnNames);
//     }
//
//     private void CompileRows(List<string> flags, List<FlaggedTransactionsForMonth> months)
//     {
//         var firstMonth = months.First();
//         var firstRow = CompileFirstRow(flags, firstMonth);
//         _expenditureChangeByFlagViewModel.Rows.Add(firstRow);
//         
//         for (var i = 1; i < months.Count; i++)
//         {
//             var currentMonth = months[i];
//             var priorMonth = months[i - 1];
//             
//             var otherRow = CompileOtherRow(flags, currentMonth, priorMonth);
//             _expenditureChangeByFlagViewModel.Rows.Add(otherRow);
//         }
//     }
//     
//     private static List<object> CompileFirstRow(List<string> flags, FlaggedTransactionsForMonth currentMonth)
//     {
//         var currentRow = new List<object> { currentMonth.Month };
//
//         foreach (var currentFlag in flags)
//         {
//             var currentMonthFlag = currentMonth.TransactionsForFlags
//                 .FirstOrDefault(t => t.Flag == currentFlag);
//                     
//             var flagSum = currentMonthFlag?.Transactions.Sum(t => t.Amount) ?? 0;
//                     
//             var value = GenerateRowValue(0, flagSum);
//             currentRow.Add(value);
//         }
//                 
//         return currentRow;
//     }
//
//     private static List<object> CompileOtherRow(
//         List<string> flags, 
//         FlaggedTransactionsForMonth priorMonth,
//         FlaggedTransactionsForMonth currentMonth)
//     {
//         var currentRow = new List<object> { currentMonth.Month };
//
//         foreach (var currentFlag in flags)
//         {
//             var currentMonthFlag = currentMonth.TransactionsForFlags
//                 .FirstOrDefault(t => t.Flag == currentFlag);
//
//             var priorMonthFlag = priorMonth.TransactionsForFlags
//                 .FirstOrDefault(p => p.Flag == currentFlag);
//
//             var flagSum = currentMonthFlag?.Transactions.Sum(t => t.Amount) ?? 0;
//             var priorFlagSum = priorMonthFlag?.Transactions.Sum(t => t.Amount) ?? 0;
//
//             var value = GenerateRowValue(priorFlagSum, flagSum);
//             currentRow.Add(value);
//         }
//             
//         return currentRow;
//     }
//     
//     private static List<string> GetCommonFlags(IEnumerable<FlaggedTransactionsForMonth> flaggedTransactionsByMonths)
//         => flaggedTransactionsByMonths
//             .OrderBy(p => p.TransactionsForFlags.Count)
//             .Last()
//             .TransactionsForFlags
//             .Select(t => t.Flag)
//             .ToList();
//
//     private static string GenerateRowValue(decimal priorSum, decimal newSum)
//     {
//         var absPriorSum = Math.Abs(priorSum); 
//         var absNewSum = Math.Abs(newSum);
//     
//         var percentageChange = PercentageCalculator.CalculateChange(absPriorSum, absNewSum);
//         
//         return percentageChange switch
//         {
//             > 0 => $">> £{absNewSum} (+{percentageChange}%)",
//             < 0 => $"<< £{absNewSum} ({percentageChange}%)",
//             _ => $"~ £{absNewSum} ({percentageChange}%)"
//         };
//     }
// }