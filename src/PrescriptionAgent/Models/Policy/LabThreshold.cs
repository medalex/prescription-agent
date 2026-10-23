using System.Text.Json.Serialization;

namespace PrescriptionAgent.Models.Policy;

public sealed record LabThreshold(
    string Attribute,
    double Min,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Aggregation = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? AnyLabUnknown = null);
