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
        if (GameInputManager.GetKeyDown(MechanicButton))
        {
            player.IsRunning = true;
        }

        if (GameInputManager.GetKeyUp(MechanicButton))
        {
            player.IsRunning = false;
        }
    }
    public override void ShutdownMechanic()
    {
    }
}
