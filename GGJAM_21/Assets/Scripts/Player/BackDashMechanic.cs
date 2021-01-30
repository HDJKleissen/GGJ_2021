using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDashMechanic : DashMechanic
{
    public override string MechanicButton => "BackDash";

    public override void SetupMechanic(Player player)
    {
        DashSpeed = -DashSpeed;
    }
}
