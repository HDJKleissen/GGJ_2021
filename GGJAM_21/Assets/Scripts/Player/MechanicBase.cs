using UnityEngine;

public abstract class MechanicBase : MonoBehaviour
{
    public bool mechanicIsActive = false;
    public bool MechanicIsActive {
        get { return mechanicIsActive; }
        set {
            Debug.Log(MechanicButton + ": " + value);
            if (!value)
            {
                ShutdownMechanic();
            }
            mechanicIsActive = value;
        }
    }

    public abstract string MechanicButton { get; }

    public Player player;

    public void Start()
    {
        player = GetComponent<Player>();
        if (player != null)
        {
            SetupMechanic();
        }
    }

    public abstract void SetupMechanic();
    public abstract void ApplyMechanic();

    public abstract void ShutdownMechanic();
}
