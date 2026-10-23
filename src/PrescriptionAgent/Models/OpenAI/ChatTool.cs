namespace PrescriptionAgent.Models.OpenAI;

public sealed record ChatTool(ChatFunction Function, string Type = "function");
