public abstract class MoveMechanic : MechanicBase
{
    public float MoveSpeed;
    public abstract int direction { get; }

    public override void SetupMechanic(Player player)
    {
    }

    public override void ApplyMechanic(Player player)
    {
        if (GameInputManager.GetKey(MechanicButton))
        {
            if (!player.IsDashing)
            {
                player.HorizontalVelocity += direction * MoveSpeed;
            }
        }
    }

}
