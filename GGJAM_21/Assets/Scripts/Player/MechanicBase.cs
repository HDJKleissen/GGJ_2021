using UnityEngine;

public abstract class MechanicBase : MonoBehaviour
{
    public bool MechanicIsActive = false;
    public int pickupOrderId = 0;
    public abstract string MechanicButton { get; }

    public void Start()
    {
        Player player = GetComponent<Player>();
        if (player != null)
        {
            SetupMechanic(player);
        }
    }

    public abstract void SetupMechanic(Player player);
    public abstract void ApplyMechanic(Player player);
}
