using System;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.IO;

namespace GunboundGameGuard
{
    public class DllInjectionModule : IGuardModule
    {
        private readonly ServerConfig _config;
        private Process? _gameProcess;

        public DllInjectionModule(ServerConfig config)
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

                CheckForInjectedDlls();
                Thread.Sleep(10000); // Verifica a cada 10 segundos
            }
        }

        private void CheckForInjectedDlls()
        {
            if (_gameProcess == null) return;

            _gameProcess.Refresh();
            foreach (ProcessModule module in _gameProcess.Modules)
            {
                if (!IsAllowedModule(module.FileName))
                {
                    ReportViolation($"DLL suspeita detectada: {module.FileName}");
                }
            }
        }

        private bool IsAllowedModule(string modulePath)
        {
            string fileName = Path.GetFileName(modulePath);
            return _config.AllowedDlls.Contains(fileName, StringComparer.OrdinalIgnoreCase);
        }

        public void ReportViolation(string violation)
        {
            Console.WriteLine($"[DllInjection] {violation}");
        }
    }
}