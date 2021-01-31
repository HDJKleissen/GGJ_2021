public abstract class JumpMechanicAbstract : MechanicBase
{
    public override string MechanicButton => "Jump";

    public float JumpForce;
    bool withinJumpBuffer = false;

    public abstract int JumpAmount { get; }

    public override void SetupMechanic()
    {
    }

    public override void ShutdownMechanic()
    {
    }

    public override void ApplyMechanic()
    {
        if (player.MaxJumps < JumpAmount)
        {
            player.MaxJumps = JumpAmount;
            if (player.IsGrounded)
            {
                player.JumpsRemaining = JumpAmount;
            }
        }
        if (GameInputManager.GetKeyDown(MechanicButton))
        {
            // We have a jump remaining, jump immediately
            if (player.JumpsRemaining > 0)
            {
                player.Jump(JumpForce);
            }
            else
            {
                withinJumpBuffer = true;

                StartCoroutine(CoroutineHelper.DelaySeconds(() =>
                {
                    withinJumpBuffer = false;
                }, player.JumpBufferTime));

                StartCoroutine(CoroutineHelper.Chain(
                    CoroutineHelper.WaitUntil(() => player.IsGrounded),
                    CoroutineHelper.Do(() =>
                    {
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
