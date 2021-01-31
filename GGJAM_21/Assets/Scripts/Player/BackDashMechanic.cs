public class BackDashMechanic : DashMechanic
{
    public override string MechanicButton => "BackDash";

    public override void SetupMechanic()
    {
        DashSpeed = -DashSpeed;
    }
}
