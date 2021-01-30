using UnityEngine;

public class Audio_Player_Sounds : MonoBehaviour
{

    void PlayFootstepSound ()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Footstep");
    }

    void PlayFirstJumpSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jump");

    }

    void PlaySecondJumpSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SecondJump");

    }
}
