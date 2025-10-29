using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using SpendfulnessCli.Commands.Builders;

namespace SpendfulnessCli.Commands.Reporting.SpareMoney.Help;

public class SpareMoneyHelpCliCliCommandHandler(CommandHelpCliTableBuilder commandHelpCliTableBuilder)
    : CliCommandHandler, ICliCommandHandler<SpareMoneyHelpCliCommand>
{
    public Task<CliCommandOutcome> Handle(SpareMoneyHelpCliCommand request, CancellationToken cancellationToken)
    {
        var aggregator = new SpareMoneyCommandHelpYnabAggregator();
        
        var viewModel = commandHelpCliTableBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();
        
        var compilation = Compile(viewModel);

        return Task.FromResult<CliCommandOutcome>(compilation);
    }
}