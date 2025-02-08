using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Instructions.InstructionArgumentBuilders;

namespace YnabProgressConsole.Instructions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleInstructions(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IInstructionArgumentBuilder, StringInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, IntInstructionArgumentBuilder>()
            .AddSingleton<InstructionParser>();

}