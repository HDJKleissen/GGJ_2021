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

    public float RunSpeedModifier = 0;
    public float DashSpeed = 0;

    public float HorizontalVelocity;
    public bool IsGrounded;
    public bool IsDashing = false;
    public bool IsRunning = false;

    public List<MechanicBase> mechanics = new List<MechanicBase>();
    public Transform GroundCheckTopLeft, GroundCheckBottomRight;
    public LayerMask GroundLayer;

    // Temporary for showing facing, until sprite
    public Transform Nose;
    public float NoseXPosLeft, NoseXPosRight;
    Rigidbody2D playerRigidBody;
    int facing = 1;

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

        if(facing < 0)
        {
            Nose.localPosition = new Vector3(NoseXPosLeft, Nose.localPosition.y, Nose.localPosition.z);
        }
        else if (facing > 0)
        {
            Nose.localPosition = new Vector3(NoseXPosRight, Nose.localPosition.y, Nose.localPosition.z);
        }
        else
        {
            Debug.Log("wtf facing shouldn't be 0");
        }
    }

    void FixedUpdate()
    {
        if(HorizontalVelocity != 0)
        {
            facing = (int)Mathf.Sign(HorizontalVelocity);
        }
        float modifiedHorizontalVelocity = HorizontalVelocity;
        
        if (IsDashing)
        {
            int direction = facing;
            modifiedHorizontalVelocity = direction * DashSpeed;
        }
        else if (IsRunning)
        {
            modifiedHorizontalVelocity *= RunSpeedModifier;
        }

        playerRigidBody.velocity = new Vector2(modifiedHorizontalVelocity, Mathf.Max(playerRigidBody.velocity.y, -MaxPlayerFallSpeed));

        bool previousIsGrounded = IsGrounded;
        IsGrounded = Physics2D.OverlapArea(GroundCheckTopLeft.position, GroundCheckBottomRight.position, GroundLayer);

        // We're on the ground, so reset jump amount
        if (IsGrounded && JumpsRemaining != MaxJumps)
        {
            JumpsRemaining = MaxJumps;
        }

        if (!IsGrounded)
        {
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
