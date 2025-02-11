using System;
using System.Windows;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GunboundGameGuard
{
    public partial class MainWindow : Window
    {
        private GunboundGameGuard _gameGuard;
        private ServerConfig _serverConfig;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameGuard();
        }

        private void InitializeGameGuard()
        {
            _serverConfig = new ServerConfig
            {
                ServerName = "Servidor de Teste",
                GameExecutablePath = @"C:\Games\Gunbound\gunbound.exe", // Ajuste este caminho
                FileHashes = new Dictionary<string, string>
                {
                    { "gunbound.exe", "expectedHash1" },
                    { "game_data.dat", "expectedHash2" }
                },
                SuspiciousProcesses = new List<string>
                {
                    "cheatengine",
                    "artmoney",
                    "wireshark"
                },
                AllowedDlls = new List<string>
                {
                    "system.dll",
                    "gunbound_core.dll"
                },
                MemoryCheckInterval = 5000,
                NetworkAnalysisInterval = 15000
            };

            _gameGuard = new GunboundGameGuard(_serverConfig);
            _gameGuard.AddModule(new FileIntegrityModule(_serverConfig));
            _gameGuard.AddModule(new ProcessMonitorModule(_serverConfig));
            _gameGuard.AddModule(new PlayerBehaviorModule(_serverConfig));
            _gameGuard.AddModule(new MemoryIntegrityModule(_serverConfig));
            _gameGuard.AddModule(new DllInjectionModule(_serverConfig));
            _gameGuard.AddModule(new NetworkAnalysisModule(_serverConfig));
        }

        private async void StartGuardButton_Click(object sender, RoutedEventArgs e)
        {
            StartGuardButton.IsEnabled = false;
            LogTextBox.AppendText("Iniciando GameGuard...\n");
            
            await Task.Run(() => _gameGuard.StartAsync());
            
            LogTextBox.AppendText("GameGuard está em execução.\n");
        }

        private void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(_serverConfig.GameExecutablePath);
                LogTextBox.AppendText("Jogo iniciado.\n");
            }
            catch (Exception ex)
            {
                LogTextBox.AppendText($"Erro ao iniciar o jogo: {ex.Message}\n");
            }
        }
    }
}