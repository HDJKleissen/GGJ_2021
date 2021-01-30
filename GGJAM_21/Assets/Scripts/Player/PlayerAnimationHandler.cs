using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    public GameObject LeftLeg, RightLeg, FastLeftLeg, FastRightLeg, JumpSpring, DoubleJumpBooster, DashForwardBooster, DashBackBooster;
    Animator LeftLegAnimator, RightLegAnimator, FastLeftLegAnimator, FastRightLegAnimator, JumpSpringAnimator, DoubleJumpBoosterAnimator, DashForwardBoosterAnimator, DashBackBoosterAnimator;
    Player player;

    Dictionary<MechanicBase, GameObject> mechanicGOPairs = new Dictionary<MechanicBase, GameObject>();
    MechanicBase runMechanic;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        LeftLegAnimator = LeftLeg.GetComponent<Animator>();
        RightLegAnimator = RightLeg.GetComponent<Animator>();
        FastLeftLegAnimator = FastLeftLeg.GetComponent<Animator>();
        FastRightLegAnimator = FastRightLeg.GetComponent<Animator>();
        JumpSpringAnimator = JumpSpring.GetComponent<Animator>();
        DoubleJumpBoosterAnimator = DoubleJumpBooster.GetComponent<Animator>();
        DashForwardBoosterAnimator = DashForwardBooster.GetComponent<Animator>();
        DashBackBoosterAnimator = DashBackBooster.GetComponent<Animator>();
        
        mechanicGOPairs.Add(player.GetComponent<MoveLeftMechanic>(), LeftLeg);
        mechanicGOPairs.Add(player.GetComponent<MoveRightMechanic>(), RightLeg);
        mechanicGOPairs.Add(player.GetComponent<SingleJumpMechanic>(), JumpSpring);
        mechanicGOPairs.Add(player.GetComponent<DoubleJumpMechanic>(), DoubleJumpBooster);
        mechanicGOPairs.Add(player.GetComponent<DashMechanic>(), DashForwardBooster);
        mechanicGOPairs.Add(player.GetComponent<BackDashMechanic>(), DashBackBooster);
        runMechanic = player.GetComponent<RunMechanic>();

        CheckActivatedMechanics();
    }

    void WalkingAnimation()
    {
        LeftLegAnimator.SetInteger("HorizontalVelocity", player.IsGrounded ? (int)player.ModifiedHorizontalVelocity : 0);
        RightLegAnimator.SetInteger("HorizontalVelocity", player.IsGrounded ? (int)player.ModifiedHorizontalVelocity : 0);
        FastLeftLegAnimator.SetInteger("HorizontalVelocity", player.IsGrounded ? (int)player.ModifiedHorizontalVelocity : 0);
        FastRightLegAnimator.SetInteger("HorizontalVelocity", player.IsGrounded ? (int)player.ModifiedHorizontalVelocity : 0);
        FastLeftLegAnimator.SetBool("Running", player.IsRunning);
        FastRightLegAnimator.SetBool("Running", player.IsRunning);
    } 
    
    public void TriggerAnimation(string name)
    {
        switch (name)
        {
            case "Jump":
                JumpSpringAnimator.SetTrigger(name);
                break;
            case "DoubleJump":
                DoubleJumpBoosterAnimator.SetTrigger(name);
                break;
            case "Dash":
                DashForwardBoosterAnimator.SetTrigger(name);
                break;
            case "BackDash":
                DashBackBoosterAnimator.SetTrigger(name);
                break;
        }
    }

    public void CheckActivatedMechanics()
    {
        FastLeftLeg.SetActive(false);
        FastRightLeg.SetActive(false);
        foreach (KeyValuePair<MechanicBase, GameObject> kvp in mechanicGOPairs)
        {
            if (kvp.Key.MechanicButton.Contains("Move"))
            {
                MoveMechanic moveMechanic = (MoveMechanic)kvp.Key;
                if (runMechanic.MechanicIsActive)
                {
                    kvp.Value.SetActive(false);
                    if(moveMechanic.direction < 0)
                    {
                        FastLeftLeg.SetActive(kvp.Key.MechanicIsActive);
                    }
                    else
                    {
                        FastRightLeg.SetActive(kvp.Key.MechanicIsActive);
                    }
                }
                else
                {
                    kvp.Value.SetActive(kvp.Key.MechanicIsActive);
                    FastLeftLeg.SetActive(false);
                    FastRightLeg.SetActive(false);
                }
            }
            else
            {
                if(kvp.Key.MechanicButton == "DoubleJump" && kvp.Key.MechanicIsActive)
                {
                    JumpSpring.SetActive(true);
                }
                kvp.Value.SetActive(kvp.Key.MechanicIsActive);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        WalkingAnimation();
    }
}
