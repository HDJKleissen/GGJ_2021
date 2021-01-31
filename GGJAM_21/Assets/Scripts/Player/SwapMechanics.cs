using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMechanics : MonoBehaviour
{
    Player player;
    PlayerAnimationHandler playerAnimationHandler;
    public MechanicsUI mechanicsUI;

    public int MaxMechanics = 3;
    public int TotalMechanicsActive {
        get { return activeMechanics.Count; }
    }

    Dictionary<string, MechanicBase> nameMechanicPairs = new Dictionary<string, MechanicBase>();
    List<MechanicBase> activeMechanics = new List<MechanicBase>();

    Dictionary<string, string> mechNameToSortingLayer = new Dictionary<string, string>
    {
        {"MoveLeft", "Legs" },
        {"MoveRight", "Legs" },
        {"Run", "Legs" },
        {"Jump", "Jumpers" },
        {"DoubleJump", "Jumpers" },
        {"Dash", "Dashers" },
        {"BackDash", "Dashers" }
    };

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();

        if (mechanicsUI == null)
        {
            MechanicsUI mechanicsUI = GameObject.FindGameObjectWithTag("MechanicsUI").GetComponent<MechanicsUI>();
        }
        foreach (MechanicBase mechanic in player.GetMechanics())
        {
            nameMechanicPairs.Add(mechanic.MechanicButton, mechanic);
            if (mechanic.MechanicIsActive)
            {
                activeMechanics.Add(mechanic);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision found with anything");
        if (collision.gameObject.tag == "mechanic")
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            OnPickUp(collision.gameObject.GetComponent<MechanicInstance>());
        }
    }

    void GainMechanic(MechanicInstance mechanic)
    {
        if(mechanic.mechanicName == "Run")
        {
            if (nameMechanicPairs["MoveLeft"].MechanicIsActive)
            {
                GameObject leftLeg = mechanic.GetChildByName("LeftLeg");
                GameObject leftLegAnim = Instantiate(leftLeg, leftLeg.transform.position, leftLeg.transform.rotation);
                StartMechanicGainAnimation(mechanic.mechanicName,"RunLeft", leftLegAnim);
                Destroy(mechanic.gameObject);
            }
            if (nameMechanicPairs["MoveRight"].MechanicIsActive)
            {
                GameObject rightLeg = mechanic.GetChildByName("RightLeg");
                GameObject rightLegAnim = Instantiate(rightLeg, rightLeg.transform.position, rightLeg.transform.rotation);
                StartMechanicGainAnimation(mechanic.mechanicName, "RunRight", rightLegAnim, nameMechanicPairs["MoveLeft"].MechanicIsActive);
                Destroy(mechanic.gameObject);
            }
        }
        else if (mechanic.mechanicName == "DoubleJump")
        {
            GameObject booster = mechanic.GetChildByName("DoubleJumper");
            GameObject boosterAnim = Instantiate(booster, booster.transform.position, booster.transform.rotation);
            StartMechanicGainAnimation(mechanic.mechanicName, "DoubleJump", boosterAnim);
            Destroy(mechanic.gameObject);
            GameObject spring = mechanic.GetChildByName("Jumper");
            GameObject springAnim = Instantiate(spring, spring.transform.position, spring.transform.rotation);
            StartMechanicGainAnimation(mechanic.mechanicName, "Jump", springAnim, true);
            Destroy(mechanic.gameObject);
        }
        else
        {
            GameObject mechanicAnim = Instantiate(mechanic.gameObject);
            StartMechanicGainAnimation(mechanic.mechanicName, mechanic.mechanicName, mechanicAnim);
            Destroy(mechanic.gameObject);
        }
    }

    private void StartMechanicGainAnimation(string mechanicName, string animationName, GameObject mechanicAnim, bool isExtraPart = false)
    {
        MoveAndRotateTowardsThenDie marttd = mechanicAnim.AddComponent<MoveAndRotateTowardsThenDie>();
        marttd.Destination = player.nameToTransform[animationName];
        marttd.player = player.GetComponent<Rigidbody2D>();
        marttd.RotateSpeed = 13;

        marttd.stringParameters["mechanicName"] = mechanicName;
        marttd.GetComponentInChildren<SpriteRenderer>().sortingLayerName = mechNameToSortingLayer[mechanicName];
        if (!isExtraPart)
        {
            marttd.OnDestroyEvent += ActivateNewPart;
        }
    }

    void ActivateNewPart(MonoBehaviour instance)
    {
        string mechanicName = (instance as MoveAndRotateTowardsThenDie).stringParameters["mechanicName"];
        nameMechanicPairs[mechanicName].MechanicIsActive = true;
        activeMechanics.Add(nameMechanicPairs[mechanicName]);

        //randomonly swap 1 mechanic for now
        if (TotalMechanicsActive > MaxMechanics)
        {
            MechanicBase oldestMechanic = activeMechanics[0];
            LoseMechanic(oldestMechanic);
        }


        playerAnimationHandler.CheckActivatedMechanics();
        mechanicsUI.CheckActivatedMechanics();
    }

    void LoseMechanic(MechanicBase mechanic)
    {
        Debug.Log("lose mechanic");
        mechanic.MechanicIsActive = false;
        activeMechanics.Remove(mechanic);
    }

    private void OnPickUp(MechanicInstance newMechanic)
    {
        //should perhaps start a lil animation or someshit so its clear ur swapping mechanics
        //right now shouldnt need to be inside a coroutine
        if (newMechanic.mechanicName == "DoubleJump")
        {
            if (nameMechanicPairs["Jump"].MechanicIsActive)
            {
                nameMechanicPairs["Jump"].MechanicIsActive = false;
                activeMechanics.Remove(nameMechanicPairs["Jump"]);
            }
        }
        if (newMechanic.mechanicName == "Jump") // Allow downgrade for now
        {
            if (nameMechanicPairs["DoubleJump"].MechanicIsActive)
            {
                nameMechanicPairs["DoubleJump"].MechanicIsActive = false;
                activeMechanics.Remove(nameMechanicPairs["DoubleJump"]);
            }
        }
        if (newMechanic.mechanicName == "Run") // Only pickup Run if we have a leg to stand on
        {
            if (nameMechanicPairs["MoveLeft"].MechanicIsActive || nameMechanicPairs["MoveRight"].MechanicIsActive)
            {
                GainMechanic(newMechanic);
            }
        }
        else
        {
            GainMechanic(newMechanic);
        }
    }
}
