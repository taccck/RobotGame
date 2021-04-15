using System.Collections;
using UnityEngine;

public class WeaponScript : ItemScript
{
    Animator anim;
    MeshCollider meshCollider;
    MeshRenderer meshRenderer;
    public float dmg;

    public override void Start()
    {
        anim = GetComponentInChildren<Animator>();
        meshCollider = GetComponentInChildren<MeshCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        Activate(false);
    }

    bool attacking;
    private void Update()
    {
        Cooldown();
        anim.SetBool("Attacking", attacking);
    }

    float currCooldown;
    void Cooldown()
    {
        if (currCooldown > 0)
        {
            currCooldown -= Time.deltaTime;
        }
        else
        {
            currCooldown = 0;
        }
    }

    public float cooldown; //how long until atk can be called again
    public void Attack() 
    {
        if (currCooldown <= 0)
        {
            currCooldown = cooldown;
            attacking = true;
            Activate(true);
            StartCoroutine(AttackDuration());
        }
    }

    public float atkDuration; //how long the atk lasts
    IEnumerator AttackDuration()
    {
        //keep track of attack duration

        yield return new WaitForSeconds(atkDuration);
        attacking = false;
        Activate(false);
    }

    void Activate(bool active)
    {
        //show when attacking, hide when not

        meshCollider.enabled = active;
        meshRenderer.enabled = active;
    }
}
