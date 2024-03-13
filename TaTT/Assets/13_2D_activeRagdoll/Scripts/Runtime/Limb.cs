using System.Collections;
using UnityEngine;

namespace _13_2D_activeRagdoll.Scripts.Runtime
{
    [RequireComponent(typeof(HingeJoint2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Limb : MonoBehaviour
    {
        public float shakeTime = 2f;
        public float maxTorque = 10;
        public float torqueRandomness = 0.2f; // Amount of randomness to add to the torque

        public RagdollBody Body { get; set; } //limbs know to what body they belong, they are told so by their body
        
        private HingeJoint2D _hinge;

        public HingeJoint2D Hinge
        {
            get => _hinge;
            private set => _hinge = value;
        }

        private Rigidbody2D _rb2d;

        private void Awake()
        {
            _hinge = GetComponent<HingeJoint2D>();
            _rb2d = GetComponent<Rigidbody2D>();
        }

        [ContextMenu("Shake")]
        public void Shake()
        {
            StopAllCoroutines();
            StartCoroutine(RotateCoroutine());
        }

        IEnumerator RotateCoroutine()
        {
            float elapsedTime = 0f;

            while (elapsedTime < shakeTime)
            {
                float t = elapsedTime / shakeTime;
                float curveValue = Mathf.Sin(t * 2 * Mathf.PI);
                float torque = curveValue * maxTorque;

                // Add randomness to the torque
                torque *= Random.Range(1f - torqueRandomness, 1f + torqueRandomness);

                _rb2d.AddTorque(torque);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}