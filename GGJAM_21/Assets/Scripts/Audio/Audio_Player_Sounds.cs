using UnityEngine;

public class Audio_Player_Sounds : MonoBehaviour
{

    void PlayFootstepSound ()
    {
        FMOD.Studio.EventInstance Footstep = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Footstep");
        Footstep.start();
        Footstep.release();
    }
}
