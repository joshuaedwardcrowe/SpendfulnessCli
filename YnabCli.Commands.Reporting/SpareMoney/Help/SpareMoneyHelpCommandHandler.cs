using Cli.Commands.Abstractions;
using Cli.Outcomes;
using ConsoleTables;
using YnabCli.Abstractions;
using YnabCli.Commands.Builders;
using YnabCli.Commands.Handlers;

namespace YnabCli.Commands.Reporting.SpareMoney.Help;

public class SpareMoneyHelpCommandHandler(CommandHelpCliTableBuilder commandHelpCliTableBuilder)
    : CommandHandler, ICommandHandler<SpareMoneyHelpCommand>
{
    public Task<CliCommandOutcome> Handle(SpareMoneyHelpCommand request, CancellationToken cancellationToken)
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