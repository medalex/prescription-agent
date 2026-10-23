namespace PrescriptionAgent.Configuration;

/// <summary>Section PrescriptionAgent:Mfssia.</summary>
public sealed class MfssiaOptions
{
    /// <summary>MFSSIA_URL: MFSSIA base URL including the API prefix.</summary>
    public string Url { get; set; } = "http://localhost:3000/api";
}
