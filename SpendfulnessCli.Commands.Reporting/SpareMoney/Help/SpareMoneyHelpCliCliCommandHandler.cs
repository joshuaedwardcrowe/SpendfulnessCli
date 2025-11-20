using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using SpendfulnessCli.Commands.Builders;

namespace SpendfulnessCli.Commands.Reporting.SpareMoney.Help;

public class SpareMoneyHelpCliCliCommandHandler(CommandHelpCliTableBuilder cliTableBuilder)
    : CliCommandHandler, ICliCommandHandler<SpareMoneyHelpCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(SpareMoneyHelpCliCommand request, CancellationToken cancellationToken)
    {
        var aggregator = new SpareMoneyCommandHelpYnabAggregator();
        
        var table = cliTableBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();
        
        return Task.FromResult(OutcomeAs(table));
    }
}