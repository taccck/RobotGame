using UnityEngine;

public class PoliceNormalAtk : PoliceBaseStateBehavior
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetEnemyBehavior(animator).currState = EnemyBehavior.State.attack;
        timer = 0;
    }

    float atkDuration = 3.1f; //how long this atk lasts
    float timer;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (!GetEnemyBehavior(animator).PlayerInBounds()) //if player leaves, go back to idle state
        {
            animator.SetBool(NORMAL_ATK_STATE, false);
            animator.SetBool(WALK_STATE, false);
        }
        else if (timer > atkDuration) //when atk animation is over
        {
            animator.SetBool(NORMAL_ATK_STATE, false);
            animator.SetBool(WALK_STATE, true);
        }
    }
}
