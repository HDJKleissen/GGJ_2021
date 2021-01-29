using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftMechanic : MechanicBase
{
    public float MoveSpeed;

    public override void SetupMechanic(Player player)
    {
    }

    public override void ApplyMechanic(Player player)
    {
        // TODO: Bindable keys
        if (Input.GetKey(KeyCode.A))
        {
            player.moveVelocity += new Vector2(-MoveSpeed, 0);
        }
    }
}
