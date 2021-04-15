using UnityEngine;

public class PlayerWalkIdleBehavior : PlayerBaseStateBehavior
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetPlayerMovement(animator).Walking || GetPlayerMovement(animator).Dashing)
        {
            animator.SetBool(WALK_STATE, true);
        }
    }
}
