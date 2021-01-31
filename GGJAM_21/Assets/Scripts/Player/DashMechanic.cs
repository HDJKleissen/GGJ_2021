using UnityEngine;

// TODO: Fix going up slopes with dash
public class DashMechanic : MechanicBase
{
    public override string MechanicButton => "Dash";

    public float DashSpeed;
    public float DashTime;
    public float DashCooldownTime;
    
    public override void SetupMechanic()
    {
    }

    public override void ApplyMechanic()
    {
        if (Input.GetButtonDown(MechanicButton))
        {
            player.DashSpeed = DashSpeed;
            player.Dash(MechanicButton, DashTime, DashCooldownTime);
        }
    }

    public override void ShutdownMechanic()
    {
    }
}
