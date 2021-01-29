using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Just some test vars
    public int MaxJumps;
    public int JumpsRemaining = 0;
    public float JumpGraceTime;
    public float JumpBufferTime;
    public float MaxPlayerFallSpeed;
    public float PlayerFloatSpeed;

    public float MoveSpeedModifier = 1;

    public float horizontalVelocity;
    public bool isGrounded;

    public List<MechanicBase> mechanics = new List<MechanicBase>();
    public Transform GroundCheckTopLeft, GroundCheckBottomRight;
    public LayerMask GroundLayer;

    Rigidbody2D playerRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Don't have all mechanics at start? or something
        mechanics = new List<MechanicBase>(GetComponents<MechanicBase>());
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalVelocity = 0;
        MoveSpeedModifier = 1;
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
        playerRigidBody.velocity = new Vector2(horizontalVelocity * MoveSpeedModifier, Mathf.Max(playerRigidBody.velocity.y, -MaxPlayerFallSpeed));

        bool previousIsGrounded = isGrounded;
        isGrounded = Physics2D.OverlapArea(GroundCheckTopLeft.position, GroundCheckBottomRight.position, GroundLayer);

        // We're on the ground, so reset jump amount
        if (isGrounded && JumpsRemaining != MaxJumps)
        {
            JumpsRemaining = MaxJumps;
        }

        if (!isGrounded)
        {
            if (isGrounded != previousIsGrounded)
            {
                // Fell off a platform, so introduce Coyote Time
                StartCoroutine(CoroutineHelper.DelaySeconds(() =>
                    {
                    // We didnt jump
                    if (JumpsRemaining == MaxJumps && !isGrounded)
                        {
                            JumpsRemaining = MaxJumps - 1;
                        }
                    }, JumpGraceTime
                ));
            }
            else
            {
                if(Input.GetButton("Jump") && Mathf.Abs(playerRigidBody.velocity.y) < PlayerFloatSpeed)
                {
                    // Float at the top to give the player more time to land the jump
                    playerRigidBody.gravityScale = 2;
                }
                else
                {
                    playerRigidBody.gravityScale = 4;
                }
            }
        }
    }

    public void Jump(float jumpForce)
    {
        if (JumpsRemaining > 0)
        {
            JumpsRemaining--;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
        }
    }
}
