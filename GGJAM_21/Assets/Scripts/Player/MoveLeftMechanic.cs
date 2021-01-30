using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftMechanic : MechanicBase
{
    public float MoveSpeed;

    public override string MechanicButton => "MoveLeft";
    public override void SetupMechanic(Player player)
    {
    }

    public override void ApplyMechanic(Player player)
    {
        if (GameInputManager.GetKey(MechanicButton))
        {
            if (!player.IsDashing)
            {
                player.HorizontalVelocity += -1 * MoveSpeed;
            }
        }
    }
}
