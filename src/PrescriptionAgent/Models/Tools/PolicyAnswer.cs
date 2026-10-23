using PrescriptionAgent.Models.Policy;

namespace PrescriptionAgent.Models.Tools;

public sealed record PolicyAnswer(string Drug, string Text, DrugRuleBody Rule, string General, Dictionary<string, List<string>> AllergyClasses);
