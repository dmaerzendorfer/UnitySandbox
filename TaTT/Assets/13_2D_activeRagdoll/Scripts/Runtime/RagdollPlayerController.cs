using System;
using _Generics.Scripts.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _13_2D_activeRagdoll.Scripts.Runtime
{
    public class RagdollPlayerController : MonoBehaviour
    {
        public RagdollBody ragdollBody;

        [BoxGroup("MovementValues")]
        public float dashPower = 10f;

        [BoxGroup("MovementValues")]
        public float movementSpeed = 2f;

        [BoxGroup("MovementValues")]
        public float movementDeadzone = 0.1f;

        [BoxGroup("MovementValues")]
        [Tooltip(
            "Randomness that is added to normal movement, in degrees, covers both left and right rotation eG 30deg = range of -15 & +15; is not applied to dash")]
        public float moveRandomness = 30;

        [OnValueChanged("UpdateDashCooldown")]
        [BoxGroup("Cooldowns")]
        public float dashCooldown = 1f;

        [OnValueChanged("UpdateShakeCooldown")]
        [BoxGroup("Cooldowns")]
        public float shakeCooldown = 0.1f;

        private Vector2 _inputDirection;

        private AbilityWithCooldown _dashAbility;
        private AbilityWithCooldown _shakeAbility;

        private void Awake()
        {
            _dashAbility = new AbilityWithCooldown(this, dashCooldown,
                () => { ragdollBody.ApplyRootForce(_inputDirection, dashPower); });
            _shakeAbility = new AbilityWithCooldown(this, shakeCooldown, () => { ragdollBody.FullBodyShake(); });
        }

        public void OnInputDirection(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }

        public void Update()
        {
            //apply movement
            if (movementSpeed > 0 && _inputDirection.magnitude > movementDeadzone)
            {
                ragdollBody.ApplyRootForce(_inputDirection, movementSpeed, ForceMode2D.Force, false,
                    moveRandomness);
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _dashAbility.ActivateAbility();
            }
        }

        public void OnShake(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _shakeAbility.ActivateAbility();
            }
        }

        private void UpdateDashCooldown()
        {
            if (_dashAbility != null)
                _dashAbility.CooldownDuration = dashCooldown;
        }

        private void UpdateShakeCooldown()
        {
            if (_shakeAbility != null)
                _shakeAbility.CooldownDuration = shakeCooldown;
        }
    }
}