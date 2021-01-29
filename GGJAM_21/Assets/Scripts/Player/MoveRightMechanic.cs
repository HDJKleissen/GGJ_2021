using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightMechanic : MechanicBase
{
    public float MoveSpeed;

    public override void SetupMechanic(Player player)
    {
    }

    public override void ApplyMechanic(Player player)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // TODO: Bindable keys
        if (horizontalInput > 0)
        {
            player.horizontalVelocity += horizontalInput * MoveSpeed;
        }
    }

}
