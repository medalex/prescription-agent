using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PrescriptionAgent.Configuration;

/// <summary>Fills PrescriptionAgentOptions from the "PrescriptionAgent" configuration section and the environment variables (MFSSIA_URL, AGENT_*).</summary>
public sealed class PrescriptionAgentOptionsSetup(IConfiguration configuration) : IConfigureOptions<PrescriptionAgentOptions>
{
    public const string Section = "PrescriptionAgent";

    public void Configure(PrescriptionAgentOptions options)
    {
        configuration.GetSection(Section).Bind(options);

        if (ReadNonEmpty("MFSSIA_URL") is { } url) options.Mfssia.Url = url;
        if (ReadNonEmpty("AGENT_MODELS") is { } models)
            options.Run.Models = models.Split(',').Select(m => m.Trim()).Where(m => m.Length > 0).ToList();
        if (ReadNonEmpty("AGENT_REPS") is { } reps) options.Run.Reps = int.Parse(reps, CultureInfo.InvariantCulture);
        if (configuration["AGENT_TEMPERATURE"] is { } temperature)
            options.Run.Temperature = temperature.Trim().ToLowerInvariant() is "" or "none" ? null : double.Parse(temperature, CultureInfo.InvariantCulture);
        if (ReadNonEmpty("AGENT_EFFORT") is { } effort) options.Anthropic.Effort = effort;
        if (ReadNonEmpty("AGENT_OPENAI_BASE_URL") is { } baseUrl) options.OpenAi.BaseUrl = baseUrl;
        if (ReadNonEmpty("AGENT_OPENAI_API_KEY") is { } key) options.OpenAi.ApiKey = key;
        if (ReadNonEmpty("AGENT_DIR") is { } dir) options.Data.RootDir = dir;
    }

    private string? ReadNonEmpty(string key) => configuration[key] is { Length: > 0 } v ? v : null;
}
