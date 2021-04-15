using UnityEngine;

public class PlayerJumpIdleBehavior : PlayerBaseStateBehavior
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetPlayerMovement(animator).Jumping)
        {
            animator.SetBool(JUMP_STATE, true);
        }
    }
}
