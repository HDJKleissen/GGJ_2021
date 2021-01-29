using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunMechanic : MechanicBase
{
    public float MoveSpeedModifier;

    public override void SetupMechanic(Player player)
    {
    }

    public override void ApplyMechanic(Player player)
    {
        if (Input.GetButton("Run"))
        {
            player.MoveSpeedModifier = MoveSpeedModifier;
        }
    }
}
