using PathCreation.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PathCreation
{
    [System.Serializable]
    public class TrackTrigger
    {
        [SerializeField] public bool enabled;

        [SerializeField] [Range(0f, 1f)] public float position;

        [SerializeField] public UnityEvent trackEvent;

        [SerializeField] private bool _foldedOut = false;

        [SerializeField] private int _handleId;

        public TrackTrigger(bool enabled, float position, UnityEvent trackEvent)
        {
            this.enabled = enabled;
            this.position = position;
            this.trackEvent = trackEvent;
            _handleId = HandleIds.NextId;
        }

        public int HandleId
        {
            get { return _handleId; }
        }
    }
}