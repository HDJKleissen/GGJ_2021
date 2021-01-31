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
    public float MaxPlayerSpeed;
    public float PlayerGravity = 4;
    public float RunSpeedModifier = 0;
    [HideInInspector]
    public float DashSpeed = 0;
    public float PlayerInputLeft, PlayerInputRight, ModifiedHorizontalVelocity;
    public bool IsGrounded;
    public bool IsDashing = false;
    public bool IsRunning = false;
    public bool IsCrouching = false;

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
        foreach (MechanicBase mechanic in GetMechanics(true))
        {
            mechanic.ApplyMechanic();
        }

        IsCrouching = !IsRunning && GameInputManager.GetKey("Crouch");
    }

    void FixedUpdate()
    {
        bool previousIsGrounded = IsGrounded;

        IsGrounded = Physics2D.OverlapArea(GroundedCheckTopLeft.position, GroundedCheckBottomRight.position, GroundLayer);

        if (!IsGrounded)
        {
            // Coyote time, reset sprite to vertical, max height float
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
                playerRigidBody.gravityScale = PlayerGravity;
            }
        }
        // We're on the ground, so reset jump amount (isgrounded && !previousisgrounded)
        else if(!previousIsGrounded)
        {
            if (JumpsRemaining != MaxJumps)
            {
                JumpsRemaining = MaxJumps;
            }
        }

        // If there's input, we want to glide along everything
        if (PlayerInputLeft != 0 || PlayerInputRight != 0)
        {
            playerRigidBody.sharedMaterial.friction = 0f;
            if(PlayerInputLeft != 0)
            {
                facing = -1;
            }
            else if (PlayerInputRight != 0)
            {
                facing = 1;
            }
            // If both keys are pressed, don't change facing.
        }
        else if (IsCrouching)
        {
            // Scootin
            playerRigidBody.sharedMaterial.friction = 0.005f;
        }
        else
        {
            // No input, so set enormously high friction so the player stands still on slopes
            playerRigidBody.sharedMaterial.friction = 9999;
        }

        // Merge horizontal inputs
        ModifiedHorizontalVelocity = PlayerInputRight + PlayerInputLeft;

        if (IsDashing)
        {
            playerRigidBody.sharedMaterial.friction = 0f;
            ModifiedHorizontalVelocity = facing * DashSpeed;
        }
        else if (IsRunning)
        {
            ModifiedHorizontalVelocity *= RunSpeedModifier;
        }
        else if(IsCrouching && ModifiedHorizontalVelocity != 0)
        {
            ModifiedHorizontalVelocity /= 4;
        }

        BodyTransform.localScale = new Vector3(facing, BodyTransform.localScale.y, BodyTransform.localScale.z);
        float newVelocity = 0;

        if(ModifiedHorizontalVelocity < 0)
        {
            newVelocity = Mathf.Lerp(Mathf.Min(ModifiedHorizontalVelocity, playerRigidBody.velocity.x), 0, Time.fixedDeltaTime);
        }
        else if(ModifiedHorizontalVelocity > 0)
        {
            newVelocity = Mathf.Lerp(Mathf.Max(ModifiedHorizontalVelocity, playerRigidBody.velocity.x), 0, Time.fixedDeltaTime);
        }
        else
        {
            newVelocity = Mathf.Lerp(playerRigidBody.velocity.x, 0, Time.fixedDeltaTime);
        }
        playerRigidBody.velocity = new Vector2(newVelocity, Mathf.Max(playerRigidBody.velocity.y, -MaxPlayerFallSpeed));

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
            string currentScene = SceneManager.GetActiveScene().name;

            int currLevelNum = LevelNames.LevelNameToLevelNum[currentScene];
            int nextLevelNum = currLevelNum + 1;

            if (LevelNames.LevelNumToLevelName.ContainsKey(nextLevelNum))
            {
                SceneManager.LoadScene(LevelNames.LevelNumToLevelName[nextLevelNum]);
            }
            else
            {
                SceneManager.LoadScene("GameEndScreen");
            }
        }
        else if(collision.gameObject.tag == "Killbox")
        {
            StartCoroutine(CoroutineHelper.DelaySeconds(() => Die(), 2));
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
}