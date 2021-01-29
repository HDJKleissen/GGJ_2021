using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_AmbiancePlayer : MonoBehaviour
{
    FMOD.Studio.EventInstance ambianceInstance;

    // Start is called before the first frame update
    void Start()
    {
        ambianceInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Ambiance");
        ambianceInstance.start();
        ambianceInstance.release();
    }

    private void OnDestroy()
    {
        ambianceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
