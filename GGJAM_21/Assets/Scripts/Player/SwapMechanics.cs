using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMechanics : MonoBehaviour
{
    Player player;

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
            Debug.Log("Exit on Collision with Player");
            switch(collision.gameObject.GetComponent<MechanicInstance>().mechanicName)
            {
                case "jump":              
                    StartCoroutine(OnPickUp(player.GetComponent<JumpMechanic>()));
                    break;
                case "moveleft":
                    StartCoroutine(OnPickUp(player.GetComponent<MoveLeftMechanic>()));
                    break;
                case "moveright":
                    StartCoroutine(OnPickUp(player.GetComponent<MoveRightMechanic>()));
                    break;
            }      
        }
    }

    private IEnumerator OnPickUp(MechanicBase newMechanic)
    {
        bool input = false;
        Debug.Log(newMechanic.GetType());
        //should perhaps start a lil  animation or someshit so its clear ur swapping mechanics
        while (true)
        {
            //look for all possible keyinputs
            foreach (string keyMap in GameInputManager.keyMaps)
            {
                if (GameInputManager.GetKeyDown(keyMap))
                {
                    Debug.Log("NEW INPUT KEY SELECTED " + keyMap);
                    SwapMechanicSlot(newMechanic, keyMap);
                    GameInputManager.SetKeyMap(newMechanic.MechanicButton, GameInputManager.keyMapping[keyMap]);

                    input = true;
                }
            }

            if (input)
                break;  
            
            yield return null;
        }
    }

    void SwapMechanicSlot(MechanicBase newMechanic, string MechanicKey)
    {
        bool reuseKey = false;
        int mechanicIndex = 0;
        //stop player, indicate mechanic swap
        //select one gamemechanic by pressing that key
        for (int i = 0; i < player.mechanics.Count; ++i)
        {
            if (player.mechanics[i].MechanicButton == MechanicKey)
            {
                //swap out mechanic
                mechanicIndex = i;
                reuseKey = true;
            }
        }

        //if not reusing a oldkey, remove a random mechanic
        if (!reuseKey)
        {
            mechanicIndex = Random.Range(0, 2);
        }
        
        player.mechanics[mechanicIndex] = newMechanic;
        player.mechanics[mechanicIndex].SetupMechanic(player);
    }
}
