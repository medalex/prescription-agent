using System.Text.Json;

namespace PrescriptionAgent.Models.OpenAI;

public sealed record ChatFunction(string Name, string Description, JsonElement Parameters);
