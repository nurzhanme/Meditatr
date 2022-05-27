using CliFx;
using Meditatr.Commands;
using Meditatr.Services;
using Microsoft.Extensions.DependencyInjection;

public static class Program
{
    public static async Task<int> Main()
    {
        var services = new ServiceCollection();

        // Register services
        services.AddSingleton<ClassService>();

        // Register commands
        services.AddTransient<CommandCreatorCommand>();
        services.AddTransient<QueryCreatorCommand>();
        services.AddTransient<DtoCreatorCommand>();

        var serviceProvider = services.BuildServiceProvider();

        return await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(serviceProvider.GetService)
            .Build()
            .RunAsync();
    }
}
