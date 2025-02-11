using System.Collections.Generic;

namespace GunboundGameGuard;

public class ServerConfig
{
    public required string ServerName { get; init; }
    public required string GameExecutablePath { get; init; }
    public required Dictionary<string, string> FileHashes { get; init; }
    public required List<string> SuspiciousProcesses { get; init; }
    public required List<string> AllowedDlls { get; init; }
    public required int MemoryCheckInterval { get; init; }
    public required int NetworkAnalysisInterval { get; init; }
}