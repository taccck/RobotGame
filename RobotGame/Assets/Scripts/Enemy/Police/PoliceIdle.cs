using UnityEngine;

public class PoliceIdle : PoliceBaseStateBehavior
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetEnemyBehavior(animator).currState = EnemyBehavior.State.idle;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetEnemyBehavior(animator).PlayerInBounds())
        {
            animator.SetBool(WALK_STATE, true);
        }
    }
}
