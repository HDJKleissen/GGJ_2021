using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMechanic : MechanicBase
{
    public float DashSpeed;
    public float DashTime;
    public float DashCooldownTime;

    bool canDash = true;

    public override void SetupMechanic(Player player)
    {
        player.DashSpeed = DashSpeed;
    }

    public override void ApplyMechanic(Player player)
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {
            player.IsDashing = true;
            canDash = false;
            StartCoroutine(CoroutineHelper.DelaySeconds(() => player.IsDashing = false, DashTime));
            StartCoroutine(CoroutineHelper.DelaySeconds(() => canDash = true, DashCooldownTime));
        }
    }
}
