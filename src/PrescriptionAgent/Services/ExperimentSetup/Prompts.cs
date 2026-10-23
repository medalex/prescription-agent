namespace PrescriptionAgent.Services.ExperimentSetup;

/// <summary>The prompt files in prompts.</summary>
public sealed class Prompts(ExperimentPaths paths)
{
    public string Read(string name) => File.ReadAllText(Path.Combine(paths.PromptsDir, name));
}
