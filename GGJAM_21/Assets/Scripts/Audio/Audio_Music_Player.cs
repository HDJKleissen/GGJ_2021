using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Music_Player : MonoBehaviour
{
    FMOD.Studio.EventInstance musicInstance;
    public float glitches;
    public float bleeps;
    public float beat;

    bool triedToFindPlayer = false;


    public SwapMechanics Player {
        get {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<SwapMechanics>();
                triedToFindPlayer = true;
            }
            return player;
        }
    }

    SwapMechanics player;

    // Start is called before the first frame update
    void Start()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Music");
        musicInstance.start();
        musicInstance.release();
    }

    private void Update()
    {
        checkTheVibe();
    }

    void checkTheVibe ()
    {
        //this needs hooking up to set values for the parameter floats but for now:
        glitches = 0;
        bleeps = 0;
        beat = 0;
        if (Player != null)
        {
            int mechanicsActive = Player.TotalMechanicsActive;

            if (mechanicsActive >= 1)
            {
                glitches = 1;
            }
            if (mechanicsActive >= 2)
            {
                beat = 1;
            }
            if (mechanicsActive >= 3)
            {
                bleeps = 1;
            }
        }

        musicInstance.setParameterByName("GlitchHat", glitches, false);
        musicInstance.setParameterByName("Bleeps", bleeps, false);
        musicInstance.setParameterByName("Beat", beat, false);
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
