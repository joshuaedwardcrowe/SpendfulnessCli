using Cli.Instructions.Builders;
using Cli.Instructions.Extraction;
using Cli.Instructions.Indexers;
using Cli.Instructions.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Instructions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCliInstructions(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ICliInstructionArgumentBuilder, GuidCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, StringCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, IntCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, DecimalCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, DateOnlyCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, BoolCliInstructionArgumentBuilder>()
            .AddSingleton<CliInstructionTokenIndexer>()
            .AddSingleton<CliInstructionTokenExtractor>()
            .AddSingleton<CliInstructionParser>();
}