using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMechanic : MechanicBase
{
    public override string MechanicButton => "Jump";

    public float JumpForce;
    bool withinJumpBuffer = false;
    int JumpNumber;
    Player player;

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
        if (Input.GetButtonDown(MechanicButton))
        {
            // We have a jump remaining, jump immediately
            if (player.JumpsRemaining > 0)
            {
                player.Jump(JumpForce);
                PlayJumpSound();
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

    void PlayJumpSound()
    {
        SetJumpNumber(player);
        FMOD.Studio.EventInstance jumpSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Jump");
        jumpSound.setParameterByName("JumpNumber", JumpNumber, false);
        jumpSound.start();
        jumpSound.release();
    }

    void SetJumpNumber(Player player)
    {
        JumpNumber = player.MaxJumps - player.JumpsRemaining;
    }
}
