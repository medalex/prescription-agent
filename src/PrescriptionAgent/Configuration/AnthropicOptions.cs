namespace PrescriptionAgent.Configuration;

/// <summary>Section PrescriptionAgent:Anthropic. The API key is ANTHROPIC_API_KEY, read by the SDK.</summary>
public sealed class AnthropicOptions
{
    /// <summary>AGENT_EFFORT: low | medium | high | xhigh | max; null = model default.</summary>
    public string? Effort { get; set; }
}
