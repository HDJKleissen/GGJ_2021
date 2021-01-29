using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Just some test vars
    public int MaxJumps;
    public int JumpsRemaining = 0;
    public string playerName;
    public bool isGrounded;
    public Vector2 moveVelocity;

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

        foreach (MechanicBase mechanic in mechanics)
        {
            mechanic.SetupMechanic(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveVelocity = Vector2.zero;

        foreach(MechanicBase mechanic in mechanics)
        {
            if (mechanic.MechanicIsActive)
            {
                mechanic.ApplyMechanic(this);
            }
        }
        
        playerRigidBody.velocity = new Vector2(moveVelocity.x, moveVelocity.y == 0 ? playerRigidBody.velocity.y : moveVelocity.y);
    }

    void FixedUpdate()
    {
        bool previousIsGrounded = isGrounded;
        isGrounded = Physics2D.OverlapArea(GroundCheckTopLeft.position, GroundCheckBottomRight.position, GroundLayer);

        // Did we change from air to ground?
        if (isGrounded && isGrounded != previousIsGrounded)
        {
            JumpsRemaining = MaxJumps;
        }
    }
}
