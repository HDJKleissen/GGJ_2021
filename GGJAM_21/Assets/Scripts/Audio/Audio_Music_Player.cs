using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Music_Player : MonoBehaviour
{
    [FMODUnity.ParamRef]
    [SerializeField]
    private string ParameterName;
    FMOD.Studio.EventInstance musicInstance;

    // Start is called before the first frame update
    void Start()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Music");
        musicInstance.start();
        musicInstance.release();
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
