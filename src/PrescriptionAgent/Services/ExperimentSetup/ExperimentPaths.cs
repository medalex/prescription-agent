using Microsoft.Extensions.Options;
using PrescriptionAgent.Configuration;

namespace PrescriptionAgent.Services.ExperimentSetup;

/// <summary>Location of the experiment data, prompts and output: data, prompts, out.</summary>
public sealed class ExperimentPaths
{
    /// <param name="rootDir">The repository root holding data/ and prompts/.</param>
    public ExperimentPaths(string rootDir) => RootDir = Path.GetFullPath(rootDir);

    public ExperimentPaths(IOptions<PrescriptionAgentOptions> options) : this(options.Value.Data.RootDir ?? Find()) { }

    public string RootDir { get; }
    public string DataDir => Path.Combine(RootDir, "data");
    public string PromptsDir => Path.Combine(RootDir, "prompts");

    /// <summary>Default output folder: out.</summary>
    public string OutDir => Path.Combine(RootDir, "out");

    /// <summary>The first folder with data/scenarios.yaml at or above the application or the working directory.</summary>
    public static string Find()
    {
        foreach (var start in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
        {
            for (var dir = new DirectoryInfo(start); dir != null; dir = dir.Parent)
            {
                if (File.Exists(Path.Combine(dir.FullName, "data", "scenarios.yaml"))) return dir.FullName;
            }
        }
        throw new DirectoryNotFoundException("data/scenarios.yaml not found above the working directory; set AGENT_DIR.");
    }
}
