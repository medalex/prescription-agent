namespace PrescriptionAgent.Models.OpenAI;

public sealed record ChatResponseToolCall(string? Id, ChatResponseFunction? Function);
