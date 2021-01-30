using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMechanics : MonoBehaviour
{
    Player player;

    int nextPickUpId = 0;
    public int maxMechanics = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision found with anything");
        if(collision.gameObject.tag == "mechanic")
        {
            StartCoroutine(OnPickUp(collision.gameObject.GetComponent<MechanicInstance>().mechanicName));
            Destroy(collision.gameObject);
        }
    }

    //get form the active mechanics the oldest one that is active
    MechanicBase GetOldestMechanic()
    {
        int smallestId = 0;
        MechanicBase mb = player.mechanics[0];
        foreach(MechanicBase m in player.mechanics)
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

        //should perhaps start a lil  animation or someshit so its clear ur swapping mechanics
        //right now shouldnt need to be inside a coroutine
        while (true)
        {
            //set the new mechanic active
            foreach(MechanicBase m in player.mechanics)
            {
                if(m.MechanicButton == newMechanic)
                {
                    m.MechanicIsActive = true;
                    nextPickUpId++;
                    m.pickupOrderId = nextPickUpId;
                }
            }

            //randomonly swap 1 mechanic for now
            int totalMechanicsActive = 0;
            foreach (MechanicBase m in player.mechanics)
            {
                if (m.MechanicIsActive)
                    totalMechanicsActive++;
            }

            if(totalMechanicsActive > maxMechanics)
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
    }
}
