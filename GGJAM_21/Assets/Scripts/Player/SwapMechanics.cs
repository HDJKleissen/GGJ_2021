using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMechanics : MonoBehaviour
{
    Player player;
    PlayerAnimationHandler playerAnimationHandler;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
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

    private IEnumerator OnPickUp(string newMechanic)
    {
        bool input = false;

        //should perhaps start a lil animation or someshit so its clear ur swapping mechanics
        //right now shouldnt need to be inside a coroutine
        while (true)
        {
            List<MechanicBase> playerMechanicList = player.GetMechanics();
            //randomonly swap 1 mechanic for now
            int totalMechanicsActive = 0;
            foreach (MechanicBase m in playerMechanicList)
            {
                if (m.MechanicIsActive)
                    totalMechanicsActive++;
            }
            if(totalMechanicsActive >= 3)
            {
                playerMechanicList[Random.Range(0, playerMechanicList.Count - 1)].MechanicIsActive = false;
            }

            //set the new mechanic active
            foreach(MechanicBase m in playerMechanicList)
            {
                if(m.MechanicButton == newMechanic)
                {
                    m.MechanicIsActive = true;
                }
            }

            input = true;

            if (input)
                break;  
            
            yield return null;
        }
        playerAnimationHandler.CheckActivatedMechanics();
    }
}
