using System;
using System.Threading;

namespace GunboundGameGuard;

public class PlayerBehaviorModule : IGuardModule
{
    private readonly ServerConfig _config;
    private readonly Random _random = new Random();

    public PlayerBehaviorModule(ServerConfig config)
    {
        _config = config;
    }

    public void Run()
    {
        while (true)
        {
            float accuracy = GetPlayerAccuracy();
            int totalShots = GetTotalShots();

            if (accuracy > 0.95f && totalShots > 50)
            {
                ReportViolation($"Precisão suspeita detectada: {accuracy:P2} em {totalShots} tiros");
            }

            Thread.Sleep(10000); // Analisar a cada 10 segundos
        }
    }

    private float GetPlayerAccuracy()
    {
        // Simulação: Na prática, isso seria calculado com base nos dados reais do jogo
        return (float)_random.NextDouble();
    }

    private int GetTotalShots()
    {
        // Simulação: Na prática, isso seria obtido dos dados reais do jogo
        return _random.Next(0, 100);
    }

    public void ReportViolation(string violation)
    {
        Console.WriteLine($"[PlayerBehavior] {violation}");
    }
}