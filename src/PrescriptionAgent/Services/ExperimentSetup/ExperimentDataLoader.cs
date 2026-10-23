using PrescriptionAgent.Models.Policy;
using PrescriptionAgent.Models.Scenario;

namespace PrescriptionAgent.Services.ExperimentSetup;

/// <summary>Reads data/*.yaml into ExperimentData.</summary>
public sealed class ExperimentDataLoader(ExperimentPaths paths, YamlLoader yaml)
{
    private sealed record CustodiansFile(List<Custodian> Custodians);
    private sealed record TasksFile(List<ClinicalTask> Tasks);
    private sealed record ScenariosFile(string DoctorId, Dictionary<string, Patient> Patients);

    public ExperimentData Load()
    {
        var scenarios = yaml.Load<ScenariosFile>(DataFile("scenarios.yaml"));
        foreach (var (label, patient) in scenarios.Patients) patient.Label = label;
        return new ExperimentData
        {
            Custodians = yaml.Load<CustodiansFile>(DataFile("custodians.yaml")).Custodians,
            Policy = yaml.Load<PolicyFile>(DataFile("policy.yaml")),
            Tasks = yaml.Load<TasksFile>(DataFile("tasks.yaml")).Tasks,
            Patients = scenarios.Patients.Values.ToList(),
            DoctorId = scenarios.DoctorId,
        };
    }

    private string DataFile(string name) => Path.Combine(paths.DataDir, name);
}
