using PrescriptionAgent.Models.Policy;

namespace PrescriptionAgent.Models.Scenario;

public sealed class ExperimentData
{
    public required List<Custodian> Custodians { get; init; }
    public required PolicyFile Policy { get; init; }
    public required List<ClinicalTask> Tasks { get; init; }
    public required List<Patient> Patients { get; init; }
    public required string DoctorId { get; init; }

    public List<Custodian> GetCustodiansHolding(string attribute) => Custodians.Where(c => c.Holds(attribute)).ToList();

    public ClinicalTask GetTask(string taskId) =>
        Tasks.FirstOrDefault(t => t.Id == taskId) ?? throw new KeyNotFoundException($"unknown task {taskId}");

    public Patient GetPatient(string label) =>
        Patients.FirstOrDefault(p => p.Label == label) ?? throw new KeyNotFoundException($"unknown patient {label}");

    public DrugRule? FindDrugRule(string drug) => Policy.Drugs.GetValueOrDefault(drug);
}
