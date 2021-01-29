using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMechanic : MechanicBase
{
    public float JumpForce;
    bool withinJumpBuffer = false;

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
        if (Input.GetButtonDown("Jump"))
        {
            // We have a jump remaining, jump immediately
            if (player.JumpsRemaining > 0)
            {
                player.Jump(JumpForce);
            }
            else
            {
                withinJumpBuffer = true;

                StartCoroutine(CoroutineHelper.DelaySeconds(() => {
                    withinJumpBuffer = false;
                    }, player.JumpBufferTime));

                StartCoroutine(CoroutineHelper.Chain(
                    CoroutineHelper.WaitUntil(() => player.isGrounded),
                    CoroutineHelper.Do(() => {
                        if (withinJumpBuffer)
                        {
                            player.Jump(JumpForce);
                            withinJumpBuffer = false;
                        }
                    })
                ));
            }
        }
    }
}
