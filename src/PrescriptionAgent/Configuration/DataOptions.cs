namespace PrescriptionAgent.Configuration;

/// <summary>Section PrescriptionAgent:Data: where the experiment data and prompts are.</summary>
public sealed class DataOptions
{
    /// <summary>AGENT_DIR: the folder with data/ and prompts/; found by walking up when null.</summary>
    public string? RootDir { get; set; }
}
