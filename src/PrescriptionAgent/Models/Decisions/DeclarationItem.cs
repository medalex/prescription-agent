using System.Text.Json.Serialization;
using PrescriptionAgent.Models.Mfssia;

namespace PrescriptionAgent.Models.Decisions;

public sealed class DeclarationItem
{
    public static readonly string[] ObservedValues = ["present", "confirmed_absent", "unknown", "empty"];

    [JsonRequired] public string CustodianId { get; init; } = "";
    [JsonRequired] public string Attribute { get; init; } = "";
    [JsonRequired] public string Observed { get; init; } = "";
    /// <summary>The value relied on, or null; any JSON.</summary>
    public EvidenceValue? ValueUsed { get; init; }
}
