using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int MaxJumps;
    public int JumpsRemaining = 0;
    public float JumpGraceTime;
    public float JumpBufferTime;
    public float MaxPlayerFallSpeed;
    public float PlayerFloatSpeed;

    public float PlayerGravity = 4;
    public float RunSpeedModifier = 0;
    [HideInInspector]
    public float DashSpeed = 0;
    public float HorizontalVelocity, ModifiedHorizontalVelocity;
    public bool IsGrounded;
    public bool IsDashing = false;
    public bool IsRunning = false;

    List<MechanicBase> mechanics;
    public Transform SpriteTransform, BodyTransform, GroundedCheckTopLeft, GroundedCheckBottomRight;
    public LayerMask GroundLayer;
    public float RotationSpeed;
    public float GroundCheckRayLength;

    Rigidbody2D playerRigidBody;
    BoxCollider2D playerBoxCollider;
    PlayerAnimationHandler playerAnimationHandler;
    int facing = 1;
    bool canDash = true;

    public Quaternion RotationDestination = Quaternion.identity;

    int GetJumpNumber {
        get {
            return MaxJumps - JumpsRemaining;
        }
    }

    public List<MechanicBase> GetMechanics(bool activeOnly = false)
    {
        if(mechanics == null)
        {
            mechanics = new List<MechanicBase>(GetComponents<MechanicBase>());
        }

        if (activeOnly)
        {
            List<MechanicBase> activeMechanics = new List<MechanicBase>();
            foreach (MechanicBase mechanic in mechanics)
            {
                if (mechanic.MechanicIsActive)
                {
                    activeMechanics.Add(mechanic);
                }
            }
            return activeMechanics;
        }
        else
        {
            return mechanics;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // TODO: Don't have all mechanics at start? or something
        mechanics = new List<MechanicBase>(GetComponents<MechanicBase>());
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDashing)
        {
            HorizontalVelocity = 0;
        }
        foreach (MechanicBase mechanic in GetMechanics(true))
        {
            mechanic.ApplyMechanic(this);
        }
    }

    void FixedUpdate()
    {
        bool previousIsGrounded = IsGrounded;

        IsGrounded = Physics2D.OverlapArea(GroundedCheckTopLeft.position, GroundedCheckBottomRight.position, GroundLayer);

        if (!IsGrounded)
        {
            SpriteTransform.localPosition = Vector3.zero;
            SpriteTransform.rotation = Quaternion.identity;
            if (IsGrounded != previousIsGrounded)
            {
                // Fell off a platform, so introduce Coyote Time
                StartCoroutine(CoroutineHelper.DelaySeconds(() =>
                    {
                        // We didnt jump
                        if (JumpsRemaining == MaxJumps && !IsGrounded)
                        {
                            JumpsRemaining = MaxJumps - 1;
                        }
                    }, JumpGraceTime
                ));
            }
            else
            {
                if (GameInputManager.GetKey("Jump") && Mathf.Abs(playerRigidBody.velocity.y) < PlayerFloatSpeed)
                {
                    // Float at the top to give the player more time to land the jump
                    PlayerGravity = 2;
                }
                else
                {
                    PlayerGravity = 4;
                }
            }
        }
        // We're on the ground, so reset jump amount
        else if(!previousIsGrounded)
        {
            if (JumpsRemaining != MaxJumps)
            {
                JumpsRemaining = MaxJumps;
            }
        }
        playerRigidBody.gravityScale = PlayerGravity;

        if (HorizontalVelocity != 0)
        {
            playerRigidBody.sharedMaterial.friction = 0.4f;
            facing = (int)Mathf.Sign(HorizontalVelocity);
        }
        else
        {
            playerRigidBody.sharedMaterial.friction = 9999;
        }
        ModifiedHorizontalVelocity = HorizontalVelocity;

        if (IsDashing)
        {
            playerRigidBody.sharedMaterial.friction = 0f;
            ModifiedHorizontalVelocity = facing * DashSpeed;
        }
        else if (IsRunning)
        {
            ModifiedHorizontalVelocity *= RunSpeedModifier;
        }

        BodyTransform.localScale = new Vector3(facing, BodyTransform.localScale.y, BodyTransform.localScale.z);

        playerRigidBody.velocity = new Vector2(ModifiedHorizontalVelocity, Mathf.Max(playerRigidBody.velocity.y, -MaxPlayerFallSpeed));

        // Hacky shit to have a correct friction on the physics 2D material (HAS BEEN A BUG FOR 5 YEARS FUCK YOU UNITY)
        playerBoxCollider.enabled = false;
        playerBoxCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.2)
        {
            SpriteTransform.rotation = Quaternion.FromToRotation(transform.up, collision.contacts[0].normal);
        }

        if (SpriteTransform.rotation != Quaternion.identity)
        {
            SpriteTransform.localPosition = new Vector3(0, -0.3f, 0);
        }
        else
        {
            SpriteTransform.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            //go next scene
            SceneManager.LoadScene("Level2");
        }
        else if(collision.gameObject.tag == "Killbox")
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Jump(float jumpForce)
    {
        if (JumpsRemaining > 0)
        {
            // TODO The Jumpnumber counter for audio doesn't work if it triggers twice, this happens when DoubleJumpMechanic is active
            PlayJumpSound();
            string animationName = "DoubleJump";
            if(JumpsRemaining == MaxJumps)
            {
                animationName = "Jump";
            }
            playerAnimationHandler.TriggerAnimation(animationName);
            RotationDestination = Quaternion.identity;
            JumpsRemaining--;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
        }
    }

    public void Dash(string animationName, float dashTime, float dashCooldownTime)
    {
        if (canDash)
        {
            IsDashing = true;
            canDash = false;
            StartCoroutine(CoroutineHelper.DelaySeconds(() => IsDashing = false, dashTime));
            StartCoroutine(CoroutineHelper.DelaySeconds(() => canDash = true, dashCooldownTime));
            playerAnimationHandler.TriggerAnimation(animationName);
        }
    }

    void PlayJumpSound()
    {
        int jumpNumber = GetJumpNumber;
        FMOD.Studio.EventInstance jumpSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Jump");
        jumpSound.setParameterByName("JumpNumber", jumpNumber, false);
        jumpSound.start();
        jumpSound.release();
    }
}