using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Music_Player : MonoBehaviour
{
    FMOD.Studio.EventInstance musicInstance;
    public float glitches;
    public float bleeps;
    public float beat;

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
        //glitches = 1f;
        //bleeps = 1f;
        //beat = 1f;
        musicInstance.setParameterByName("GlitchHat", glitches, false);
        musicInstance.setParameterByName("Bleeps", bleeps, false);
        musicInstance.setParameterByName("Beat", beat, false);
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
