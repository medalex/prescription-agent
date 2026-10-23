namespace PrescriptionAgent.Configuration;

/// <summary>Section PrescriptionAgent:Run: what is run, for every provider.</summary>
public sealed class RunOptions
{
    /// <summary>AGENT_MODELS: model ids; "oai:" prefix for OpenAI-compatible APIs.</summary>
    public List<string> Models { get; set; } = [];

    /// <summary>AGENT_REPS: repetitions per cell in a full run.</summary>
    public int Reps { get; set; } = 10;

    /// <summary>AGENT_TEMPERATURE: sent to the model if set; "none" (null) omits it.</summary>
    public double? Temperature { get; set; } = 0;
}
