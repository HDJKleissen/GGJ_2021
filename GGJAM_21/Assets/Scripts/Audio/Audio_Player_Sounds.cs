using UnityEngine;

public class Audio_Player_Sounds : MonoBehaviour
{

    void PlayFootstepSound ()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Footstep");
    }
}
