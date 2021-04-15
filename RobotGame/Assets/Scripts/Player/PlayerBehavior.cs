using UnityEngine;

public class PlayerBehavior : EntityBehavior
{
    public PlayerMovement playerMovement;

    private void FixedUpdate()
    {
        if (playerMovement.KnockedBack)
        {
            if (Time.time - knockbackTime > invincibilityTime) //stop knockback when player's been knocked back for the duration of invincibility time
            {
                StopKnockback();
            }
        }
    }

    float knockbackTime; //the time the player was hit
    public float knockbackHeight = 1.25f;
    public override bool HitMe(Transform culpit, float dmg)
    {
        if (base.HitMe(culpit, dmg))
        {
            Vector3 knockDir = -(culpit.position - transform.position); //get the culpits poistion relative to the player and reverse it
            knockDir = new Vector3(knockDir.x, 0, knockDir.z).normalized;
            knockDir = new Vector3(knockDir.x, knockbackHeight, knockDir.z); //add height
            playerMovement.KnockbackDir = knockDir; //set player movement variabels to move the player
            playerMovement.canMove = false;
            playerMovement.KnockedBack = true;
            knockbackTime = Time.time;

            return true;
        }

        return false;
    }

    void StopKnockback()
    {
        playerMovement.canMove = true;
        playerMovement.KnockedBack = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy")) // if walked into enemy
        {
            HitMe(hit.transform, 0);
        }
    }
}
