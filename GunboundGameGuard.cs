using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GunboundGameGuard
{
    public class GunboundGameGuard
    {
        private readonly ServerConfig _serverConfig;
        private readonly List<IGuardModule> _guardModules;
        private readonly CancellationTokenSource _cts;

        public event EventHandler<string> LogEvent;

        public GunboundGameGuard(ServerConfig config)
        {
            _serverConfig = config;
            _guardModules = new List<IGuardModule>();
            _cts = new CancellationTokenSource();
        }

        public void AddModule(IGuardModule module)
        {
            _guardModules.Add(module);
        }

        public IEnumerable<IGuardModule> GetModules()
        {
            return _guardModules;
        }

        public async Task StartAsync()
        {
            Log($"Iniciando GunboundGameGuard para o servidor: {_serverConfig.ServerName}");

            var tasks = _guardModules.Select(module => Task.Run(() => RunModule(module), _cts.Token)).ToList();

            await Task.WhenAll(tasks);
        }

        private async Task RunModule(IGuardModule module)
        {
            try
            {
                await Task.Run(() => module.Run(), _cts.Token);
            }
            catch (Exception ex)
            {
                Log($"Erro no módulo {module.GetType().Name}: {ex.Message}");
            }
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        private void Log(string message)
        {
            LogEvent?.Invoke(this, message);
        }
    }
}