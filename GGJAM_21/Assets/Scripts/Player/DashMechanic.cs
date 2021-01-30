using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMechanic : MechanicBase
{
    public override string MechanicButton => "Dash";

    public float DashSpeed;
    public float DashTime;
    public float DashCooldownTime;

    bool canDash = true;

    public override void SetupMechanic(Player player)
    {
    }

    public override void ApplyMechanic(Player player)
    {
        if (Input.GetButtonDown(MechanicButton) && canDash)
        {
            player.DashSpeed = DashSpeed;
            Dash(player);
        }
    }

    void Dash(Player player)
    {
        player.IsDashing = true;
        canDash = false;
        StartCoroutine(CoroutineHelper.DelaySeconds(() => player.IsDashing = false, DashTime));
        StartCoroutine(CoroutineHelper.DelaySeconds(() => canDash = true, DashCooldownTime));
    }
}
