namespace GunboundGameGuard
{
    public interface IGuardModule
    {
        /// <summary>
        /// Registra um evento com a mensagem informada.
        /// </summary>
        /// <param name="message">Mensagem a ser registrada.</param>
        void LogEvent(string message);

        /// <summary>
        /// Inicia o monitoramento ou verificação do módulo.
        /// </summary>
        void StartMonitoring();
    }
}