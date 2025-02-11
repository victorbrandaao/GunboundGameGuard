using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Linq;

namespace GunboundGameGuard;

public class MemoryIntegrityModule : IGuardModule
{
    private readonly ServerConfig _config;
    private Process? _gameProcess;

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

    public MemoryIntegrityModule(ServerConfig config)
    {
        _config = config;
    }

    public void Run()
    {
        while (true)
        {
            if (_gameProcess == null || _gameProcess.HasExited)
            {
                _gameProcess = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_config.GameExecutablePath)).FirstOrDefault();
                if (_gameProcess == null)
                {
                    Thread.Sleep(5000);
                    continue;
                }
            }

            CheckCriticalMemoryRegions();
            Thread.Sleep(_config.MemoryCheckInterval);
        }
    }

    private void CheckCriticalMemoryRegions()
    {
        if (_gameProcess == null) return;

        // Exemplo: verificar um endereço de memória específico
        IntPtr address = new IntPtr(0x12345678); // Endereço de exemplo
        byte[] buffer = new byte[4];
        int bytesRead;

        if (ReadProcessMemory(_gameProcess.Handle, address, buffer, buffer.Length, out bytesRead))
        {
            int value = BitConverter.ToInt32(buffer, 0);
            if (value != 0) // Exemplo de verificação
            {
                ReportViolation($"Valor de memória suspeito detectado: 0x{value:X} no endereço 0x{address.ToInt64():X}");
            }
        }
    }

    public void ReportViolation(string violation)
    {
        Console.WriteLine($"[MemoryIntegrity] {violation}");
    }
}