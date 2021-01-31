using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMechanics : MonoBehaviour
{
    Player player;
    PlayerAnimationHandler playerAnimationHandler;

    int nextPickUpId = 0;
    public int MaxMechanics = 3;
    public int TotalMechanicsActive = 0;

    Dictionary<string, MechanicBase> nameMechanicPairs = new Dictionary<string, MechanicBase>();

    public void UpdateTotalMechanicsActive()
    {
        TotalMechanicsActive = 0;
        foreach (MechanicBase m in player.GetMechanics())
        {
            if (m.MechanicIsActive)
                TotalMechanicsActive++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();

        foreach(MechanicBase mechanic in player.GetMechanics())
        {
            nameMechanicPairs.Add(mechanic.MechanicButton, mechanic);
        }

        UpdateTotalMechanicsActive();
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
            StartCoroutine(OnPickUp(collision.gameObject.GetComponent<MechanicInstance>().mechanicName));
            Destroy(collision.gameObject);
        }
    }

    //get form the active mechanics the oldest one that is active
    MechanicBase GetOldestMechanic()
    {
        int smallestId = 0;
        MechanicBase mb = player.GetMechanics()[0];
        foreach(MechanicBase m in player.GetMechanics())
        {
            //skip inactive mechanics
            if (!m.MechanicIsActive)
            {
                Debug.Log("Skipping: " + m);
                continue;
            }

            if(m.pickupOrderId <= smallestId)
            {
                smallestId = m.pickupOrderId;
                mb = m;
            }
        }

        return mb;
    }

    private IEnumerator OnPickUp(string newMechanic)
    {
        bool input = false;

        //should perhaps start a lil animation or someshit so its clear ur swapping mechanics
        //right now shouldnt need to be inside a coroutine
        while (true)
        {
            if (newMechanic == "DoubleJump")
            {
                if (nameMechanicPairs["Jump"].MechanicIsActive)
                {
                    nameMechanicPairs["Jump"].MechanicIsActive = false;
                }
            }
            if (newMechanic == "Jump")
            {
                if (nameMechanicPairs["DoubleJump"].MechanicIsActive)
                {
                    nameMechanicPairs["DoubleJump"].MechanicIsActive = false;
                }
            }
            nameMechanicPairs[newMechanic].MechanicIsActive = true;
            nextPickUpId++;
            nameMechanicPairs[newMechanic].pickupOrderId = nextPickUpId;

            
            //randomonly swap 1 mechanic for now
            UpdateTotalMechanicsActive();
            if (TotalMechanicsActive > MaxMechanics)
            {
                //random
                //player.mechanics[Random.Range(0, player.mechanics.Count - 1)].MechanicIsActive = false;
                GetOldestMechanic().MechanicIsActive = false;
            }


            input = true;

            if (input)
                break;  
            
            yield return null;
        }
        playerAnimationHandler.CheckActivatedMechanics();
    }
}
