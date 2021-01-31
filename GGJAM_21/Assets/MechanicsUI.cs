using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechanicsUI : MonoBehaviour
{
    public Player player;

    Dictionary<MechanicBase, Image> mechanicImagePairs = new Dictionary<MechanicBase, Image>();

    public Image MoveLeft, MoveRight, Run, Jump, DoubleJump, DashForward, DashBack;

    public Color ActivatedColor, DeactivatedColor;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            Debug.LogWarning("Reference player in inspector for Mechanics UI, save a frame.");
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        mechanicImagePairs.Add(player.GetComponent<MoveLeftMechanic>(), MoveLeft);
        mechanicImagePairs.Add(player.GetComponent<MoveRightMechanic>(), MoveRight);
        mechanicImagePairs.Add(player.GetComponent<RunMechanic>(), Run);
        mechanicImagePairs.Add(player.GetComponent<SingleJumpMechanic>(), Jump);
        mechanicImagePairs.Add(player.GetComponent<DoubleJumpMechanic>(), DoubleJump);
        mechanicImagePairs.Add(player.GetComponent<DashMechanic>(), DashForward);
        mechanicImagePairs.Add(player.GetComponent<BackDashMechanic>(), DashBack);

        CheckActivatedMechanics();
    }

    public void CheckActivatedMechanics()
    {
        foreach (KeyValuePair<MechanicBase, Image> kvp in mechanicImagePairs)
        {
            if (kvp.Key.MechanicIsActive)
            {
                kvp.Value.color = ActivatedColor;
            }
            else
            {
                kvp.Value.color = DeactivatedColor;
            }
        }
    }
}
