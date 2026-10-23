using PrescriptionAgent.Models.Mfssia;

namespace PrescriptionAgent.Services.Mfssia;

/// <summary>The application talks to MFSSIA only through these two endpoints.</summary>
public interface IMfssia
{
    string BaseUrl { get; }

    Task<EvidenceResponse> GetEvidenceAsync(string patientId, string doctorId, string custodianId, string attribute, string runId);

    Task<List<AuditEvent>> GetAuditEventsAsync(string runId);
}
