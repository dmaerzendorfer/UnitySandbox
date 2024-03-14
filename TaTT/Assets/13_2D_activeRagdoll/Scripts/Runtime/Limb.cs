using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace _13_2D_activeRagdoll.Scripts.Runtime
{
    [RequireComponent(typeof(HingeJoint2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Limb : MonoBehaviour
    {
        public float shakeDuration = 2f;
        public float maxTorque = 10;
        public float shakeSpeedScale = 3f;
        public float torqueRandomness = 0.2f; // Amount of randomness to add to the torque
        public float minShakeDelay = 0f;
        public float maxShakeDelay = 0.1f;
        public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 0);
        public RagdollBody Body { get; set; } //limbs know to what body they belong, they are told so by their body

        private HingeJoint2D _hinge;

        public HingeJoint2D Hinge
        {
            get => _hinge;
            private set => _hinge = value;
        }

        private Rigidbody2D _rb2d;

        public Rigidbody2D Rb2d
        {
            get => _rb2d;
            private set => _rb2d = value;
        }

        private void Awake()
        {
            _hinge = GetComponent<HingeJoint2D>();
            _rb2d = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Shakes this limb, starts with a random direction, applies torque to itself, uses the animation curve, scales via its shakeSpeedScale
        /// </summary>
        [ContextMenu("Shake")]
        [Button]
        public void Shake()
        {
            StopAllCoroutines();
            StartCoroutine(RotateCoroutine());
        }

        IEnumerator RotateCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(minShakeDelay, maxShakeDelay));
            float elapsedTime = 0f;

            float startDir = Random.Range(0, 2) * 2 - 1; //random 1 or -1
            while (elapsedTime < shakeDuration)
            {
                float t = (elapsedTime) / shakeDuration;
                // float curveValue = Mathf.Sin(t * 2 * Mathf.PI);
                float curveValue = animationCurve.Evaluate(t);
                float torque = curveValue * maxTorque * startDir;

                // Add randomness to the torque
                torque *= Random.Range(1f - torqueRandomness, 1f + torqueRandomness);

                _rb2d.AddTorque(torque);

                elapsedTime += Time.deltaTime * shakeSpeedScale;
                yield return null;
            }
        }
    }
}