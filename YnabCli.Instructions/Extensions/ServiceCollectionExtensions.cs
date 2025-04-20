using Microsoft.Extensions.DependencyInjection;
using YnabCli.Instructions.Builders;
using YnabCli.Instructions.Extraction;
using YnabCli.Instructions.Indexers;
using YnabCli.Instructions.Parsers;

namespace YnabCli.Instructions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInstructions(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IInstructionArgumentBuilder, GuidInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, StringInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, IntInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, DecimalInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, DateOnlyInstructionArgumentBuilder>()
            .AddSingleton<IInstructionArgumentBuilder, BoolInstructionArgumentBuilder>()
            .AddSingleton<InstructionTokenIndexer>()
            .AddSingleton<InstructionTokenExtractor>()
            .AddSingleton<InstructionParser>();
}