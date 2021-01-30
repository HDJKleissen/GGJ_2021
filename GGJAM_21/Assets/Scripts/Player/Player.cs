using System.Collections;
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
    public float HorizontalVelocity;
    public bool IsGrounded;
    public bool IsDashing = false;
    public bool IsRunning = false;

    public List<MechanicBase> mechanics = new List<MechanicBase>();
    public Transform SpriteTransform, GroundedCheckTopLeft, GroundedCheckBottomRight;
    public LayerMask GroundLayer;
    public float RotationSpeed;
    public float GroundCheckRayLength;

    Rigidbody2D playerRigidBody;
    BoxCollider2D playerBoxCollider;

    int facing = 1;

    public Quaternion RotationDestination = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Don't have all mechanics at start? or something
        mechanics = new List<MechanicBase>(GetComponents<MechanicBase>());
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDashing)
        {
            HorizontalVelocity = 0;
        }
        foreach (MechanicBase mechanic in mechanics)
        {
            if (mechanic.MechanicIsActive)
            {
                mechanic.ApplyMechanic(this);
            }
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
                if (Input.GetButton("Jump") && Mathf.Abs(playerRigidBody.velocity.y) < PlayerFloatSpeed)
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
        float modifiedHorizontalVelocity = HorizontalVelocity;

        if (IsDashing)
        {
            playerRigidBody.sharedMaterial.friction = 0f;
            modifiedHorizontalVelocity = facing * DashSpeed;
        }
        else if (IsRunning)
        {
            modifiedHorizontalVelocity *= RunSpeedModifier;
        }

        SpriteTransform.localScale = new Vector3(facing, SpriteTransform.localScale.y, SpriteTransform.localScale.z);

        playerRigidBody.velocity = new Vector2(modifiedHorizontalVelocity, Mathf.Max(playerRigidBody.velocity.y, -MaxPlayerFallSpeed));

        // Hacky shit to have a correct friction on the physics 2D material (HAS BEEN A BUG FOR 5 YEARS FUCK YOU UNITY)
        playerBoxCollider.enabled = false;
        playerBoxCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0)
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
        
        if(collision.gameObject.tag == "goal")
        {
            //go next scene
            SceneManager.LoadScene("Level2");
        }
    }

    public void Jump(float jumpForce)
    {
        if (JumpsRemaining > 0)
        {
            RotationDestination = Quaternion.identity;
            JumpsRemaining--;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
        }
    }
}