using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMechanic : MechanicBase
{
    public float JumpForce;

    public override void SetupMechanic(Player player)
    {
        if(player.MaxJumps < 1)
        {
            player.MaxJumps = 1;
        }
    }

    public override void ApplyMechanic(Player player)
    {
        // TODO: Bindable keys
        if (Input.GetKeyDown(KeyCode.Space) && player.JumpsRemaining > 0)
        {
            player.JumpsRemaining--;
            player.moveVelocity += new Vector2(0, JumpForce);
        }
    }

}
