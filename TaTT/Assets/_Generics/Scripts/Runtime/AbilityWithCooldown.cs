using System;
using System.Collections;
using UnityEngine;

namespace _Generics.Scripts.Runtime
{
    public class AbilityWithCooldown
    {
        private MonoBehaviour _user; //needed to start coroutine
        private float _cooldownDuration;
        private Action _abilityAction;
        public bool IsOnCooldown { get; set; } = false;

        public AbilityWithCooldown(MonoBehaviour user, float duration, Action ability)
        {
            _user = user;
            _cooldownDuration = duration;
            _abilityAction = ability;
        }

        /// <summary>
        /// Tries to activate the ability.
        /// </summary>
        /// <returns>true if the ability action was triggered, false if the ability is currently on cooldown</returns>
        public bool ActivateAbility()
        {
            if (!IsOnCooldown)
            {
                _user.StartCoroutine(StartCooldown());
                _abilityAction.Invoke();
                return true;
            }

            return false;
        }


        IEnumerator StartCooldown()
        {
            IsOnCooldown = true;
            yield return new WaitForSeconds(_cooldownDuration);
            IsOnCooldown = false;
        }
    }
}