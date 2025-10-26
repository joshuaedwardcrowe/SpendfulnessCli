using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Extensions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Spendfulness.Commands.Management;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManagementCommands(this IServiceCollection services)
    {
        services.AddCommandsFromAssembly(typeof(ReconcileRewardAccountCommand).Assembly);

        return services;
    }
}

public class ReconcileRewardAccountCommand : ICliCommand
{
    public string AccountName { get; set; }

    // TODO: There's got to be a way to simplify this.
    public static class ArgumentNames
    {
        public const string AccountName = "accountName";
    }

    public ReconcileRewardAccountCommand(string accountName)
    {
        AccountName = accountName;
    }
}

public class ReconcileRewardAccountCommandGenerator : ICommandGenerator<ReconcileRewardAccountCommand>
{
    public ICliCommand Generate(CliInstruction instruction)
    {
        var accountNameArgument = instruction
            .Arguments
            .OfRequiredType<string>(ReconcileRewardAccountCommand.ArgumentNames.AccountName);

        return new ReconcileRewardAccountCommand(accountNameArgument.ArgumentValue);
    }
}

public class ReconcileRewardAccountCommandHandler : ICliCommandHandler<ReconcileRewardAccountCommand>
{
    public Task<CliCommandOutcome> Handle(ReconcileRewardAccountCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement identifying an Amex reward account first.
        
        throw new NotImplementedException();
    }
}
