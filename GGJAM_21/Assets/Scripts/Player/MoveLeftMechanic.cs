public class MoveLeftMechanic : MoveMechanic
{
    public override string MechanicButton => "MoveLeft";
    public override int direction => -1;

    public override void SetPlayerInput(float movement)
    {
        player.PlayerInputLeft = movement;
    }
}
