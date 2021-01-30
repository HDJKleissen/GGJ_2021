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
        // TODO: Bindable keys
        if (GameInputManager.GetKey(MechanicButton))
        {
            player.horizontalVelocity += -1 * MoveSpeed;
        }
    }
}
