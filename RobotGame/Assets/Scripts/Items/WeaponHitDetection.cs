using UnityEngine;

public class WeaponHitDetection : MonoBehaviour
{
    public WeaponScript weaponScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!other.isTrigger) //can't hit the enemy's weapon
            {
                EnemyBehavior enemyBehavior = other.GetComponent<EnemyBehavior>();
                enemyBehavior.HitMe(transform, weaponScript.dmg);
            }
        }
    }
}
