using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Export.Csv.PersonalInflationRate;

public class PersonalInflationRateExportCsvCliCommandFactory : ICliCommandFactory<PersonalInflationRateExportCsvCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var outputFileSystemInfoArgument = instruction
            .Arguments
            .OfRequiredType<DirectoryInfo>(PersonalInflationRateExportCsvCliCommand.ArgumentNames.OutputFileSystemPath);
        
        return new PersonalInflationRateExportCsvCliCommand(outputFileSystemInfoArgument.ArgumentValue);
    }
}