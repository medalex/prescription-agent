using System.Text.Json;

namespace PrescriptionAgent.Models.OpenAI;

/// <summary>Response tool call. Arguments are a JSON string by the spec; some providers send an object.</summary>
public sealed record ChatResponseFunction(string? Name, JsonElement? Arguments);
