using UnityEngine;

public class RunMechanic : MechanicBase
{
    public override string MechanicButton => "Run";

    public float RunSpeedModifier;

    public override void SetupMechanic()
    {
        player.RunSpeedModifier = RunSpeedModifier;
    }

    public override void ApplyMechanic()
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
    public override void ShutdownMechanic()
    {
    }
}
