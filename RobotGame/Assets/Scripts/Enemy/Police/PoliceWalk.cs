using UnityEngine;

public class PoliceWalk : PoliceBaseStateBehavior
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetEnemyBehavior(animator).currState = EnemyBehavior.State.walk;
    }

    float lastNormal; //time when this atk was last used
    float normalDelay = 4; //how often this atk can be used in seconds
    float normalDistance = 6; //distance at which this atk can be used from
    float lastSpin;
    float spinDelay = 10;
    float spinDistance = 15;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToPlayer = GetEnemyBehavior(animator).DistanceToPlayer();

        if (!GetEnemyBehavior(animator).PlayerInBounds()) //if player not in bounds go to idle
        {
            animator.SetBool(WALK_STATE, false);
        }
        else if(Time.time - lastNormal > normalDelay && distanceToPlayer < normalDistance) //if enough time has pased since this atk was last used and is close enough to the player, use this atk
        {
            animator.SetBool(WALK_STATE, false);
            animator.SetBool(NORMAL_ATK_STATE, true);
            lastNormal = Time.time;
        }
        else if (Time.time - lastSpin > spinDelay && distanceToPlayer < spinDistance)
        {
            animator.SetBool(WALK_STATE, false);
            animator.SetBool(SPIN_ATK_STATE, true);
            lastSpin = Time.time;
        }
    }
}
