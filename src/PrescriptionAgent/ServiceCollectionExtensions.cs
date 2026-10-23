using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PrescriptionAgent.Adapters;
using PrescriptionAgent.Configuration;
using PrescriptionAgent.Models.Scenario;
using PrescriptionAgent.Reporting;
using PrescriptionAgent.Services.Agents;
using PrescriptionAgent.Services.Experiment;
using PrescriptionAgent.Services.ExperimentSetup;
using PrescriptionAgent.Services.Gate;
using PrescriptionAgent.Services.Mfssia;

namespace PrescriptionAgent;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers the whole application. Program and the tests use the same registrations.</summary>
    public static IServiceCollection AddPrescriptionAgent(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<PrescriptionAgentOptions>();
        services.AddSingleton<IConfigureOptions<PrescriptionAgentOptions>>(new PrescriptionAgentOptionsSetup(configuration));

        services.AddSingleton(CreateJsonOptions());
        services.AddSingleton(sp => new ExperimentPaths(sp.GetRequiredService<IOptions<PrescriptionAgentOptions>>()));
        services.AddSingleton<YamlLoader>();
        services.AddSingleton<Prompts>();
        services.AddSingleton<ExperimentDataLoader>();
        services.AddSingleton(sp => sp.GetRequiredService<ExperimentDataLoader>().Load());

        services.AddSingleton<IMfssia>(sp => new MfssiaClient(sp.GetRequiredService<IOptions<PrescriptionAgentOptions>>(), sp.GetRequiredService<JsonSerializerOptions>()));
        services.AddSingleton<ToolArguments>();
        services.AddSingleton<DecisionValidator>();
        services.AddSingleton<IModelAdapterFactory, ModelAdapterFactory>();
        services.AddSingleton<ITranscriptFactory, TranscriptFactory>();
        services.AddSingleton<Agent>();
        services.AddSingleton<PolicyGate>();
        services.AddSingleton<Preflight>();
        services.AddSingleton(Console.Out);
        services.AddSingleton<Runner>();
        return services;
    }

    /// <summary>
    /// The serializer settings for everything the application writes or reads: snake_case names for its own
    /// records (MFSSIA types name their camelCase fields explicitly), compact output with non-ASCII text kept
    /// as is, nullable annotations enforced on reading. The same text goes to the model as tool results.
    /// </summary>
    private static JsonSerializerOptions CreateJsonOptions() => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        RespectNullableAnnotations = true,
    };
}
