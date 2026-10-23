namespace PrescriptionAgent.Models.OpenAI;

public sealed record ChatCompletion(List<ChatChoice>? Choices);
