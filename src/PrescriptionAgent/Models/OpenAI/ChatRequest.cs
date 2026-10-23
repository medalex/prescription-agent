using System.Text.Json.Serialization;

namespace PrescriptionAgent.Models.OpenAI;

public sealed record ChatRequest(
    string Model,
    List<ChatMessage> Messages,
    List<ChatTool> Tools,
    string ToolChoice = "auto",
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] double? Temperature = null);
