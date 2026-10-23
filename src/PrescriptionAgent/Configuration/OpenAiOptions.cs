namespace PrescriptionAgent.Configuration;

/// <summary>Section PrescriptionAgent:OpenAi: OpenAI-compatible APIs ("oai:" models).</summary>
public sealed class OpenAiOptions
{
    /// <summary>AGENT_OPENAI_BASE_URL: required for "oai:" models.</summary>
    public string? BaseUrl { get; set; }

    /// <summary>AGENT_OPENAI_API_KEY; keep it in the environment, not in appsettings.json.</summary>
    public string ApiKey { get; set; } = "none";
}
