using UnityEngine;

public abstract class MoveMechanic : MechanicBase
{
    public float MoveSpeed;
    public abstract int direction { get; }

    public override void SetupMechanic()
    {
    }

    public override void ApplyMechanic()
    {
        SetPlayerInput(0);
        if (GameInputManager.GetKey(MechanicButton))
        {
            if (!player.IsDashing)
            {
                Debug.Log("moooooveing");
                SetPlayerInput(direction * MoveSpeed);
            }
        }
    }

    public abstract void SetPlayerInput(float movement);
    public override void ShutdownMechanic()
    {
        SetPlayerInput(0);
    }
}
