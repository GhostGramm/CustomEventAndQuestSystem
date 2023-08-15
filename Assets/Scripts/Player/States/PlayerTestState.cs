using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    private float duration;
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        duration = 0f;
        Debug.Log("Entered TestState");
        stateMachine.Input.JumpEvent += TestJump;
    }

    public override void Tick(float deltaTime)
    {
        duration += deltaTime;

        Vector3 moveDir = new Vector3();
        moveDir.x = stateMachine.Input.Movement.x;
        moveDir.y = 0;
        moveDir.z = stateMachine.Input.Movement.y;

        stateMachine.CharacterController.Move(moveDir * deltaTime * stateMachine.FreeSpeed);

        if (moveDir == Vector3.zero)
        {
            stateMachine.Animator.SetFloat("FreeLookLocomotion", 0, 0.1f, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat("FreeLookLocomotion", 1, 0.1f, deltaTime);
        stateMachine.transform.rotation = Quaternion.LookRotation(moveDir);
    }

    public override void Exit()
    {
        Debug.Log("Left TestState");
        stateMachine.Input.JumpEvent -= TestJump;
    }

    public void TestJump()
    {
        GameEventHandler.Instance.OnJumpPressed?.Invoke();
    }

    public void TestMove(Vector2 pos)
    {
        
    }

}
