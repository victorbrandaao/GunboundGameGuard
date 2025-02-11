using System;
using System.Diagnostics;
using System.Threading;

namespace GunboundGameGuard;

public class ProcessMonitorModule : IGuardModule
{
    private readonly ServerConfig _config;

    public ProcessMonitorModule(ServerConfig config)
    {
        _config = config;
    }

    public void Run()
    {
        while (true)
        {
            foreach (string processName in _config.SuspiciousProcesses)
            {
                if (Process.GetProcessesByName(processName).Length > 0)
                {
                    ReportViolation($"Processo suspeito detectado: {processName}");
                }
            }
            Thread.Sleep(30000); // Verificar a cada 30 segundos
        }
    }

    public void ReportViolation(string violation)
    {
        Console.WriteLine($"[ProcessMonitor] {violation}");
    }
}