using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpMechanic : JumpMechanic
{
    public override void SetupMechanic(Player player)
    {
        if(player.MaxJumps < 2)
        {
            player.MaxJumps = 2;
        }
    }
}
