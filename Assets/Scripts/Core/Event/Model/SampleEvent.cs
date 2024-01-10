using Core.Event.Model;

namespace Event.Model
{
    public class SampleEvent : IEventModel
    {
        public const string SampleEventChanged = "sampleEvent";

        public int XP;
        public string EventName => SampleEventChanged;
    }
}