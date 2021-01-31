using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Sounds : MonoBehaviour
{
    public void PlayConfirmSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UIConfirm");
    }
    public void PlayBackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UIBack");
    }
}
