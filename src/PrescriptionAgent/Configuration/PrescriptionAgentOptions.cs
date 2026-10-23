namespace PrescriptionAgent.Configuration;

/// <summary>
/// Settings of the application. Read from the "PrescriptionAgent" section of appsettings.json, then from the environment
/// variables (MFSSIA_URL, AGENT_MODELS, ...), which win; see PrescriptionAgentOptionsSetup.
/// Command-line options (--models, --reps) override these for one run.
/// </summary>
public sealed class PrescriptionAgentOptions
{
    /// <summary>PrescriptionAgent:Mfssia: where MFSSIA is.</summary>
    public MfssiaOptions Mfssia { get; set; } = new();

    /// <summary>PrescriptionAgent:Run: models, repetitions, temperature.</summary>
    public RunOptions Run { get; set; } = new();

    /// <summary>PrescriptionAgent:Anthropic: settings of Claude models.</summary>
    public AnthropicOptions Anthropic { get; set; } = new();

    /// <summary>PrescriptionAgent:OpenAi: settings of OpenAI-compatible APIs.</summary>
    public OpenAiOptions OpenAi { get; set; } = new();

    /// <summary>PrescriptionAgent:Data: where the data and prompts are.</summary>
    public DataOptions Data { get; set; } = new();
}
