// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Hosting;
using YnabCli.Sync;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(SyncSetup.Setup)
    .Build();
    
await host.RunAsync();