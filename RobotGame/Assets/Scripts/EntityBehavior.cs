using System.Collections;
using UnityEngine;

public class EntityBehavior : MonoBehaviour
{
    //basic structure for gameobjects that can be in combat

    public float hp;
    bool invincible = false;
    public float invincibilityTime; //time gameobjects spends invincible in seconds

    public virtual bool HitMe(Transform culpit, float dmg) 
    {
        //called when owning gameobject gets hit

        if (!invincible) //make invinicble for a period of time so this if statments only gets called once per hit 
        {
            hp -= dmg;
            StartCoroutine(HitWaitTime());
            return true;
        }

        return false;
    }

    IEnumerator HitWaitTime()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
    }
}
