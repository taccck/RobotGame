using UnityEngine;

public class PlayerBaseStateBehavior : StateMachineBehaviour
{
    protected readonly int WALK_STATE = Animator.StringToHash("Walk");
    protected readonly int JUMP_STATE = Animator.StringToHash("Jump");

    //https://www.youtube.com/watch?v=tjV7E9WITKQ
    private PlayerMovement playerMovement;
    public PlayerMovement GetPlayerMovement(Animator animator) 
    {
        //get player movement from animator

        if (playerMovement == null)
        {
            playerMovement = animator.GetComponentInParent<PlayerMovement>();
        }
        return playerMovement;
    }
}
