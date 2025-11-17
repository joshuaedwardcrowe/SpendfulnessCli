using Cli.Instructions.Abstractions.Validators;
using Cli.Instructions.Builders;
using Cli.Instructions.Extraction;
using Cli.Instructions.Indexers;
using Cli.Instructions.Parsers;
using Cli.Instructions.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Instructions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCliInstructions(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddCliInstructionArgumentBuilders()
            .AddTokenExtraction()
            .AddSingleton<ICliInstructionParser, CliInstructionParser>()
            .AddValidators();
    
    private static IServiceCollection AddCliInstructionArgumentBuilders(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ICliInstructionArgumentBuilder, GuidCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, StringCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, IntCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, DecimalCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, DateOnlyCliInstructionArgumentBuilder>()
            .AddSingleton<ICliInstructionArgumentBuilder, BoolCliInstructionArgumentBuilder>();

    private static IServiceCollection AddTokenExtraction(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<CliInstructionTokenIndexer>()
            .AddSingleton<CliInstructionTokenExtractor>();
    
    private static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ICliInstructionValidator, DefaultCliInstructionValidator>();
}