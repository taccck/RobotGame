using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBehavior : EntityBehavior
{
    public enum State { idle, walk, attack };
    public State currState = State.idle;

    public CameraMomement tripod; //get player through tripod
    public EnemyBounds enemyBounds;
    public PathRequester pathRequester;
    public float walkAcceleration; //in 
    public float StepSpeed { get; set; } //set in attack state behaviors
    Rigidbody rb;
    float r;
    public float radiusOffset; //sometimes this gameobject gets stuck on obstacles, and adding sum to the radius reduces it

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        r = GetComponent<CapsuleCollider>().radius + radiusOffset;
        var main = hitParticles.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World; //particles shouldn't move with the enemy
    }

    void FixedUpdate()
    {
        switch (currState)
        {
            case State.idle:
                break;

            case State.walk:
                MoveTo(GetWalkDirection(tripod.player.transform.position), walkAcceleration); //walk to player
                break;

            case State.attack:
                MoveTo(GetWalkDirection(tripod.player.transform.position), StepSpeed); //step speed is set from animation events
                break;
        }
    }

    readonly float magnitideDiffForNewPath = 1f;
    Vector3 GetWalkDirection(Vector3 walkTo) 
    {
        //returns current vector in path

        if (pathRequester.path == null) //if there is no path make one
        {
            pathRequester.SetPath(transform.position, walkTo, r);
            if (pathRequester.path == null) //if path not found return curr pos to not move
            {
                return transform.position;
            }
        }

        if (Mathf.Abs(pathRequester.path[pathRequester.path.Length - 1].magnitude - tripod.player.transform.position.magnitude) > magnitideDiffForNewPath) //find new path when player moves
        {
            pathRequester.SetPath(transform.position, walkTo, r);
        }

        return pathRequester.GetWalkPoint(transform.position);
    }

    public void SetStepSpeed(float speed) //called from animation events to have varying speed
    {
        StepSpeed = speed;
    }

    public bool PlayerInBounds()
    {
        //if the player is in the enemy bounds

        Vector3 relativePlayerPosition = tripod.player.transform.position - enemyBounds.Position; //get the players position relative to the enemy bounds
        return Mathf.Abs(relativePlayerPosition.x) <= enemyBounds.bounds.x / 2 && Mathf.Abs(relativePlayerPosition.z) <= enemyBounds.bounds.z / 2; //check if player is in the bounds through halfextents sinc player pos can be nagative
    }

    public float DistanceToPlayer()
    {
        Vector2 playerPos = new Vector2(tripod.player.transform.position.x, tripod.player.transform.position.z); //disregard height
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        return Vector2.Distance(playerPos, enemyPos);
    }

    public void MoveTo(Vector3 destination, float moveSpeed)
    {
        //move to the given destiantion

        Vector3 relativePosition = new Vector3(destination.x - transform.position.x, 0, destination.z - transform.position.z);
        RotateTo(relativePosition);
        if (pathRequester.CanMoveTo(Vector3.Distance(Vector3.zero, relativePosition))) //if not too close to the destination
        {
            rb.velocity = transform.forward * moveSpeed;
        }
    }

    public float rotationSpeed;
    public void RotateTo(Vector3 dir)
    {
        float yaw = Mathf.Atan2(dir.z, dir.x) * (180 / -Mathf.PI); //get angle in degrees
        Quaternion yRot = Quaternion.Euler(0, yaw + 90, 0); //i'm not good enough at math to know why i need to add 90
        transform.rotation = Quaternion.Lerp(transform.rotation, yRot, rotationSpeed); //lerp for effect
    }

    public ParticleSystem hitParticles;
    public override bool HitMe(Transform culpit, float dmg)
    {
        base.HitMe(culpit, dmg);
        float yaw = Mathf.Atan2(culpit.position.z - transform.position.z, culpit.position.x - transform.position.x) * (180 / -Mathf.PI);
        hitParticles.transform.parent.rotation = Quaternion.Euler(0, yaw + 90, 0); //make particle system face the player
        hitParticles.Play();
        return false;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player")) //walk into player
        {
            PlayerBehavior pb = collision.gameObject.GetComponent<PlayerBehavior>();
            pb.HitMe(transform, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //hit player with weapon
        {
            PlayerBehavior pb = other.gameObject.GetComponent<PlayerBehavior>();
            pb.HitMe(transform, 0);
        }
    }
}
