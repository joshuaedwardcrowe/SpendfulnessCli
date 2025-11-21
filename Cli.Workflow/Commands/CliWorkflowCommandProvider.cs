using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow.Commands;

public class CliWorkflowCommandProvider(IServiceProvider serviceProvider) : ICliWorkflowCommandProvider
{
    public CliCommand GetCommand(CliInstruction instruction, List<CliCommandOutcome> outcomes)
    {
        var generators = serviceProvider
            .GetKeyedServices<IUnidentifiedCliCommandGenerator>(instruction.Name)
            .ToList();
        
        if (generators.Count == 0)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + instruction.Name);
        }
        
        var properties = ConvertOutcomesToProperties(outcomes);

        var generator = generators.FirstOrDefault(g => g.CanGenerate(instruction, properties));
        if (generator == null)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + instruction.Name);
        }

        return generator.Generate(instruction, properties);
    }

    private List<CliCommandProperty> ConvertOutcomesToProperties(List<CliCommandOutcome> priorOutcomes)
    {
        var propertyFactories = serviceProvider.GetServices<ICliCommandPropertyFactory>();
        
        var convertableOutcomes = priorOutcomes
            .Where(priorOutcome => propertyFactories
                .Any(propertyFactory => propertyFactory.CanCreateProperty(priorOutcome)));
        
        return convertableOutcomes
            .Select(priorOutcome => propertyFactories
                .First(propertyFactory => propertyFactory.CanCreateProperty(priorOutcome))
                .CreateProperty(priorOutcome))
            .ToList();
    }
}