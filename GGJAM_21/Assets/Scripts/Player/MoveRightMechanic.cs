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
        // TODO: Bindable keys
        if (Input.GetKey(KeyCode.D))
        {
            player.moveVelocity += new Vector2(MoveSpeed, 0);
        }
    }

}
