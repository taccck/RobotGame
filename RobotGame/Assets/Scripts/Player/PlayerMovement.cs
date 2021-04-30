using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    private void Start()
    {
        dashAcceleration *= dashSpeed * Time.deltaTime;
        walkAcceleration *= walkSpeed * Time.deltaTime;
        r = controller.radius;
        var main = jumpParticles.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World; //particles should stay in place when landing from a jump
    }

    private void FixedUpdate()
    {
        //everything that moves the player goes in fixed update

        Walk();

        Jump();

        Dash();

        Gravity();

        Rotate();

        Knockback();
    }

    public bool canMove = true; //if you can take inputs for player movement
    void Update()
    {
        //all inputs go in fixed update

        OnGround();

        if (canMove)
        {
            WalkInput();

            JumpInput();

            DashInput();

            Attack();
        }
    }

    public bool Walking { get; private set; }
    Vector3 facing;
    public CameraMomement tripod;
    void WalkInput()
    {
        //finds out what direction to walk in

        facing = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //set dir from input
        if (facing.magnitude > 0) //if movement was inputed
        {
            double sin = (1 / facing.magnitude) * facing.x; //Law of sines to find out what angle the player should walk in, use atan2 in future
            double angle = (Math.Asin(sin) / Math.PI) * 180;
            if (facing.z < 0) angle = 180 - angle;

            angle += tripod.Yrot;//make relative to camera position

            targetRotation = Quaternion.Euler(0, (float)angle, 0); //see Rotate() for rotating to target rotation

            angle *= (float)(Math.PI / 180); //convert angle back into a positional vector, that position will be walked towards
            double x = facing.magnitude * Math.Sin(angle);
            double z = facing.magnitude * Math.Cos(angle);
            facing = new Vector3((float)x, 0, (float)z);
            facing = facing.normalized; //noralized cus i have no idea how long it is 

            Walking = true; //moves player, see fixed update
        }
        else
        {
            Walking = false;
            facing = transform.forward; //set for dashing
        }
    }


    float Acceleration(float acceleration, float maxSpeed, float currSpeed, bool accelerarte)
    {
        //returns the current speed

        if (accelerarte) //if you should accelerate or not
        {
            if (currSpeed + acceleration < maxSpeed)
                return currSpeed + acceleration;
            else
                return maxSpeed;
        }
        else
        {
            return 0;
        }
    }

    public float walkSpeed;
    public float walkAcceleration = 7.5f; //set in start
    float currWalkSpeed;
    void Walk()
    {
        currWalkSpeed = Acceleration(walkAcceleration, walkSpeed, currWalkSpeed, Walking); //set speed

        //moves the player

        if (canMove && Walking)
        {
            controller.Move(facing * currWalkSpeed * Time.deltaTime); //move to dir, set in WalkInput(), with speed in m/s
        }
    }

    public Quaternion targetRotation { get; private set; } = Quaternion.identity; //rotation to rotate towards
    public float turnRate; //speed of rotation
    void Rotate()
    {
        //rotates the player to the target rotation over a period of time

        if (Mathf.Abs(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y) > .1f) //only rotate when needed
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnRate); //set the players rotation with lerp to smoothly rotate 
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w); //should not rotate on x, z axes
        }
    }

    public bool Jumping { get; private set; }
    public float jumpSpeed;
    public float maxJumpTime;
    float jumpTime;
    const float bonkCoefficient = .5f;
    void JumpInput()
    {
        //ground check, if pressing jump button, get jump time

        if (Input.GetButtonDown("Jump"))
        {
            if (OnGroundVar && canMove)
            {
                Jumping = true;
            }
        }
        else if (Input.GetButtonUp("Jump") || !canMove)
        {
            Jumping = false;
        }

        if (Jumping) jumpTime += Time.deltaTime;
        else jumpTime = 0;

        if (jumpTime >= maxJumpTime)
            Jumping = false;

        if (Physics.CheckBox(new Vector3(0, controller.height + .101f, 0) + transform.position, new Vector3(.4f, .2f, .4f), transform.rotation, ~groundCheckIgnoreMask) && yVel > 0) //if player jumps into a roof
        {
            Debug.Log("bonk");
            jumpTime = maxJumpTime; //can't jump more
            yVel *= bonkCoefficient; //bonk
        }
    }

    void Jump()
    {
        //jumps if jumping is allowed

        if (Jumping)
        {
            yVel = jumpSpeed * Time.deltaTime; // set vertical velocity to jumpForce in m/s
        }
    }

    float yVel;
    void Gravity()
    {
        //applies a simulated force of gravity to the player

        if (!Jumping)
        {
            if (OnGroundVar)
            {
                yVel = -.25f; //if standing on the ground, use a small velocity to make sure the player is on the ground
            }
            else
            {
                if (yVel < 150) //terminal velocity
                {
                    yVel += Physics.gravity.y / 10 * Time.deltaTime; //accelerate the vertical velocity
                }
                else if (yVel > 150)
                {
                    yVel = 150;
                }
            }

        }

        controller.Move(new Vector3(0, yVel, 0)); //move the player with the vertical velocity
    }

    public bool OnGroundVar { get; private set; }
    public LayerMask groundCheckIgnoreMask;
    public float groundCheckLength;
    float r; //set in start
    void OnGround()
    {
        //checks if the player is on the ground

        if (Physics.SphereCast(new Vector3(transform.position.x, transform.position.y + r + .1f, transform.position.z), r, -transform.up, out _, groundCheckLength, ~groundCheckIgnoreMask)) //use sphere cast to see player is on the floor
        {
            OnGroundVar = true; //hit floor
        }
        else
        {
            OnGroundVar = false; //if hit nothing
        }
    }

    public ParticleSystem jumpParticles;
    public void PlayJumpParticles() //called from jump state behavior, when exiting
    {
        jumpParticles.Play();
    }

    public Inventory inventory;
    WeaponScript weaponScript;
    void Attack()
    {
        if (Input.GetButtonDown("Fire"))
        {
            if (inventory.Hand != null)
            {
                if (inventory.Hand.ItemScript.type == ItemFunctionality.ItemType.Weapon) //if hand is a weapon
                {
                    if (weaponScript != inventory.Hand.ItemScript) //if hand has been swaped, get new weapon script
                    {
                        weaponScript = inventory.Hand.ItemScript.GetComponent<WeaponScript>();
                    }
                    weaponScript.Attack();
                }
            }
        }
    }

    public bool Dashing { get; private set; }
    public float dashTime; //how long the dash lasts
    float currDashTime;
    public float dashSpeed;
    float currDashSpeed;
    public float dashAcceleration = 40; //set on start
    void DashInput()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (currDashCooldown <= 0 && canMove)
            {
                Dashing = true;
                dashTrail.emitting = true;
                currDashCooldown = dashCooldown;
            }
        }
    }

    public float dashCooldown; //how long until you can dash again
    float currDashCooldown;
    public TrailRenderer dashTrail;
    void Dash()
    {
        currDashSpeed = Acceleration(dashAcceleration, dashSpeed, currDashSpeed, Dashing); //set speed

        if (currDashCooldown > 0) //dash cooldown
        {
            currDashCooldown -= Time.deltaTime;
            if (currDashCooldown < 0)
            {
                currDashCooldown = 0;
            }
        }

        if (Dashing)
        {
            controller.Move(facing * currDashSpeed * Time.deltaTime); //move in facing direction, set in WalkInput(), with speed in m/s

            currDashTime += Time.deltaTime;
            if (currDashTime >= dashTime) //stop dashing when timer runs out
            {
                currDashTime = 0;
                dashTrail.emitting = false;
                Dashing = false;
            }
        }
    }

    public Vector3 KnockbackDir { get; set; }
    public float knockbackSpeed;
    public bool KnockedBack { get; set; } = false;
    void Knockback()
    {
        //knockback in given direction with given speed in m/s

        if (KnockedBack)
        {
            controller.Move(KnockbackDir * knockbackSpeed * Time.deltaTime);
        }
    }
}
