public class MoveRightMechanic : MoveMechanic
{
    public override string MechanicButton => "MoveRight";
    public override int direction => 1;
    public override void SetPlayerInput(float movement)
    {
        player.PlayerInputRight = movement;
    }
}
