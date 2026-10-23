namespace PrescriptionAgent.Models.Scenario;

public sealed record ClinicalTask(string Id, string Description, List<string> Candidates);
