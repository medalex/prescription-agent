using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrescriptionAgent.Adapters;
using PrescriptionAgent.Models.Scenario;
using PrescriptionAgent.Services.Agents;
using PrescriptionAgent.Services.ExperimentSetup;
using PrescriptionAgent.Services.Gate;
using PrescriptionAgent.Services.Mfssia;

namespace PrescriptionAgent.Tests;

/// <summary>The application services as Program registers them, with configuration in memory instead of the environment.</summary>
public static class TestHost
{
    /// <param name="config">Configuration keys, e.g. AGENT_MODELS; nothing is read from the process environment.</param>
    /// <param name="replace">Registrations that replace the real ones (fake MFSSIA, scripted models, output).</param>
    public static ServiceProvider Build(IReadOnlyDictionary<string, string?>? config = null, Action<IServiceCollection>? replace = null)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(config ?? new Dictionary<string, string?>()).Build();
        var services = new ServiceCollection().AddPrescriptionAgent(configuration);
        replace?.Invoke(services);
        return services.BuildServiceProvider();
    }

    private static readonly ServiceProvider Shared = Build();

    public static ExperimentData Data => Shared.GetRequiredService<ExperimentData>();
    public static JsonSerializerOptions Json => Shared.GetRequiredService<JsonSerializerOptions>();
    public static YamlLoader Yaml => Shared.GetRequiredService<YamlLoader>();
    public static ToolArguments Arguments => Shared.GetRequiredService<ToolArguments>();
    public static DecisionValidator Validator => Shared.GetRequiredService<DecisionValidator>();

    public static string Serialize(object value) => JsonSerializer.Serialize(value, value.GetType(), Json);

    /// <summary>The agent, talking to the given MFSSIA.</summary>
    public static Agent Agent(IMfssia mfssia) => Build(replace: s => s.AddSingleton(mfssia)).GetRequiredService<Agent>();

    /// <summary>The policy gate, talking to the given MFSSIA.</summary>
    public static PolicyGate Gate(IMfssia mfssia) => Build(replace: s => s.AddSingleton(mfssia)).GetRequiredService<PolicyGate>();
}

/// <summary>Model adapter factory for tests: the adapter for each model id comes from a function.</summary>
public sealed class ScriptedAdapterFactory(Func<string, IModelAdapter> create) : IModelAdapterFactory
{
    public IModelAdapter Create(string modelId) => create(modelId);
}
