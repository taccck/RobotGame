using UnityEngine;

public class PoliceBaseStateBehavior : EnemyBaseStateBehavior
{
    protected static int WALK_STATE = Animator.StringToHash("Walk");
    protected static int NORMAL_ATK_STATE = Animator.StringToHash("NormalAtk");
    protected static int SPIN_ATK_STATE = Animator.StringToHash("SpinAtk");
}
