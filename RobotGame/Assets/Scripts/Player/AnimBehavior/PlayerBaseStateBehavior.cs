using UnityEngine;

public class PlayerBaseStateBehavior : StateMachineBehaviour
{
    protected static readonly int WALK_STATE = Animator.StringToHash("Walk");
    protected static readonly int JUMP_STATE = Animator.StringToHash("Jump");

    //https://www.youtube.com/watch?v=tjV7E9WITKQ
    private PlayerMovement playerMovement;
    public PlayerMovement GetPlayerMovement(Animator animator)
    {
        //get player movement from animator

        if (playerMovement == null)
        {
            return playerMovement = animator.GetComponent<PlayerMovement>();
        }
        return playerMovement;
    }
}
