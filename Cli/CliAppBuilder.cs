using Cli.Instructions.Abstractions;
using Cli.Workflow.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cli;

public class CliAppBuilder
{
    private readonly ServiceCollection _services = [];
    private IConfigurationRoot _configuration;
    
    public CliAppBuilder WithCli<TCliApp>() where TCliApp : CliApp
    {
        _services.AddCli<TCliApp>();
        
        return this;
    }

    public CliAppBuilder WithJsonSettings(string fileName)
    {
        _services.AddOptions();
        
        var currentDirectory = Directory.GetCurrentDirectory();
        
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(currentDirectory)
            .AddJsonFile(fileName, optional: true, reloadOnChange: true);
        
        _configuration = configurationBuilder.Build();

        return this;
    }
    
    public CliAppBuilder WithSettings<TSettings>() where TSettings : class
    {
        var configurationName = typeof(TSettings)
            .Name
            .Replace("Settings", string.Empty);
        
        var section = _configuration.GetSection(configurationName);
        
        _services.Configure<TSettings>(section);

        return this;
    }

    public CliAppBuilder WithCustomServices(Func<ServiceCollection, IServiceCollection> configureServices)
    {
        configureServices(_services);
        
        return this;
    }

    public async Task Run()
    {
        var serviceProvider = _services.BuildServiceProvider();
        
        var cliApp = serviceProvider.GetRequiredService<CliApp>();
        
        await cliApp.Run();
    }
}