using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow.Commands;

public class CliWorkflowCommandProvider(IServiceProvider serviceProvider) : ICliWorkflowCommandProvider
{
    public CliCommand GetCommand(CliInstruction instruction, List<CliCommandOutcome> outcomes)
    {
        var generators = serviceProvider
            .GetKeyedServices<IUnidentifiedCliCommandFactory>(instruction.Name)
            .ToList();
        
        if (generators.Count == 0)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + instruction.Name);
        }
        
        var artefacts = ConvertOutcomesToArtefacts(outcomes);

        var generator = generators.FirstOrDefault(g => g.CanCreateWhen(instruction, artefacts));
        if (generator == null)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + instruction.Name);
        }

        return generator.Create(instruction, artefacts);
    }

    private List<CliCommandArtefact> ConvertOutcomesToArtefacts(List<CliCommandOutcome> priorOutcomes)
    {
        var artefactFactories = serviceProvider.GetServices<ICliCommandArtefactFactory>();
        
        var convertableOutcomes = priorOutcomes
            .Where(priorOutcome => artefactFactories
                .Any(artefactFactory => artefactFactory.CanCreateWhen(priorOutcome)));
        
        return convertableOutcomes
            .Select(priorOutcome => artefactFactories
                .First(artefactFactory => artefactFactory.CanCreateWhen(priorOutcome))
                .Create(priorOutcome))
            .ToList();
    }
}