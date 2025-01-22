using System.Diagnostics;

namespace NewsLetters.Api.OpenTelemetry;

internal static class DiagnosticConfig
{
    internal static readonly ActivitySource ActivitySource = new("NewsLetters.Api");
}
