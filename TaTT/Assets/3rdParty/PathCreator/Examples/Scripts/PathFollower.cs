using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public bool updateRotation = true;
        public bool enableTriggers = true;


        private float _distanceTravelled;
        private List<TrackTrigger> _passedTriggers;
        [SerializeField] private float _lastResolvedT = 0f;

        public enum FollowDirection
        {
            Forwards, //0
            Backwards //1
        }

        private FollowDirection _direction = FollowDirection.Forwards;

        void Start()
        {
            _passedTriggers = new List<TrackTrigger>();
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                _distanceTravelled += speed * Time.deltaTime *
                                      pathCreator.path.GetWeightAtDistance(_distanceTravelled, endOfPathInstruction);
                transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled, endOfPathInstruction);
                if (updateRotation)
                {
                    transform.rotation =
                        pathCreator.path.GetRotationAtDistance(_distanceTravelled, endOfPathInstruction);
                }

                //trigger logic
                List<TrackTrigger> newPassedTriggers;
                //depending on our movement dir
                if (_direction == FollowDirection.Forwards)
                {
                    newPassedTriggers =
                        pathCreator.path.GetPassedTriggersAtDistance(_distanceTravelled, endOfPathInstruction);
                }
                else
                {
                    //not passed triggers since we are now backwards
                    newPassedTriggers =
                        pathCreator.path.GetNotPassedTriggersAtDistance(_distanceTravelled, endOfPathInstruction);
                }

                //iterate triggers
                //new ones get triggerd and added to the passedTriggers
                foreach (var trigger in newPassedTriggers)
                {
                    if (!_passedTriggers.Contains(trigger))
                    {
                        _passedTriggers.Add(trigger);
                        if (enableTriggers && trigger.enabled)
                        {
                            trigger.trackEvent.Invoke();
                        }
                    }
                }


                //if we reach end or start -> and path instruction == loop -> clear backlog
                //if current endOfPath instruction is reverse -> change foward/backward dir
                float resolvedT =
                    pathCreator.path.GetResolvedPercentageAtDistance(_distanceTravelled, endOfPathInstruction);
                bool loopComplete = resolvedT < _lastResolvedT;
                if (_passedTriggers.Count == pathCreator.path.localTriggers.Count && loopComplete)
                {
                    _passedTriggers.Clear();
                }

                _lastResolvedT = resolvedT;
                // if (resolvedT == 0 || resolvedT == 1)
                // {
                //     _direction = _direction == FollowDirection.Forwards
                //         ? FollowDirection.Backwards
                //         : FollowDirection.Forwards;
                //     _passedTriggers.Clear();
                // }
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            _distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}