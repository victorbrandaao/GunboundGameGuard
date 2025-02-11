using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace GunboundGameGuard
{
    public class FileIntegrityModule : IGuardModule
    {
        private readonly ServerConfig _config;

        public event EventHandler<string> LogEvent;

        public FileIntegrityModule(ServerConfig config)
        {
            _config = config;
        }

        public void Run()
        {
            while (true)
            {
                foreach (var file in _config.FileHashes)
                {
                    if (!VerifyFileHash(file.Key, file.Value))
                    {
                        ReportViolation($"Integridade do arquivo comprometida: {file.Key}");
                    }
                }
                Thread.Sleep(60000); // Verificar a cada minuto
            }
        }

        private bool VerifyFileHash(string filePath, string expectedHash)
        {
            if (!File.Exists(filePath))
            {
                ReportViolation($"Arquivo não encontrado: {filePath}");
                return false;
            }

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    string actualHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    return actualHash == expectedHash;
                }
            }
        }

        public void ReportViolation(string violation)
        {
            LogEvent?.Invoke(this, $"[FileIntegrity] {violation}");
        }
    }
}