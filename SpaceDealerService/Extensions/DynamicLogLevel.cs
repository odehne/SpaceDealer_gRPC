using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading;

namespace SpaceDealerService.Extensions
{
    public static class DynamicLogLevel
    {
        public static readonly LoggingLevelSwitch Switch = new LoggingLevelSwitch(LogEventLevel.Information);

        private static LogEventLevel _baseline = LogEventLevel.Information;
        private static Timer _revertTimer;
        private static readonly object Gate = new object();

        public static void Initialize(LogEventLevel baseline)
        {
            lock (Gate)
            {
                _baseline = baseline;
                Switch.MinimumLevel = baseline;
            }
        }

        /// <summary>
        /// Raises the level for the given duration, then automatically reverts to the
        /// configured baseline. Calling this again while already elevated restarts the timer.
        /// </summary>
        public static void Elevate(LogEventLevel level, TimeSpan duration)
        {
            lock (Gate)
            {
                Switch.MinimumLevel = level;
                _revertTimer?.Dispose();
                _revertTimer = new Timer(_ => Revert(), null, duration, Timeout.InfiniteTimeSpan);
            }
        }

        public static void Revert()
        {
            lock (Gate)
            {
                Switch.MinimumLevel = _baseline;
                _revertTimer?.Dispose();
                _revertTimer = null;
            }
        }
    }
}
