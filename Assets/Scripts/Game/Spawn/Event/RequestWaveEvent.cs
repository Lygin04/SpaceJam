using Core.Event.Model;

namespace Game.Spawn.Event
{
    public class RequestWaveEvent : IEventModel
    {
        public const string REQUEST_WAVE = "RequestWave";
        public string EventName => REQUEST_WAVE;

        private readonly int _waveIndex = -1;

        public int WaveIndex => _waveIndex;

        public RequestWaveEvent()
        {
            _waveIndex = -1;
        }

        public RequestWaveEvent(int waveIndex)
        {
            _waveIndex = waveIndex;
        }
    }
}