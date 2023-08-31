using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
        if (Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }

        Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(IJumpResetRoutine());
        if (Ctx.JumpCount == 3)
        {
            Ctx.JumpCount = 0;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

    private void HandleJump()
    {
        if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetRoutine != null)
        {
            Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
        }

        Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
        Ctx.IsJumping = true;
        Ctx.JumpCount++;
        Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocities[Ctx.JumpCount];
    }

    private void HandleGravity()
    {
        bool isFalling = Ctx.CurrentMovement.y <= 0.0f || !Ctx.IsJumpPressed;
        if (isFalling)
        {
            //previous velocity stuff for framerate consistent jumps -> verlet integration
            float previousYVelocity = Ctx.CurrentMovement.y;
            Ctx.CurrentMovementY = Ctx.CurrentMovement.y +
                                    (Ctx.JumpGravities[Ctx.JumpCount] * Ctx.FallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovement.y) * .5f, Ctx.MaxFallSpeed);
        }
        else
        {
            //previous velocity stuff for framerate consistent jumps -> verlet integration
            float previousYVelocity = Ctx.CurrentMovement.y;
            Ctx.CurrentMovementY = Ctx.CurrentMovement.y + (Ctx.JumpGravities[Ctx.JumpCount] * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovement.y) * .5f, Ctx.MaxFallSpeed);
        }
    }

    private IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(Ctx.ComboJumpTimeFrame);
        Ctx.JumpCount = 0;
    }
}