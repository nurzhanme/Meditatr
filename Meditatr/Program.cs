using CliFx;
using Meditatr.Services;
using Microsoft.Extensions.DependencyInjection;

public static class Program
{
    public static async Task<int> Main()
    {
        var services = new ServiceCollection();

        // Register services
        services.AddSingleton<ClassService>();

        var serviceProvider = services.BuildServiceProvider();

        return await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(serviceProvider.GetService)
            .Build()
            .RunAsync();
    }
}
