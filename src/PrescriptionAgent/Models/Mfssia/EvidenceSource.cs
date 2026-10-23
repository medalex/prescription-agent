using System.Text.Json.Serialization;

namespace PrescriptionAgent.Models.Mfssia;

public sealed record EvidenceSource(
    [property: JsonPropertyName("custodianId")] string? CustodianId,
    [property: JsonPropertyName("auditEventId")] string? AuditEventId);
