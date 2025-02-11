using System;

namespace GunboundGameGuard
{
    public interface IGuardModule
    {
        void Run();
        void ReportViolation(string violation);
        event EventHandler<string> LogEvent;
    }
}