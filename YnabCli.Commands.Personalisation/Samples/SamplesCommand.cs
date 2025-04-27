using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using Ynab;
using YnabCli.Commands.Generators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.Database.SpendingSamples;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Personalisation.Samples;

public class SamplesCommand : ICommand
{
    public static class SubCommandNames
    {
        public const string Matches = "match";
    }
}

public class SamplesMatchesCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string TransactionId = "transactionId";
    }
    
    public string TransactionId { get; set; }
}

public class SamplesCommandGenerator : ICommandGenerator<SamplesCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
        => subCommandName switch
        {
            SamplesCommand.SubCommandNames.Matches => GetMatchesCommand(arguments),
            _ => new SamplesCommand()
        };

    private SamplesMatchesCommand GetMatchesCommand(List<InstructionArgument> arguments)
    {
        var transactionIdArgument = arguments.OfRequiredType<string>(SamplesMatchesCommand.ArgumentNames.TransactionId);

        return new SamplesMatchesCommand
        {
            TransactionId = transactionIdArgument.ArgumentValue
        };
    }
}

public class SamplesMatchesCommandHandler(ConfiguredBudgetClient configuredBudgetClient, YnabCliDb db)
    : ICommandHandler<SamplesMatchesCommand>
{
    public async Task<ConsoleTable> Handle(SamplesMatchesCommand request, CancellationToken cancellationToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        
        // Step 1: Get the transaction
        var transaction = await budget.GetTransaction(request.TransactionId);
        
        // Step 2: Find all sample matches -- flter by payee name, category name, memo.
        // TODO: Move to DB abstraction
        var samples = await db.Context.SpendingSamples
            .Include(x => x.Matches)
            .ThenInclude(x => x.Prices)
            .ToListAsync(cancellationToken);
        
        // TODO: Turn this into an aggregator?
        var matchedSamples = samples
            .Where(sample => sample.Matches(transaction))
            .ToList();
        
        // Step 4: Show in a view model.
        
        throw new NotImplementedException();
    }
}

public static class SpendingSampleExtensions
{
    public static bool Matches(this SpendingSample sample, Transaction transaction)
    {

        var mostRecentSampleTotal = sample.Matches.Sum(x => x.MostRecentPrice.Amount);
        
        var allCategoryIds = sample.Matches
            .Select(x => x.YnabCategoryId)
            .Distinct();
        
        // Payee is the same, though not sure this matters.
        return sample.YnabPayeeId == transaction.PayeeId && 
               
               // Transaction amount equals or can be inclusive
               mostRecentSampleTotal <= transaction.Amount && 
               
               // Transaction is categorised like one of the matches
               transaction.CategoryId.HasValue &&
               allCategoryIds.Contains(transaction.CategoryId.Value);
    }
}