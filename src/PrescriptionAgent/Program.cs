using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrescriptionAgent.Services.Experiment;

namespace PrescriptionAgent;

/// <summary>Entry point: dotnet run --project src/PrescriptionAgent -- [--smoke | report | options]; see Runner.</summary>
public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Configuration: appsettings.json next to the program, then environment variables. The arguments are the
        // runner's own (--smoke, --models ...), not configuration keys, so they are not given to the host.
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings { ContentRootPath = AppContext.BaseDirectory });
        builder.Services.AddPrescriptionAgent(builder.Configuration);
        using var host = builder.Build();
        return await host.Services.GetRequiredService<Runner>().MainAsync(args);
    }
}
