using UnityEngine;

public class PlayerJumpBehavior : PlayerBaseStateBehavior
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!GetPlayerMovement(animator).Jumping)
        {
            if (GetPlayerMovement(animator).OnGroundVar) //don't stop the jump anim until on the ground
            {
                animator.SetBool(JUMP_STATE, false);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetPlayerMovement(animator).PlayJumpParticles(); //make particles when landing
    }
}
