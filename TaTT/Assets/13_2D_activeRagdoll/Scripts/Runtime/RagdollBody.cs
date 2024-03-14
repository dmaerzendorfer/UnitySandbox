using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace _13_2D_activeRagdoll.Scripts.Runtime
{
    public class RagdollBody : MonoBehaviour
    {
        public Rigidbody2D rootRigidbody;
        public List<Limb> limbs;

        private void Awake()
        {
            limbs.ForEach(x => x.Body = this);
        }

        [ContextMenu("FullBodyShake")]
        [Button]
        public void FullBodyShake()
        {
            limbs.ForEach(x => x.Shake());
        }

        public void ApplyRootForce(Vector2 direction, float power, ForceMode2D forceMode = ForceMode2D.Impulse,
            bool zeroOutVelocity = true,
            float rotationRandomness = 0f)
        {
            ApplyForceToRigidbody2D(rootRigidbody, direction, power, forceMode, zeroOutVelocity, rotationRandomness);
        }

        /// <summary>
        /// applies the force to root and all the limbs
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="power"></param>
        /// <param name="forceMode"></param>
        /// <param name="zeroOutVelocity"></param>
        /// <param name="rotationRandomness"></param>
        public void ApplyFullBodyForceImpulse(Vector2 direction, float power,
            ForceMode2D forceMode = ForceMode2D.Impulse, bool zeroOutVelocity = true, float rotationRandomness = 0f)
        {
            ApplyRootForce(direction, power, forceMode, zeroOutVelocity, rotationRandomness);
            foreach (var l in limbs)
            {
                ApplyForceToRigidbody2D(l.Rb2d, direction, power, forceMode, zeroOutVelocity, rotationRandomness);
            }
        }

        /// <summary>
        /// applies a force to a rigidbody2d
        /// </summary>
        /// <param name="rb2d">the rigidbody</param>
        /// <param name="direction">the direction of the force</param>
        /// <param name="power">the power of the force</param>
        /// <param name="forceMode"> force mode to use, per default set to impulse</param>
        /// <param name="zeroOutVelocity">if the current velocity should be set to zero before applying force</param>
        /// <param name="rotationRandomness">the range of randomness added to the direction eG 30deg for +-15deg</param>
        private void ApplyForceToRigidbody2D(Rigidbody2D rb2d, Vector2 direction, float power,
            ForceMode2D forceMode = ForceMode2D.Impulse, bool zeroOutVelocity = true, float rotationRandomness = 0f)
        {
            if (zeroOutVelocity)
                rb2d.velocity = Vector2.zero;

            if (rotationRandomness > 0)
            {
                direction = RotateVector2(direction, rotationRandomness);
            }

            rb2d.AddForce(direction * power, forceMode);
        }

        private Vector2 RotateVector2(Vector2 input, float rotateRange)
        {
            //https://www.youtube.com/watch?v=HH6JzH5pTGo
            //https://stackoverflow.com/questions/73244915/how-can-i-rotate-20-degrees-off-of-a-vector2-direction-in-unity2d
            var randomRotationAngle = Random.Range(-rotateRange / 2, rotateRange / 2);

            return Quaternion.AngleAxis(randomRotationAngle, Vector3.forward) * input;
        }
    }
}