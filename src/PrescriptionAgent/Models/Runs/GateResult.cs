using PrescriptionAgent.Models.Mfssia;
using PrescriptionAgent.Models.Policy;

namespace PrescriptionAgent.Models.Runs;

public sealed class GateResult
{
    public Verdict Verdict { get; set; } = new();
    public List<EvidenceResponse> MfssiaResponses { get; } = [];
    public List<(string Custodian, string Attribute)> Queries { get; } = [];
}
