using UnityEngine;

namespace _03_StateMachine.ScriptableObjects.StateLogic
{
    [CreateAssetMenu(fileName = "PlayerMovementState", menuName="Player Logic/Movement")]
    public class PlayerMovementState : BaseStateSO
    {
        protected override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}