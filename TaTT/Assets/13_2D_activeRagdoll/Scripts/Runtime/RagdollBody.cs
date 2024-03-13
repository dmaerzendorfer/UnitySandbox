using System.Collections.Generic;
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
        public void FullBodyShake()
        {
            limbs.ForEach(x => x.Shake());
        }
    }
}