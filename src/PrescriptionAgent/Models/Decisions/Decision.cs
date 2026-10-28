using System.Text.Json.Serialization;

namespace PrescriptionAgent.Models.Decisions;

/// <summary>A final decision: the agent's submit_decision arguments (checked by DecisionValidator), or the policy gate's verdict.</summary>
public sealed class Decision
{
    [JsonPropertyName("decision"), JsonRequired] public string Kind { get; init; } = ""; // prescribe | escalate
    public string? Drug { get; init; }
    [JsonRequired] public string Rationale { get; init; } = "";
    /// <summary>The policy attributes the agent names as the cause of an escalation; empty for a prescription.</summary>
    [JsonRequired] public List<string> Blocking { get; init; } = [];
    public List<DeclarationItem>? Declaration { get; init; }
    public List<string>? ReliedOn { get; init; }
    public List<string>? Missing { get; init; }
}
