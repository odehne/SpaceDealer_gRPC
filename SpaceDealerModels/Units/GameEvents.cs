using SpaceDealerModels.Events;
using System;

namespace SpaceDealerModels.Units
{
    public class GameEvents : IGameEvents
    {
        public event Action<AreaScannedEvent> Scanned;
        public event Action<JourneyArrivedEvent> Arrived;
        public event Action<JourneyInterruptedEvent> Interrupted;
        public event Action<ProbeSignalLostEvent> ProbeSignalLost;

        public void Publish(ProbeSignalLostEvent e) => ProbeSignalLost?.Invoke(e);
        public void Publish(AreaScannedEvent e) => Scanned?.Invoke(e);
        public void Publish(JourneyArrivedEvent e) => Arrived?.Invoke(e);
        public void Publish(JourneyInterruptedEvent e) => Interrupted?.Invoke(e);
    }
}
