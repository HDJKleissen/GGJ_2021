using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunMechanic : MechanicBase
{
    public float RunSpeedModifier;

    public override void SetupMechanic(Player player)
    {
        player.RunSpeedModifier = RunSpeedModifier;
    }

    public override void ApplyMechanic(Player player)
    {
        if (Input.GetButtonDown("Run"))
        {
            player.IsRunning = true;
        }

        if (Input.GetButtonUp("Run"))
        {
            player.IsRunning = false;
        }
    }
}
