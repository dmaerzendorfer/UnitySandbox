using UnityEngine.Events;

namespace PathCreation
{
    [System.Serializable]
    public class TrackTrigger
    {
        public bool enabled;
        public float position;
        public UnityEvent trackEvent;

        public TrackTrigger(bool enabled, float position, UnityEvent trackEvent)
        {
            this.enabled = enabled;
            this.position = position;
            this.trackEvent = trackEvent;
        }
    }
}