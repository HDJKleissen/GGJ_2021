using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightMechanic : MechanicBase
{
    public float MoveSpeed;
    public override string MechanicButton => "MoveRight";

    public override void SetupMechanic(Player player)
    {
    }

    public override void ApplyMechanic(Player player)
    {
        if (GameInputManager.GetKey(MechanicButton))
        {
            player.horizontalVelocity += 1 * MoveSpeed;
        }
    }

}
