using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMechanic : MechanicBase
{
    public override string MechanicButton => "Jump";

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
        if (GameInputManager.GetKeyDown(MechanicButton))
        {
            Debug.Log("Pressing Key: " + MechanicButton);
            // We have a jump remaining, jump immediately
            if (player.JumpsRemaining > 0)
            {
                player.Jump(JumpForce);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jump");
            }
            else
            {
                withinJumpBuffer = true;

                StartCoroutine(CoroutineHelper.DelaySeconds(() => {
                    withinJumpBuffer = false;
                    }, player.JumpBufferTime));

                StartCoroutine(CoroutineHelper.Chain(
                    CoroutineHelper.WaitUntil(() => player.IsGrounded),
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
