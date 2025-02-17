using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Instructions.InstructionArgumentBuilders;

namespace YnabProgressConsole.Instructions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleInstructions(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IInstructionArgumentBuilder, BoolInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, StringInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, DecimalInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, IntInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, DateOnlyInstructionArgumentBuilder>()
            .AddSingleton<InstructionTokenParser>()
            .AddSingleton<InstructionParser>();

}