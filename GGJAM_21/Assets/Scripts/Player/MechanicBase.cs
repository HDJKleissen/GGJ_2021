using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechanicBase : MonoBehaviour
{
    public bool MechanicIsActive = false;

    public abstract void SetupMechanic(Player player);
    public abstract void ApplyMechanic(Player player);
}
