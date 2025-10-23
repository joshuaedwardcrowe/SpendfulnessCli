using Cli.Instructions.Parsers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Exceptions;
using YnabCli.Abstractions;
using YnabCli.Commands;
using YnabCli.Commands.Generators;

namespace YnabCli;

[Obsolete("Going to create a new one.")]
public class YnabCliApp(IServiceProvider serviceProvider)
{
    public async Task Run()
    {
        var instructionParser = serviceProvider.GetRequiredService<ConsoleInstructionParser>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        
        PrintToConsole("Welcome to YnabCli!");
        
        bool noExitCommandEnteredd = true;

        while (noExitCommandEnteredd)
        {
            try
            {
                PrintToConsole("Enter a Command:");

                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    PrintToConsole("Please enter a Command:");
                    continue;
                }

                var instruction = instructionParser.Parse(input);
                if (instruction.Name is ExitCommand.CommandName or ExitCommand.ShorthandCommandName)
                {
                    PrintToConsole("Exiting...");
                    noExitCommandEnteredd = false;
                    continue;
                }
             
                if (instruction.Name is ClearCommand.CommandName or ClearCommand.ShorthandCommandName)
                {
                    ClearTheConsole();
                    continue;
                }
             
                var generator = serviceProvider.GetKeyedService<IGenericCommandGenerator>(instruction.Name);
                if (generator == null)
                {
                    PrintToConsole("Invalid Command, Try Again...");
                    continue;
                }
             
                var command = generator.Generate(instruction.SubName, instruction.Arguments.ToList());
             
                var table = await mediator.Send(command);

                PrintToConsole(table.ToString());
                
            }
            catch (YnabException ex)
            {
                PrintToConsole(ex.Message);
            }
            catch (YnabCliException ex)
            {
                PrintToConsole(ex.Message);
            }
        }
    }
    
     private static void PrintToConsole(string print)
     {
         Console.WriteLine(print);
         Console.WriteLine();
     }
     
     private static void ClearTheConsole() => Console.Clear();
}
