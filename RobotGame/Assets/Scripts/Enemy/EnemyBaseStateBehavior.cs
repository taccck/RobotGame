using UnityEngine;

public class EnemyBaseStateBehavior : StateMachineBehaviour
{
    //https://www.youtube.com/watch?v=tjV7E9WITKQ
    private EnemyBehavior enemyBehavior; //know what state to be in from enemy behavior
    public EnemyBehavior GetEnemyBehavior(Animator animator)
    {
        if (enemyBehavior == null)
        {
            enemyBehavior = animator.GetComponentInParent<EnemyBehavior>();
        }
        return enemyBehavior;
    }
}
