using System;

namespace _03_StateMachine.Scripts.Runtime
{
    [Serializable]
    public class MovementState : State
    {
        public int test = 0;

        protected override void OnEnter()
        {
            base.OnEnter();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}